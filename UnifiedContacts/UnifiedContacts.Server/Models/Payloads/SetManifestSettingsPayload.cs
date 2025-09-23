namespace UnifiedContacts.Models.Payloads
{
    public class SetManifestSettingsPayload
    {
        public string? DisplayName { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? ApiDomain { get; set; }
    }
}
