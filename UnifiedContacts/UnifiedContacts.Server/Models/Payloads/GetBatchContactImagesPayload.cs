namespace UnifiedContacts.Models.Payloads
{
    /// <summary>
    /// Payload for /api/contacts/photos
    /// </summary>
    public class GetBatchContactImagesPayload
    {
        /// <summary>
        /// Unique contactsId. This is the Id that is returned from the search endpoints
        /// </summary>
        public List<string> ContactIds { get; set; } = new List<string>();
    }
}
