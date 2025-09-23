using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using UnifiedContacts.Interfaces;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Engines
{
    public class GraphApiEngine : IGraphApiEngine
    {
        private readonly AuthSettings _authSettings;
        private readonly AppSettings _appSettings;

        public GraphApiEngine(AuthSettings authSettings, AppSettings appSettings)
        {
            _authSettings = authSettings;
            _appSettings = appSettings;
        }

        public GraphServiceClient AuthorizeWithOnBehalfOfToken(string delegatedToken)
        {
            TokenCredentialOptions options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };
            string? tenantId = _appSettings.IsFreeSaaS == true ? "common" : _authSettings.TenantId; //IsFreeSaaS == null should be treated as false - hence the explicit '== true' is necessary
            OnBehalfOfCredential onBehalfOfCredential = new OnBehalfOfCredential(tenantId, _authSettings.ClientId, _authSettings.ClientSecret!, delegatedToken);
            string[] scopes = StaticSettings.NECESSARY_PERMISSIONS_PRO.ToArray();
            TokenRequestContext tokenRequestContext = new TokenRequestContext(scopes);
            GraphServiceClient graphClient = new GraphServiceClient(onBehalfOfCredential, scopes);

            return new GraphServiceClient(onBehalfOfCredential, scopes);
        }

        public GraphServiceClient AuthorizeWithOnBehalfOfTokenAdminPage(string delegatedToken)
        {
            string[] scopes = new[] { ".default" };
            string? tenantId = _appSettings.IsFreeSaaS == true ? "common" : _authSettings.AdminPageTenantId; //IsFreeSaaS == null should be treated as false - hence the explicit '== true' is necessary
            OnBehalfOfCredential onBehalfOfCredential = new OnBehalfOfCredential(tenantId, _authSettings.AdminPageClientId, _authSettings.AdminPageClientSecret!, delegatedToken);
            TokenRequestContext tokenRequestContext = new TokenRequestContext(scopes);
            return new GraphServiceClient(onBehalfOfCredential, scopes);
        }

        public GraphServiceClient AuthorizeWithApplicationPermissions()
        {
            string[] scopes = new[] { ".default" };
            ClientSecretCredential clientCredential = new ClientSecretCredential(_authSettings.TenantId, _authSettings.ClientId, _authSettings.ClientSecret);
            return new GraphServiceClient(clientCredential, scopes);
        }

        public async Task<AuthenticationResult> GetAuthenticationWithOnBehalfOfToken(string delegatedToken)
        {
            string[] scopes = new[] { ".default" };
            string? tenantId = _appSettings.IsFreeSaaS == true ? "common" : _authSettings.TenantId; //IsFreeSaaS == null should be treated as false - hence the explicit '== true' is necessary
            IConfidentialClientApplication cca = ConfidentialClientApplicationBuilder.Create(_authSettings.ClientId).WithTenantId(tenantId).WithClientSecret(_authSettings.ClientSecret).Build();

            // Use Microsoft.Identity.Client to retrieve token
            UserAssertion assertion = new UserAssertion(delegatedToken);
            return await cca.AcquireTokenOnBehalfOf(scopes, assertion).ExecuteAsync();
        }

        public async Task<bool> CheckAdminGrant(string delegatedToken, IEnumerable<string> scopes)
        {
            try
            {
                string? tenantId = _appSettings.IsFreeSaaS == true ? "common" : _authSettings.TenantId; //IsFreeSaaS == null should be treated as false - hence the explicit '== true' is necessary
                IConfidentialClientApplication cca = ConfidentialClientApplicationBuilder.Create(_authSettings.ClientId).WithTenantId(tenantId).WithClientSecret(_authSettings.ClientSecret).Build();
                UserAssertion assertion = new Microsoft.Identity.Client.UserAssertion(delegatedToken);
                await cca.AcquireTokenOnBehalfOf(scopes, assertion).ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                if (ex.Message.Contains("AADSTS65001:"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}