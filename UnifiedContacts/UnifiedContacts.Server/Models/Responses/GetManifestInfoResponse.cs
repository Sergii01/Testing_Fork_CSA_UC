namespace UnifiedContacts.Models.Responses
{
    public class GetManifestInfoResponse
    {
        public bool TeamsManifestExists { get; set; } = false;
        public bool TeamsManifestUpdatePossible { get; set; } = false;
        public string? TeamsManifestVersion { get; set; }
        public string? ApiVersion { get; set; }
    }
}
