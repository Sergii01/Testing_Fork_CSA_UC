using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using UnifiedContacts.Engines.SearchEngines;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Models.Responses;
using UnifiedContacts.Repositories;

namespace UnifiedContacts.Controllers
{
    [Route("v1.3.0/api/favorites")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly FavoritesRepository _favoritesRepository;
        private readonly SearchEngineFactory _searchEngineFactory;
        private readonly RuntimeInfoDto _startupInfoDto;

        public FavoritesController(FavoritesRepository favoritesRepository, SearchEngineFactory searchEngineFactory, RuntimeInfoDto startupInfo)
        {
            _favoritesRepository = favoritesRepository;
            _searchEngineFactory = searchEngineFactory;
            _startupInfoDto = startupInfo;
        }

        [HttpGet("tenants/{tenantId}/contacts")]
        public async Task<ActionResult<UnifiedContactsSearchResponse>> GetFavoritesOfUserForTenant([FromRoute(Name = "tenantId")] Guid tenantId)
        {
            string? userId = User.GetObjectId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User object id is empty!");
            }

            IEnumerable<FavoritesDB> favorites = await _favoritesRepository.GetFavoritesByTenantIdOfUser(tenantId, new Guid(userId));

            if (favorites == null || favorites.Count() == 0)
            {
                return Ok(new UnifiedContactsSearchResponse());
            }
            List<string> favoriteContactIds = favorites.Where(favorite => favorite.ContactId != null).ToList().ConvertAll(favorite => favorite.ContactId!);

            string delegatedToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", string.Empty).Replace(" ", string.Empty);

            UnifiedContactsSearchResponse responseObject = new UnifiedContactsSearchResponse();
            foreach (KeyValuePair<UnifiedContactsSource, Interfaces.ISearchEngine> searchEngineKeyValue in _searchEngineFactory.AllSearchEnginesDict)
            {
                IEnumerable<SearchEngineResultDto> result = await searchEngineKeyValue.Value.GetFavoritesAsync(favoriteContactIds, delegatedToken, User);
                responseObject.SearchResult.AddRange(result.ToList().ConvertAll(searchEngineResult => new UnifiedContactsSearchResponseSearchResult(searchEngineResult, isFavorite: true)));
            }

            return Ok(responseObject);
        }

        [HttpPut("tenants/{tenantId}/contacts/{contactId}")]
        public async Task<ActionResult> AddFavorite([FromRoute(Name = "tenantId")] Guid tenantId, [FromRoute(Name = "contactId")] string contactId)
        {
            string? userObjectId = User.GetObjectId();
            if (string.IsNullOrWhiteSpace(userObjectId))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User object id is empty!");
            }
            if (!_startupInfoDto.DatabaseConfigured)
            {
                return StatusCode(StatusCodes.Status424FailedDependency);
            }

            await _favoritesRepository.AddFavorite(new FavoritesDB { TenantId = tenantId, ContactId = contactId, UserId = new Guid(userObjectId) });
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("tenants/{tenantId}/contacts/{contactId}")]
        public async Task<ActionResult> DeleteFavorite([FromRoute(Name = "tenantId")] Guid tenantId, [FromRoute(Name = "contactId")] string contactId)
        {
            string? userObjectId = User.GetObjectId();
            if (!_startupInfoDto.DatabaseConfigured)
            {
                return StatusCode(StatusCodes.Status424FailedDependency);
            }
            if (string.IsNullOrWhiteSpace(userObjectId))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User object id is empty!");
            }
            await _favoritesRepository.DeleteFavorite(new Models.DbTables.FavoritesDB() { TenantId = tenantId, ContactId = contactId, UserId = new Guid(userObjectId) });
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}