using System.ComponentModel.DataAnnotations;

namespace UnifiedContacts.Models.Payloads.ContactManagement
{
    /// <summary>
    /// Model that contains meta information about a contact that are necessary for CRUD operations
    /// </summary>
    public class CreateCustomContactPayload : CustomContactBase
    {
        /// <summary>
        /// Unique identifier of the contact
        /// </summary>
        /// <example>sap_28648f3b-8a60-4ded-a2df-5f303a74a17a</example>
        [StringLength(256, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string Id { get; set; }
        public CreateCustomContactPayload(string id)
        {
            Id = id;
        }

    }
}
