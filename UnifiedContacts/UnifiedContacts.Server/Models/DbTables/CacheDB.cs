using Dapper.Contrib.Extensions;

namespace UnifiedContacts.Models.DbTables
{
    [Table("UnifiedContacts.Cache")]
    public class CacheDB
    {
        [ExplicitKey]
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
