namespace UnifiedContacts.Models.Responses
{
    public class GetVersionUpdateInfoResponse
    {
        public bool? UpdateAvailable { get; set; }
        public bool? RestartRequired { get; set; }
        public bool? UpdateInProgress { get; set; }
        public string? SelectedChannel { get; set; }
        public string? UpdateVersion { get; set; }
        public string? CurrentVersion { get; set; }
        public List<string>? AvaliableChannels { get; set; }
        public string? AppServiceAzureUrl { get; set; }
    }
}
