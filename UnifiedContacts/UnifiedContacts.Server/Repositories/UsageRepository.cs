using Dapper;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Repositories
{
    public class UsageRepository : RepositoryBase
    {
        private readonly AuthSettings _authSettings;
        public UsageRepository(RuntimeInfoDto startupInfo, AuthSettings authSettings) : base(startupInfo)
        {
            _authSettings = authSettings;
        }

        public async Task RegisterUsage(ClaimsPrincipal user, DateTime? lastAccess = null)
        {
            if (!IsDatabaseConfigured())
            {
                return;
            }

            Guid userId;
            Guid tenantId;
            if (Guid.TryParse(user.GetObjectId(), out userId) && Guid.TryParse(user.GetObjectTenantId(), out tenantId))
            {
                lastAccess ??= DateTime.UtcNow;
                using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
                {
                    await connection.InsertOrUpdate(new UsageStatisticsDB(userId, tenantId, lastAccess.Value));
                }
            }
        }

        private class UsageCount
        {
            public int Count { get; set; }
        }
        public async Task<int> GetUsageCount(TimeSpan usageTimespan)
        {
            DateTime useageCountStartDateTime = DateTime.UtcNow.Subtract(usageTimespan);
            string sql = "SELECT COUNT(*) as count FROM [UnifiedContacts].[UsageStatistics] WHERE lastAccessDateTime > @useageCountStartDateTime";


            UsageCount? usageCount;
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                usageCount = (await connection.QueryAsync<UsageCount>(sql, new { useageCountStartDateTime = useageCountStartDateTime })).FirstOrDefault();
            }

            return usageCount?.Count ?? 0;
        }
    }
}
