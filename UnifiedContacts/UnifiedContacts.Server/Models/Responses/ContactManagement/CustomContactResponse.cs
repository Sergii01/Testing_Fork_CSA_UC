using UnifiedContacts.Models.DbTables;

namespace UnifiedContacts.Models.Responses.ContactManagement
{
    /// <summary>
    /// Model that contains all information about a contact
    /// </summary>
    public class CustomContactResponse : CustomContactBase
    {
        public string Id { get; set; }
        public CustomContactResponse(string id)
        {
            Id = id;
        }

        public CustomContactResponse(DatabaseContactDB databaseContact) : base(databaseContact)
        {
            Id = databaseContact.Id;
        }
    }
}
