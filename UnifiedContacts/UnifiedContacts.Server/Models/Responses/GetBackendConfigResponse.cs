namespace UnifiedContacts.Models.Responses
{
    public class GetBackendConfigResponse
    {
        public bool IsDatabaseConfigured { get; set; }

        public GetBackendConfigResponse(bool isDatabaseConfigured)
        {
            IsDatabaseConfigured = isDatabaseConfigured;
        }
    }
}
