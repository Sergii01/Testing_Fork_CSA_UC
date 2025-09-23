namespace UnifiedContacts.Models.Dto
{
    public class UpdateStatusDto
    {
        public bool IsUpdatePending { get; set; } = false;
        public bool RestartRequired { get; set; } = false;
        public DateTime? UpdateStartTimestamp { get; set; } = null;
    }
}
