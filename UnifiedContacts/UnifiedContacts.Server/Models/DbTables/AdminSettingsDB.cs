using Dapper.Contrib.Extensions;

namespace UnifiedContacts.Models.DbTables
{
    [Table("UnifiedContacts.AdminSettings")]
    public class AdminSettingsDB
    {
        [ExplicitKey]
        public string Key { get; set; }
        [ExplicitKey]
        public string Category { get; set; }
        public string? Value { get; set; }

        public AdminSettingsDB(string key, string category, string? value = null)
        {
            Key = key;
            Category = category;
            Value = value;
        }
    }
}
