using Dapper.Contrib.Extensions;

namespace UnifiedContacts.Models.DbTables
{
    [Table("UnifiedContacts.TelemetryTenantHistory")]
    public class TelemetryTenantHistoryDB
    {
        [ExplicitKey]
        public DateTime Date { get; set; }
        public int ResultCountAad { get; set; }
        public int ResultCountUserContact { get; set; }
        public int ResultCountOrgContact { get; set; }
        public int ResultCountSharePoint { get; set; }
        public int ResultCountDatabase { get; set; }
        public int RequestCount { get; set; }
        public int ActiveUserCountLast30Days { get; set; }
        public int RequestCountAndroid { get; set; }
        public int RequestCountIos { get; set; }
        public int RequestCountWeb { get; set; }
        public int RequestCountDesktop { get; set; }
        public int RequestCountUnknown { get; set; }

        public TelemetryTenantHistoryDB(DateTime date, int resultCountAad, int resultCountUserContact, int resultCountOrgContact, int resultCountSharePoint, int resultCountDatabase, int requestCount, int activeUserCountLast30Days, int requestCountAndroid, int requestCountIos, int requestCountWeb, int requestCountDesktop, int requestCountUnknown)
        {
            Date = date;
            ResultCountAad = resultCountAad;
            ResultCountUserContact = resultCountUserContact;
            ResultCountOrgContact = resultCountOrgContact;
            ResultCountSharePoint = resultCountSharePoint;
            ResultCountDatabase = resultCountDatabase;
            RequestCount = requestCount;
            ActiveUserCountLast30Days = activeUserCountLast30Days;
            RequestCountAndroid = requestCountAndroid;
            RequestCountIos = requestCountIos;
            RequestCountWeb = requestCountWeb;
            RequestCountDesktop = requestCountDesktop;
            RequestCountUnknown = requestCountUnknown;
        }
    }
}
