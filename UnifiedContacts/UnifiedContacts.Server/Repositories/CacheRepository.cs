using Dapper;
using Microsoft.Data.SqlClient;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Repositories
{
    public class CacheRepository : RepositoryBase
    {
        private readonly AuthSettings _authSettings;
        public CacheRepository(AuthSettings authSettings, RuntimeInfoDto startupInfo) : base(startupInfo)
        {
            _authSettings = authSettings;
        }

        /// <summary>
        /// Gets the cache value of a specific settings key.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cache value</returns>
        public async Task<string?> GetValue(string key)
        {
            VerifyDatabaseConfiguration();

            CacheDB? cache;
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                cache = (await connection.QueryAsync<CacheDB>("SELECT * FROM [UnifiedContacts].[Cache] WHERE [key] = @key", new { key = key })).FirstOrDefault();
            }
            if (cache == null)
            {
                return null;
            }
            return cache.Value;
        }

        /// <summary>
        /// Sets the cache value of a specific cache key. This operation is executed as insert or update
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Cache value</param>
        /// <returns></returns>
        public async Task SetValue(string key, string? value)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertOrUpdate(new CacheDB() { Key = key, Value = value });
            }
        }
    }
}
