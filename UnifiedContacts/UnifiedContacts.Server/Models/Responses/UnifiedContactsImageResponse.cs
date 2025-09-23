namespace UnifiedContacts.Models.Responses
{

    /// <summary>
    /// Image response model that returns a image of the contact
    /// </summary>
    public class UnifiedContactsImageResponse
    {
        /// <summary>
        /// Unique id of contact (same as from search endpoint)
        /// Template: &lt;UnifiedContactStaticStrings&gt;.&lt;sourceprefix&gt;_&lt;ContactId&gt;
        /// The ContactId must be chosen so the image endpoint can acquire an image on base of this Id
        /// </summary>
        public string ContactId { get; set; }
        /// <summary>
        /// Format of image data
        /// </summary>
        public UnifiedContactsImageType ImageType { get; set; } = UnifiedContactsImageType.NONE;
        /// <summary>
        /// Image data encoded in the in imageType specified format
        /// </summary>
        public string? ImageData { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="contactId">Unique id of contact (same as from search endpoint)</param>
        public UnifiedContactsImageResponse(string contactId)
        {
            ContactId = contactId;
        }
    }
}
