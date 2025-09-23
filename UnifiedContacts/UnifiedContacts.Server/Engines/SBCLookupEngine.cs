using System.Security.Cryptography;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Repositories;

namespace UnifiedContacts.Engines
{
    public class SBCLookupEngine
    {
        private readonly DatabaseContactsRepository _databaseContactsRepository;
        public SBCLookupEngine(DatabaseContactsRepository databaseContactsRepository)
        {
            _databaseContactsRepository = databaseContactsRepository;
        }

        public string GetCredentialHash(byte[] credentialBytes)
        {
            byte[] salt = credentialBytes.Reverse().ToArray();
            byte[] hashedCredentialByteArray = Rfc2898DeriveBytes.Pbkdf2(credentialBytes, salt, 600001, HashAlgorithmName.SHA256, 32);
            return Convert.ToHexString(hashedCredentialByteArray);
        }

        private static string GetEscapedPhoneNumber(string rawNumber)
        {
            //Remove '(0)' and '(', ')', '+', '-', '/', ' '
            return rawNumber.Replace("(0)", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("+", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty);
        }

        public async Task<DatabaseContactDB?> SearchContactByPhoneNumberAsync(string number)
        {
            IEnumerable<DatabaseContactDB> potentialContacts = await _databaseContactsRepository.SearchDatabaseContactsByPhoneNumberAsync(GetEscapedPhoneNumber(number));
            foreach (DatabaseContactDB contact in potentialContacts)
            {
                List<string> numbersOfUser = new List<string>();
                if (contact.BusinessPhoneNumbers != null)
                {
                    numbersOfUser.AddRange(contact.BusinessPhoneNumbers.Split(";"));
                }
                if (contact.HomePhoneNumbers != null)
                {
                    numbersOfUser.AddRange(contact.HomePhoneNumbers.Split(";"));
                }
                if (contact.MobilePhoneNumbers != null)
                {
                    numbersOfUser.AddRange(contact.MobilePhoneNumbers.Split(";"));
                }

                IEnumerable<string> numbersWithExactMatch = numbersOfUser.Where(rawNumber =>
                {
                    return GetEscapedPhoneNumber(number) == GetEscapedPhoneNumber(rawNumber);
                });

                if (!numbersWithExactMatch.IsNullOrEmpty())
                {
                    return contact;
                }
            }

            return null;
        }
    }
}
