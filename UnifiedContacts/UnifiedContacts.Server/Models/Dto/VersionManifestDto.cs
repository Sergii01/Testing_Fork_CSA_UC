namespace UnifiedContacts.Models.Dto
{
    public class VersionManifestDtoChannel
    {
        public bool Default { get; set; } = false;
        public string? Name { get; set; }
        public string? LatestVersion { get; set; }
        public string? LatestVersionRef { get; set; }
    }
    public class VersionManifestDto
    {
        public List<VersionManifestDtoChannel>? Channels { get; set; }
    }
}
