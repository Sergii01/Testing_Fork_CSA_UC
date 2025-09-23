using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Communications.GetPresencesByUserId;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Net.Http.Headers;
using UnifiedContacts.Interfaces;
using UnifiedContacts.Models.Payloads;
using UnifiedContacts.Models.Responses;

namespace UnifiedContacts.Controllers
{
    [Authorize(Policy = "TeamsApp")]
    [Route("v1.3.0/api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IGraphApiEngine _graphApiEngine;

        public ContactsController(IGraphApiEngine graphApiEngine)
        {
            _graphApiEngine = graphApiEngine;
        }

        private class IdInfo
        {
            public string ContactId { get; set; }
            public string Id { get; set; }
            public UnifiedContactsSource Source { get; set; }

            public IdInfo(string contactId, string id, UnifiedContactsSource source)
            {
                ContactId = contactId;
                Id = id;
                Source = source;
            }
        }

        /// <summary>
        /// This endpoint return images for up to 20 contacts.
        /// </summary>
        /// <param name="body">Request body. See Schema for details. </param>
        /// <response code="200">Photos of contacts</response>
        /// <response code="400">An invalid body was provided</response>
        /// <response code="400">More than 20 ids have been provided</response>
        /// <response code="400">At least one contactId is of invalid format</response>
        [HttpPost("photos")]
        public async Task<ActionResult<List<UnifiedContactsImageResponse>>> GetBatchContactImages([FromBody] GetBatchContactImagesPayload body)
        {
            if (body == null || body.ContactIds.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Please provide a valid body");
            }

            if (body.ContactIds.Count > 20)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "A maximum of 20 ids is supported");
            }

            Dictionary<string, IdInfo> idInfoDict = new Dictionary<string, IdInfo>();

            foreach (string contactId in body.ContactIds)
            {
                if (!string.IsNullOrEmpty(contactId))
                {
                    string[] splittedContactId = contactId.Split("_", 2);
                    if (splittedContactId.Length != 2)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "At least one contactId is of invalid format");
                    }

                    if (!idInfoDict.ContainsKey(contactId) && UnifiedContactsEnumConverter.TryConvertIdStringPrefixToUnifiedContactsSource(splittedContactId[0], out UnifiedContactsSource? source))
                    {
                        idInfoDict.Add(contactId, new IdInfo(contactId, splittedContactId[1], source!.Value));
                    }
                }
            }

            string delegatedToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", string.Empty).Replace(" ", string.Empty);
            GraphServiceClient graphClient = _graphApiEngine.AuthorizeWithOnBehalfOfToken(delegatedToken);

            BatchRequestContentCollection graphBatchRequestContent = new BatchRequestContentCollection(graphClient);
            RequestInformation? azureADUserBatchRequests = null;
            Dictionary<string, string> stepIds = new Dictionary<string, string>();
            foreach (KeyValuePair<string, IdInfo> idSourceKeyValue in idInfoDict)
            {
                switch (idSourceKeyValue.Value.Source)
                {
                    case UnifiedContactsSource.AZURE_AD:
                        azureADUserBatchRequests = graphClient.Users[idSourceKeyValue.Value.Id].Photos["96x96"].Content.ToGetRequestInformation();
                        stepIds.Add(idSourceKeyValue.Value.ContactId, await graphBatchRequestContent.AddBatchRequestStepAsync(azureADUserBatchRequests));

                        break;

                    case UnifiedContactsSource.USER_CONTACT:
                        azureADUserBatchRequests = graphClient.Me.Contacts[idSourceKeyValue.Value.Id].Photo.Content.ToGetRequestInformation();
                        stepIds.Add(idSourceKeyValue.Value.ContactId, await graphBatchRequestContent.AddBatchRequestStepAsync(azureADUserBatchRequests));
                        break;

                    default:
                        break;
                }
            }

            List<UnifiedContactsImageResponse> responseList = new List<UnifiedContactsImageResponse>();

            if (graphBatchRequestContent.BatchRequestSteps.Count() != 0)
            {
                BatchResponseContentCollection response = await graphClient.Batch.PostAsync(graphBatchRequestContent);

                foreach (string contactId in body.ContactIds)
                {
                    UnifiedContactsImageResponse resultElement = new UnifiedContactsImageResponse(contactId);
                    try
                    {
                        if (idInfoDict.TryGetValue(contactId, out IdInfo? idInfo))
                        {
                            HttpResponseMessage imageResponse = new HttpResponseMessage();
                            imageResponse = await response.GetResponseByIdAsync(stepIds[contactId]);
                            if (imageResponse != null && imageResponse.IsSuccessStatusCode)
                            {
                                if (idInfo.Source == UnifiedContactsSource.AZURE_AD || idInfo.Source == UnifiedContactsSource.USER_CONTACT)
                                {
                                    resultElement.ImageData = await imageResponse.Content.ReadAsStringAsync();
                                    resultElement.ImageType = UnifiedContactsImageType.BASE64;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Exception exception = ex;
                        /* Fall through */
                    }
                    responseList.Add(resultElement);
                }
            }
            else
            {
                foreach (string contactId in body.ContactIds)
                {
                    responseList.Add(new UnifiedContactsImageResponse(contactId));
                }
            }

            return Ok(responseList);
        }

        /// <summary>
        /// This endpoint returns the presence status for up to 650 contacts
        /// </summary>
        /// <param name="body">Request body. See Schema for details.</param>
        /// <response code="200">Presence of contacts</response>
        /// <response code="400">An invalid body was provided</response>
        /// <response code="400">More than 650 ids have been provided</response>
        /// <response code="400">At least one contactId is of invalid format</response>
        [HttpPost("presence")]
        public async Task<ActionResult<List<UnifiedContactsPresenceResponse>>> GetBatchPresence([FromBody] GetBatchPresencePayload body)
        {
            if (body == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Please provide a valid body");
            }
            if (body.ContactIds.Count > 650) // The limit of 650 is predetermined by Graph API endpoint
            {
                return StatusCode(StatusCodes.Status400BadRequest, "A maximum of 650 ids is supported");
            }

            Dictionary<string, string> idDict = new Dictionary<string, string>();
            List<string> presenceIdRequest = new List<string>();

            foreach (string contactId in body.ContactIds)
            {
                if (!string.IsNullOrEmpty(contactId))
                {
                    string[] splittedContactId = contactId.Split("_", 2);
                    if (splittedContactId.Length != 2)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "At least one contactId is of invalid format");
                    }

                    idDict.Add(contactId, splittedContactId[1]);

                    if (UnifiedContactsEnumConverter.TryConvertIdStringPrefixToUnifiedContactsSource(splittedContactId[0], out UnifiedContactsSource? source))
                    {
                        // Nested if for better readability
                        if (source == UnifiedContactsSource.AZURE_AD)
                        {
                            presenceIdRequest.Add(splittedContactId[1]);
                        }
                    }
                }
            }

            string delegatedToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", string.Empty).Replace(" ", string.Empty);
            GraphServiceClient graphClient = _graphApiEngine.AuthorizeWithOnBehalfOfToken(delegatedToken);
            Dictionary<string, Presence> presenceResponseDict = new Dictionary<string, Presence>();

            if (!presenceIdRequest.IsNullOrEmpty())
            {
                GetPresencesByUserIdPostResponse? presenceResponse;
                do
                {
                    presenceResponse = await graphClient.Communications.GetPresencesByUserId.PostAsGetPresencesByUserIdPostResponseAsync(new GetPresencesByUserIdPostRequestBody() { Ids = presenceIdRequest.Distinct().ToList() });
                    if (presenceResponse != null && presenceResponse.Value != null)
                    {
                        foreach (Presence presence in presenceResponse.Value)
                        {
                            if (presence?.Id != null)
                            {
                                presenceResponseDict.Add(presence.Id, presence);
                            }
                        }
                    }
                } while (presenceResponse?.OdataNextLink != null);
            }

            List<UnifiedContactsPresenceResponse> returnArray = new List<UnifiedContactsPresenceResponse>();
            foreach (KeyValuePair<string, string> contactIdIdKeyValuePair in idDict)
            {
                if (presenceResponseDict.TryGetValue(contactIdIdKeyValuePair.Value, out Presence? presence))
                {
                    returnArray.Add(new UnifiedContactsPresenceResponse(contactIdIdKeyValuePair.Key) { Activity = presence.Activity, Availability = presence.Availability });
                }
                else
                {
                    returnArray.Add(new UnifiedContactsPresenceResponse(contactIdIdKeyValuePair.Key));
                }
            }

            return Ok(returnArray);
        }
    }
}