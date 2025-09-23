using UnifiedContacts.Models.Dto;

namespace UnifiedContacts.Models.Responses
{
    /// <summary>
    /// Unified search result model
    /// </summary>
    public class UnifiedContactsSearchResponseSearchResult : SearchResultBase
    {
        public bool IsFavorite { get; set; }

        public UnifiedContactsSearchResponseSearchResult(SearchEngineResultDto searchResults, bool isFavorite = false) : base(searchResults.Id, searchResults.Source)
        {
            DisplayName = searchResults.DisplayName;
            JobTitle = searchResults.JobTitle;
            Department = searchResults.Department;
            CompanyName = searchResults.CompanyName;
            MailAddresses = searchResults.MailAddresses;
            ImAddresses = searchResults.ImAddresses;
            PhoneNumbers = searchResults.PhoneNumbers;
            Addresses = searchResults.Addresses;
            IsFavorite = isFavorite;
            SubSource = searchResults.SubSource;
        }
    }
    /// <summary>
    /// Response Model of all UnifiedContacts endpoints. This is used incase we will provide metadata or features that need metadata (like pagination) in the future.
    /// </summary>
    public class UnifiedContactsSearchResponse
    {
        /// <summary>
        /// Search results
        /// </summary>
        public List<UnifiedContactsSearchResponseSearchResult> SearchResult { get; set; } = new List<UnifiedContactsSearchResponseSearchResult>();

        public UnifiedContactsSearchResponse()
        {

        }

        /// <summary>
        /// Constructor to generate Unified Search result from SearchEngineResultDto
        /// </summary>
        /// <param name="searchResults">Search result of SearchEngine</param>
        public UnifiedContactsSearchResponse(IEnumerable<SearchEngineResultDto> searchResults)
        {
            SearchResult = searchResults.ToList().ConvertAll(searchEngineResult => new UnifiedContactsSearchResponseSearchResult(searchEngineResult));
        }

        public UnifiedContactsSearchResponse(IEnumerable<SearchEngineResultDto> searchResults, IEnumerable<string> favoriteContactIds)
        {
            SearchResult = searchResults.ToList().ConvertAll(searchEngineResult => new UnifiedContactsSearchResponseSearchResult(searchEngineResult));
            foreach (UnifiedContactsSearchResponseSearchResult result in SearchResult)
            {
                result.IsFavorite = favoriteContactIds.Contains(result.Id);
            }
        }
    }

}
