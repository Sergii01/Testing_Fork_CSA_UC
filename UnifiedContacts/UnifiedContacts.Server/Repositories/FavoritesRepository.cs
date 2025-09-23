using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Repositories
{
    public class FavoritesRepository : RepositoryBase
    {
        private readonly AuthSettings _authSettings;

        public FavoritesRepository(AuthSettings authSettings, RuntimeInfoDto startupInfo) : base(startupInfo)
        {
            _authSettings = authSettings;
        }

        public async Task<IEnumerable<FavoritesDB>> GetFavoritesByTenantIdOfUser(Guid tenantId, Guid userId)
        {
            VerifyDatabaseConfiguration();
            string sql = $"SELECT * FROM [UnifiedContacts].[Favorites] WHERE tenantId = @tenantId AND userId = @userId";

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.QueryAsync<FavoritesDB>(sql, new { tenantId = tenantId, userId = userId });
            }
        }

        public async Task<IEnumerable<int>> GetFavoritesByTenantIdOfUserCount(Guid tenantId, Guid userId)
        {
            VerifyDatabaseConfiguration();

            string sql = $"SELECT COUNT(*) FROM [UnifiedContacts].[Favorites] WHERE tenantId = @tenantId AND userId = @userId";

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.QueryAsync<int>(sql, new { tenantId = tenantId, userId = userId });
            }
        }

        public async Task AddFavorite(FavoritesDB element)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertOrUpdate(element);
            }
        }

        public async Task DeleteFavorite(FavoritesDB element)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.DeleteAsync(element);
            }
        }
    }
}