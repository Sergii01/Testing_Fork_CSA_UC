using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace UnifiedContacts.Interfaces
{
    public interface IGraphApiEngine
    {
        GraphServiceClient AuthorizeWithOnBehalfOfToken(string delegatedToken);

        GraphServiceClient AuthorizeWithOnBehalfOfTokenAdminPage(string delegatedToken);

        GraphServiceClient AuthorizeWithApplicationPermissions();

        Task<AuthenticationResult> GetAuthenticationWithOnBehalfOfToken(string delegatedToken);

        Task<bool> CheckAdminGrant(string delegatedToken, IEnumerable<string> scopes);
    }
}