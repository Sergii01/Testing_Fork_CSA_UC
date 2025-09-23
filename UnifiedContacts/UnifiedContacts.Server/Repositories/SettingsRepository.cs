using Dapper;
using Microsoft.Data.SqlClient;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Repositories
{
    public class SettingsRepository : RepositoryBase
    {
        private readonly AuthSettings _authSettings;
        private readonly RuntimeInfoDto _runtimeInfo;

        public SettingsRepository(AuthSettings authSettings, RuntimeInfoDto runtimeInfo) : base(runtimeInfo)
        {
            _authSettings = authSettings;
            _runtimeInfo = runtimeInfo;
        }

        /// <summary>
        /// Gets the settings value of a specific settings key from a specific category.
        /// </summary>
        /// <param name="key">Settings key</param>
        /// <param name="category">Settings category</param>
        /// <returns>Settings value</returns>
        public async Task<string?> GetValue(string key, string category)
        {
            VerifyDatabaseConfiguration();

            AdminSettingsDB? setting;
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                setting = (await connection.QueryAsync<AdminSettingsDB>("SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = @key AND [category] = @category", new { key = key, category = category })).FirstOrDefault();
            }
            if (setting == null)
            {
                return null;
            }
            return setting.Value;
        }

        /// <summary>
        /// Gets the settings value of a specific settings key from a specific category.
        /// </summary>
        /// <param name="category">Settings category</param>
        /// <returns>Settings value</returns>
        public async Task<IEnumerable<AdminSettingsDB>> GetValues(string category)
        {
            VerifyDatabaseConfiguration();

            IEnumerable<AdminSettingsDB> setting;
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                setting = await connection.QueryAsync<AdminSettingsDB>("SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [category] = @category", new { category = category });
            }

            return setting;
        }

        /// <summary>
        /// Sets the settings value of a specific settings key of a specific category. This operation is executed as insert or update
        /// </summary>
        /// <param name="key">Settings key</param>
        /// <param name="category">Settings category</param>
        /// <param name="value">Settings value</param>
        /// <returns>Settings value</returns>
        public async Task SetValue(string key, string category, string? value)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertOrUpdate(new AdminSettingsDB(key, category, value));
            }
        }

        public async Task<ManifestSettingsDto> GetManifestSettings()
        {
            VerifyDatabaseConfiguration();

            ManifestSettingsDto manifestSettings = new ManifestSettingsDto();
            IEnumerable<AdminSettingsDB> adminSettings;
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                adminSettings = await connection.QueryAsync<AdminSettingsDB>("SELECT * FROM UnifiedContacts.AdminSettings WHERE category = 'teamsManifest'");
            }
            foreach (AdminSettingsDB adminSetting in adminSettings)
            {
                switch (adminSetting.Key)
                {
                    case "displayName":
                        manifestSettings.DisplayName = adminSetting.Value;
                        break;

                    case "shortDescription":
                        manifestSettings.ShortDescription = adminSetting.Value;
                        break;

                    case "longDescription":
                        manifestSettings.LongDescription = adminSetting.Value;
                        break;

                    case "apiDomain":
                        manifestSettings.ApiDomain = adminSetting.Value;
                        break;

                    default:
                        break;
                }
            }
            return manifestSettings;
        }

        /// <summary>
        /// This function updates the RuntimeInfoDto object with the current settings from the database
        /// </summary>
        /// <returns></returns>
        public async Task UpdateRuntimeInfo()
        {
            // Set SBCLookup Settings
            IEnumerable<AdminSettingsDB> sbcLookupSettings = await GetValues(UnifiedContactStaticStrings.SETTINGS_SBC_LOOKUP_CATEGORY_ID);
            if (sbcLookupSettings != null)
            {
                _runtimeInfo.SBCLookup = sbcLookupSettings.ConvertToRuntimeInfoSBCLookup();
            }

            // Set EnabledSources
            IEnumerable<AdminSettingsDB> enabledSources = await GetValues("enabledSources");
            if (_runtimeInfo.EnabledSources == null)
            {
                _runtimeInfo.EnabledSources = new HashSet<string>();
            }
            else
            {
                _runtimeInfo.EnabledSources.Clear();
            }
            foreach (AdminSettingsDB enabledSource in enabledSources)
            {
                if (enabledSource.Value == "1")
                {
                    _runtimeInfo.EnabledSources.Add(enabledSource.Key);
                }
            }

            //Set EntraIdFilter
            string? entraIdFilter = await GetValue(UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_ID, UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_CATEGORY);
            if (entraIdFilter != null)
            {
                List<RuntimeInfoEntraIdFilter> runtimeInfoEntraIdFilter = RuntimeInfoEntraIdFilter.FromJsonString(entraIdFilter);
                if (runtimeInfoEntraIdFilter != null)
                {
                    _runtimeInfo.EntraIdFilter = runtimeInfoEntraIdFilter;
                }
            }
        }

        public async Task SetManifestSettings(ManifestSettingsDto settings)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertOrUpdate(new AdminSettingsDB("displayName", "teamsManifest", settings.DisplayName));
            }
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertOrUpdate(new AdminSettingsDB("shortDescription", "teamsManifest", settings.ShortDescription));
            }
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertOrUpdate(new AdminSettingsDB("longDescription", "teamsManifest", settings.LongDescription));
            }
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertOrUpdate(new AdminSettingsDB("apiDomain", "teamsManifest", settings.ApiDomain));
            }
        }
    }
}