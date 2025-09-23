using System.Security.Claims;
using UnifiedContacts.Models.Dto;

namespace UnifiedContacts.Interfaces
{
    public interface ISearchEngine
    {
        Task<IEnumerable<SearchEngineResultDto>> SearchContactAsync(string searchQuery, string delegatedToken, ClaimsPrincipal userPrincipal);

        Task<IEnumerable<SearchEngineResultDto>> GetFavoritesAsync(IEnumerable<string> contactIds, string delegatedToken, ClaimsPrincipal userPrincipal);
    }
}
