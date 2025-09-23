namespace UnifiedContacts.Models.Dto
{
    /// <summary>
    /// Unified search result model
    /// </summary>
    public class SearchEngineResultDto : SearchResultBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id">
        /// <param name="source">Source of the SearchEngineResult</param>
        /// Unique Id for every search result.
        /// Template: &lt;UnifiedContactStaticStrings&gt;.&lt;sourceprefix&gt;_&lt;ContactId&gt;
        /// The ContactId must be chosen so the image endpoint can acquire an image on base of this Id
        /// </param>
        public SearchEngineResultDto(string id, UnifiedContactsSource source) : base(id, source)
        {
        }
    }
}
