namespace UnifiedContacts.Models.Responses
{
    public class UnifiedContactsPresenceResponse
    {
        /// <summary>
        /// Unique id of contact (same as from search endpoint)
        /// Template: &lt;UnifiedContactStaticStrings&gt;.&lt;sourceprefix&gt;_&lt;ContactId&gt;
        /// The ContactId must be chosen so the image endpoint can acquire an image on base of this Id
        /// </summary>
        public string ContactId { get; set; }
        /// <summary>
        /// Availability of user. Same as returned from GraphAPI - https://docs.microsoft.com/en-us/graph/api/resources/presence?view=graph-rest-1.0 / https://docs.microsoft.com/en-us/microsoftteams/presence-admins
        /// Can be null for sources that do not support presence (e.g. Org/UserContacts)
        /// </summary>
        public string? Availability { get; set; }
        /// <summary>
        /// Activity of user. Same as returned from GraphAPI - https://docs.microsoft.com/en-us/graph/api/resources/presence?view=graph-rest-1.0 / https://docs.microsoft.com/en-us/microsoftteams/presence-admins
        /// Can be null for sources that do not support presence (e.g. Org/UserContacts)
        /// </summary>
        public string? Activity { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="contactId">Unique id of contact (same as from search endpoint)</param>
        public UnifiedContactsPresenceResponse(string contactId)
        {
            ContactId = contactId;
        }
    }
}
