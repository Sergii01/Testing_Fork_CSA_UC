using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using UnifiedContacts.Interfaces;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Models.Responses;
using UnifiedContacts.Repositories;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Controllers
{
    [Route("v1.3.0/api/general")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly AuthSettings _authSettings;
        private readonly IGraphApiEngine _graphApiEngine;
        private readonly UsageRepository _licensingRepository;
        private readonly RuntimeInfoDto _startupInfoDto;
        private readonly AppSettings _appSettings;

        public GeneralController(AuthSettings authSettings, AppSettings appSettings, IGraphApiEngine graphApiEngine, UsageRepository licensingRepository, RuntimeInfoDto startupInfo)
        {
            _authSettings = authSettings;
            _graphApiEngine = graphApiEngine;
            _licensingRepository = licensingRepository;
            _startupInfoDto = startupInfo;
            _appSettings = appSettings;
        }

        /// <summary>
        /// This endpoint is used to check the connection authentication. It also returns information about which permissions are admin granted in the tenant and which permissions may missing, which may cause 403 on some search endpoints
        /// </summary>
        /// <response code="200">Connection info including granted permission info</response>
        [Authorize]
        [HttpGet("connection")]
        public async Task<ActionResult<CheckConnectionResponse>> CheckConnection()
        {
            Task usageCountTask = _licensingRepository.RegisterUsage(User);

            string delegatedToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", string.Empty).Replace(" ", string.Empty);
            CheckConnectionResponse returnObject = new CheckConnectionResponse();

            //Check Admin grant
            if (await _graphApiEngine.CheckAdminGrant(delegatedToken, new[] { "https://graph.microsoft.com/User.Read" })) //This is the bear minimum permissions necessary
            {
                returnObject.IsAdminGranted = true;
            }
            ;

            Microsoft.Identity.Client.AuthenticationResult accessTokenInfo = await _graphApiEngine.GetAuthenticationWithOnBehalfOfToken(delegatedToken);
            List<string> scopes = accessTokenInfo.Scopes.Where(scope => scope.Contains("00000003-0000-0000-c000-000000000000/") && scope != "00000003-0000-0000-c000-000000000000/.default").ToList().ConvertAll(scope => scope.Replace("00000003-0000-0000-c000-000000000000/", string.Empty));

            returnObject.GrantedPermissions = scopes;
            returnObject.NotGrantedPermissions = StaticSettings.NECESSARY_PERMISSIONS_PRO.Where(scope => !scopes.Contains(scope)).ToList();
            returnObject.ClientId = _authSettings.ClientId;

            try
            {
                await usageCountTask;
            }
            catch (Exception)
            {
                /* Fall through*/
            }
            return Ok(returnObject);
        }

        /// <summary>
        /// This endpoint returns the current version of the UnifiedContacts environment
        /// </summary>
        /// <response code="200">Version of the current UnfiedContacts environment</response>
        [HttpGet("version")]
        [AllowAnonymous]
        public ActionResult<GetVersionResponse> GetVersion()
        {
            return Ok(new GetVersionResponse(StaticSettings.VERSION));
        }

        [HttpGet("adminpage/appreginfo")]
        [AllowAnonymous]
        public ActionResult<GetAdminAppRegInfoResponse> GetAdminAppRegInfo()
        {
            return Ok(new GetAdminAppRegInfoResponse()
            {
                ClientId = _authSettings.AdminPageClientId,
                TenantId = _authSettings.AdminPageTenantId
            });
        }

        /// <summary>
        /// This endpoint returns wether the current UC is a standalone free oder free SAAS
        /// </summary>
        /// <response code="200">Version of the current UnfiedContacts environment</response>
        [AllowAnonymous]
        [HttpGet("backendconfig")]
        public ActionResult<GetBackendConfigResponse> GetBackendConfig()
        {
            return Ok(new GetBackendConfigResponse(_startupInfoDto.DatabaseConfigured));
        }
    }
}