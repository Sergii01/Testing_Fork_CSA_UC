using Dapper.Contrib.Extensions;

namespace UnifiedContacts.Models.DbTables
{
    [Table("UnifiedContacts.Favorites")]
    public class FavoritesDB
    {
        [ExplicitKey]
        public Guid? UserId { get; set; }
        [ExplicitKey]
        public string? ContactId { get; set; }
        [ExplicitKey]
        public Guid? TenantId { get; set; }
    }
}
