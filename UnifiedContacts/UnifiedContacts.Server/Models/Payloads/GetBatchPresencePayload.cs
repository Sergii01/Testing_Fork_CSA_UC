namespace UnifiedContacts.Models.Payloads
{
    /// <summary>
    /// Payload for /api/contacts/presence
    /// </summary>
    public class GetBatchPresencePayload
    {
        /// <summary>
        /// Unique contactsId. This is the Id that is returned from the search endpoints
        /// </summary>
        public List<string> ContactIds { get; set; } = new List<string>();
    }
}
