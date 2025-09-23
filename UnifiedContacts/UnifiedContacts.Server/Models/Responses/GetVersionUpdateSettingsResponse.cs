namespace UnifiedContacts.Models.Responses
{
    public class GetVersionUpdateSettingsResponse
    {
        public string? SelectedReleaseChannel { get; set; }
        public List<string>? AvailableReleaseChannels { get; set; }
    }
}
