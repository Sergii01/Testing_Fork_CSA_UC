namespace UnifiedContacts.Models.Responses
{
    public class GetManifestSettingsResponse
    {
        public string? DisplayName { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? ClientId { get; set; }
        public string? ApiDomain { get; set; }
    }
}
