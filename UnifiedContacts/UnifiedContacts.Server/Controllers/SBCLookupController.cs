using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UnifiedContacts.Engines;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Server.Models.Payloads.ReverseNumberLookup;

namespace UnifiedContacts.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SBCLookupController : ControllerBase
    {
        private readonly RuntimeInfoDto _runtimeInfo;
        private readonly SBCLookupEngine _sbcLookupEngine;
        public SBCLookupController(RuntimeInfoDto runtimeInfo, SBCLookupEngine sbcLookupEngine)
        {
            _runtimeInfo = runtimeInfo;
            _sbcLookupEngine = sbcLookupEngine;
        }

        private bool IsAuthenticated(HttpRequest request)
        {
            // IP validation
            if (_runtimeInfo.SBCLookup.IsIpAuthenticationEnabled)
            {
                IPAddress? requestingIp = request.HttpContext.Connection.RemoteIpAddress;
                if (requestingIp == null || _runtimeInfo.SBCLookup.AllowedIpAddresses.IsNullOrEmpty())
                {
                    return false;
                }

                bool isAllowed = false;
                foreach (string allowedIpString in _runtimeInfo.SBCLookup.AllowedIpAddresses)
                {
                    if (IPAddress.TryParse(allowedIpString, out IPAddress? allowedIp))
                    {
                        if (requestingIp.Equals(allowedIp))
                        {
                            isAllowed = true;
                            break;
                        }
                    }
                }

                if (!isAllowed)
                {
                    return false;
                }
            }

            // Auth Header validation
            if (_runtimeInfo.SBCLookup.HashedAuthenticationCredential == null)
            {
                return false;
            }

            Microsoft.Extensions.Primitives.StringValues authorizationHeaderValue = request.Headers["Authorization"];
            byte[] credentialByteArray = Convert.FromBase64String(authorizationHeaderValue.ToString().Replace("Basic ", string.Empty));

            string hashedCredential = _sbcLookupEngine.GetCredentialHash(credentialByteArray);

            if (!string.Equals(hashedCredential, _runtimeInfo.SBCLookup.HashedAuthenticationCredential, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        [HttpGet("phone/general/{phoneNumber}")]
        [HttpGet("phone/{phoneNumber}")]
        [AllowAnonymous] // This endpoint is authenticated within the function
        public async Task<ActionResult> LookupPhoneNumber([FromRoute(Name = "phoneNumber")] string phoneNumber)
        {
            if (_runtimeInfo.SBCLookup.EndpointEnabled != true)
            {
                return StatusCode(StatusCodes.Status423Locked);
            }

            if (!IsAuthenticated(Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            DatabaseContactDB? contact = await _sbcLookupEngine.SearchContactByPhoneNumberAsync(phoneNumber);

            if (contact == null || string.IsNullOrEmpty(contact.DisplayName))
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(contact.DisplayName);
        }

        [HttpPost("phone/anynode")]
        [AllowAnonymous] // This endpoint is authenticated within the function
        public async Task<ActionResult<AnyNodeReverseNumberLookupResponse>> AnyNodeLookupPhoneNumber([FromBody] AnyNodeReverseNumberLookupPayload body)
        {
            if (_runtimeInfo.SBCLookup.AnyNodeEndpointEnabled != true)
            {
                return StatusCode(StatusCodes.Status423Locked);
            }

            if (!IsAuthenticated(Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            DatabaseContactDB? contact = null;

            if (!string.IsNullOrWhiteSpace(body.SourceAddress.DialString))
            {
                contact = await _sbcLookupEngine.SearchContactByPhoneNumberAsync(body.SourceAddress.DialString);
            }

            if (contact == null || string.IsNullOrEmpty(contact.DisplayName))
            {
                return Ok(new AnyNodeReverseNumberLookupResponse(true, body.SourceAddress));
            }

            AnyNodeSourceAddress returnAddress = new AnyNodeSourceAddress(body.SourceAddress.DialString, contact.DisplayName);

            return Ok(new AnyNodeReverseNumberLookupResponse(true, returnAddress));
        }
    }
}
