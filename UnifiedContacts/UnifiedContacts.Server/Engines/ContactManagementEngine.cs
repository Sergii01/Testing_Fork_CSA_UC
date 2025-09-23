using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Exceptions;
using UnifiedContacts.Models.Payloads.ContactManagement;
using UnifiedContacts.Models.Responses.ContactManagement;
using UnifiedContacts.Repositories;

namespace UnifiedContacts.Engines
{
    public class ContactManagementEngine
    {
        public DatabaseContactsRepository _databaseContactsRepository { get; set; }
        public ContactManagementEngine(DatabaseContactsRepository databaseContactsRepository)
        {
            _databaseContactsRepository = databaseContactsRepository;
        }

        public async Task<CustomContactResponse> GetContactById(string contactId)
        {

            DatabaseContactDB dbEntry = await _databaseContactsRepository.GetDatabaseContactsByIdAsync(contactId);
            if (dbEntry == null)
            {
                throw new DatabaseContactNotFound("Contact was not found in Database");
            }

            return new CustomContactResponse(dbEntry);
        }

        public async Task<CustomContactResponse> InsertContact(CreateCustomContactPayload contact)
        {
            DatabaseContactDB dbEntry = await _databaseContactsRepository.GetDatabaseContactsByIdAsync(contact.Id);
            if (dbEntry != null)
            {
                throw new DatabaseContactConflict(dbEntry, "Contact with Id already exists");
            }
            DatabaseContactDB newContact = new DatabaseContactDB(contact.Id, contact);
            await _databaseContactsRepository.InsertDatabaseContactAsync(newContact);
            return new CustomContactResponse(newContact);

        }

        public async Task<CustomContactResponse> UpdateContact(string contactId, UpdateCustomContactPayload contact, bool overwriteExistingContact)
        {
            DatabaseContactDB dbEntry = await _databaseContactsRepository.GetDatabaseContactsByIdAsync(contactId);
            if (dbEntry == null)
            {
                throw new DatabaseContactNotFound("Contact was not found in Database");
            }

            DatabaseContactDB updatedContact = new DatabaseContactDB(contactId, contact);
            if (!overwriteExistingContact)
            {
                updatedContact.Merge(dbEntry);
            }
            await _databaseContactsRepository.UpdateDatabaseContactAsync(updatedContact);
            return new CustomContactResponse(updatedContact);
        }

        public async Task DeleteContactById(string contactId)
        {
            DatabaseContactDB dbEntry = await _databaseContactsRepository.GetDatabaseContactsByIdAsync(contactId);
            if (dbEntry == null)
            {
                throw new DatabaseContactNotFound("Contact was not found in Database");
            }

            await _databaseContactsRepository.DeleteDatabaseContactAsync(contactId);
        }
    }
}
