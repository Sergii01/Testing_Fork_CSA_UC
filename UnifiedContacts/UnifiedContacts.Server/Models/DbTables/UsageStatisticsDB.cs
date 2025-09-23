using Dapper.Contrib.Extensions;

namespace UnifiedContacts.Models.DbTables
{
    [Table("UnifiedContacts.UsageStatistics")]
    public class UsageStatisticsDB
    {
        [ExplicitKey]
        public Guid UserId { get; set; }
        [ExplicitKey]
        public Guid TenantId { get; set; }
        public DateTime LastAccessDateTime { get; set; }

        public UsageStatisticsDB(Guid userId, Guid tenantId, DateTime lastAccessDateTime)
        {
            UserId = userId;
            TenantId = tenantId;
            LastAccessDateTime = lastAccessDateTime;
        }
    }
}
