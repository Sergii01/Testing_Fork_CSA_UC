namespace UnifiedContacts.Models.Responses
{
    public class GetIsUpdateInProgressResponse
    {
        public bool IsUpdateInProgress { get; set; }
        public bool RestartRequired { get; set; }
    }
}
