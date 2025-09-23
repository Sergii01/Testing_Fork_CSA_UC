namespace UnifiedContacts.Models.Responses
{
    public enum DependencyStatus
    {
        HEALTHY,
        WARNING,
        ERROR,
        NO_LICENSE_KEY,
        UNKNOWN
    }

    public class GetDependenciesStatusDependency
    {
        public string DisplayName { get; set; }
        public DependencyStatus Status { get; set; }
        public string? StatusDescription { get; set; }
        public GetDependenciesStatusDependency(string displayName, DependencyStatus status, string? statusDescription)
        {
            DisplayName = displayName;
            Status = status;
            StatusDescription = statusDescription;
        }
        public GetDependenciesStatusDependency(string displayName, DependencyStatus status) : this(displayName, status, null)
        {

        }
        public GetDependenciesStatusDependency(string displayName) : this(displayName, DependencyStatus.UNKNOWN)
        {

        }
    }

    public class GetDependenciesStatusResponse
    {
        public List<GetDependenciesStatusDependency> Dependencies { get; set; } = new List<GetDependenciesStatusDependency>();
    }
}
