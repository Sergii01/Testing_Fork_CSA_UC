using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnifiedContacts.Engines;
using UnifiedContacts.Models.Exceptions;
using UnifiedContacts.Models.Payloads.ContactManagement;
using UnifiedContacts.Models.Responses.ContactManagement;

namespace UnifiedContacts.Controllers.ConsumerEndpoints
{
    // This Controller can be used directly by the customer, therefore XML documentation here needs to be very thorough!
    [Route("api/contacts")]
    [Route("api/v1/contacts")]
    [Authorize(Policy = "CustomerApi", Roles = "Contacts.Database.ReadWrite.All")]
    [ApiController]
    public class ContactManagementController : ControllerBase
    {
        private readonly ContactManagementEngine _contactManagementEngine;
        public ContactManagementController(ContactManagementEngine contactManagementEngine)
        {
            _contactManagementEngine = contactManagementEngine;
        }

        /// <summary>
        /// Gets a contact with all meta data that is stored in the database
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <response code="200">Contact successfully found and returned</response>
        /// <response code="404">Contact with id not found</response>
        [HttpGet("{contactId}")]
        public async Task<ActionResult<CustomContactResponse>> GetContact([FromRoute] string contactId)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await _contactManagementEngine.GetContactById(contactId));
            }
            catch (DatabaseContactNotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Contact with id not found");
            }
        }

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="body">Request body</param>
        /// <response code="201">Contact was successfully created</response>
        /// <response code="409">Contact with id already exists</response>
        [HttpPost]
        public async Task<ActionResult<CustomContactResponse>> CreateContact([FromBody] CreateCustomContactPayload body)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created, await _contactManagementEngine.InsertContact(body));
            }
            catch (DatabaseContactConflict)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Contact with id already exists");
            }
        }

        /// <summary>
        /// Overwrites the meta information of an existing contact
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="body">Request body</param>
        /// <response code="200">Contact was successfully updated - full contact info is returned</response>
        /// <response code="404">Contact with id not found</response>
        [HttpPut("{contactId}")]
        public async Task<ActionResult<CustomContactResponse>> UpdateContact([FromRoute] string contactId, [FromBody] UpdateCustomContactPayload body)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await _contactManagementEngine.UpdateContact(contactId, body, overwriteExistingContact: true));
            }
            catch (DatabaseContactNotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Contact with id not found");
            }
        }

        /// <summary>
        /// Updates provided meta information of an existing contact
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="body">Request body</param>
        /// <response code="200">Contact was successfully updated - full contact info is returned</response>
        /// <response code="404">Contact with id not found</response>
        [HttpPatch("{contactId}")]
        public async Task<ActionResult<CustomContactResponse>> PatchUpdateContact([FromRoute] string contactId, [FromBody] UpdateCustomContactPayload body)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await _contactManagementEngine.UpdateContact(contactId, body, overwriteExistingContact: false));
            }
            catch (DatabaseContactNotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Contact with id not found");
            }
        }

        /// <summary>
        /// Deletes an existing contact
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <response code="204">Contact was successfully deleted</response>
        /// <response code="404">Contact with id not found</response>
        [HttpDelete("{contactId}")]
        public async Task<ActionResult> DeleteContact([FromRoute] string contactId)
        {

            try
            {
                await _contactManagementEngine.DeleteContactById(contactId);
            }
            catch (DatabaseContactNotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Contact with id not found");
            }

            return StatusCode(StatusCodes.Status204NoContent);

        }
    }
}
