using Dapper;
using Microsoft.Data.SqlClient;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Repositories
{
    public class TelemetryRepository : RepositoryBase
    {
        private readonly AuthSettings _authSettings;

        public TelemetryRepository(AuthSettings authSettings, RuntimeInfoDto startupInfo) : base(startupInfo)
        {
            _authSettings = authSettings;
        }

        public async Task<int> GetMaxFavoritesOfUser()
        {
            VerifyDatabaseConfiguration();
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>("SELECT TOP(1) COUNT(contactId) as favoriteCount FROM [UnifiedContacts].[Favorites] GROUP BY userId ORDER BY favoriteCount DESC");
            }
        }

        public async Task<int> GetAverageFavoriteCountOfUsers()
        {
            VerifyDatabaseConfiguration();
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>("SELECT AVG(CAST(favoriteCount AS DECIMAL(19, 5))) FROM (SELECT COUNT(contactId) as favoriteCount FROM [UnifiedContacts].[Favorites] GROUP BY userId) as t");
            }
        }

        public async Task<int> GetLast30DaysActiveUserCount()
        {
            VerifyDatabaseConfiguration();
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>("SELECT COUNT(userId) FROM [UnifiedContacts].[UsageStatistics] WHERE DATEDIFF(day, lastAccessDateTime, GETUTCDATE()) < 30");
            }
        }

        /// <summary>
        /// Increments the count of the request counter and the clientType request counter depending on the client Type
        /// NOTE: This is a fire and forget call
        /// </summary>
        /// <param name="clientType">Client type where the search request originates from</param>
        public async Task RegisterRequestAsync(TeamsClientType clientType)
        {
            VerifyDatabaseConfiguration();
            var sqlParams = new
            {
                resultCountAad = 0,
                resultCountUserContact = 0,
                resultCountOrgContact = 0,
                resultCountSharepoint = 0,
                resultCountDatabase = 0,
                requestCount = 1,
                requestCountAndroid = clientType == TeamsClientType.ANDROID ? 1 : 0,
                requestCountIos = clientType == TeamsClientType.IOS ? 1 : 0,
                requestCountWeb = clientType == TeamsClientType.WEB ? 1 : 0,
                requestCountDesktop = clientType == TeamsClientType.DESKTOP ? 1 : 0,
                requestCountUnknown = clientType == TeamsClientType.UNKNOWN ? 1 : 0
            };

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.ExecuteAsync("EXECUTE [UnifiedContacts].[UpdateTelemetryTenantHistory] @resultCountAad, @resultCountUserContact, @resultCountOrgContact, @resultCountSharepoint, @resultCountDatabase, @requestCount, @requestCountAndroid, @requestCountIos, @requestCountWeb, @requestCountDesktop, @requestCountUnknown", sqlParams);
            }
        }

        /// <summary>
        /// Increments the count of the result counter depending on the source
        /// NOTE: This is a fire and forget call
        /// </summary>
        /// <param name="source">Search Source where the responses originate from</param>
        /// <param name="count">Number of responses found</param>
        public async Task RegisterResultsAsync(UnifiedContactsSource source, int count)
        {
            VerifyDatabaseConfiguration();
            var sqlParams = new
            {
                resultCountAad = source == UnifiedContactsSource.AZURE_AD ? count : 0,
                resultCountUserContact = source == UnifiedContactsSource.USER_CONTACT ? count : 0,
                resultCountOrgContact = source == UnifiedContactsSource.ORG_CONTACT ? count : 0,
                resultCountSharepoint = source == UnifiedContactsSource.SHAREPOINT ? count : 0,
                resultCountDatabase = source == UnifiedContactsSource.DATABASE ? count : 0,
                requestCount = 0,
                requestCountAndroid = 0,
                requestCountIos = 0,
                requestCountWeb = 0,
                requestCountDesktop = 0,
                requestCountUnknown = 0
            };

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.ExecuteAsync("EXECUTE [UnifiedContacts].[UpdateTelemetryTenantHistory] @resultCountAad, @resultCountUserContact, @resultCountOrgContact, @resultCountSharepoint, @resultCountDatabase, @requestCount, @requestCountAndroid, @requestCountIos, @requestCountWeb, @requestCountDesktop, @requestCountUnknown", sqlParams);
            }
        }
    }
}
