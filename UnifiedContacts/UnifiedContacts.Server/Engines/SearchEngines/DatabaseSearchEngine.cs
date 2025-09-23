using System.Security.Claims;
using UnifiedContacts.Interfaces;
using UnifiedContacts.Models;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Models.Exceptions;
using UnifiedContacts.Repositories;

namespace UnifiedContacts.Engines.SearchEngines
{
    public class DatabaseSearchEngine : ISearchEngine
    {
        private readonly DatabaseContactsRepository _databaseContactsRepository;
        private const string LIST_CELL_DELIMITER = ";";
        public DatabaseSearchEngine(DatabaseContactsRepository databaseContactsRepository)
        {
            _databaseContactsRepository = databaseContactsRepository;
        }
        private List<SearchEngineResultDto> ConvertToSearchEngineResults(IEnumerable<DatabaseContactDB> databaseContacts)
        {
            List<SearchEngineResultDto> returnDtos = new List<SearchEngineResultDto>();
            foreach (DatabaseContactDB databaseContact in databaseContacts)
            {
                SearchEngineResultDto searchResult = new SearchEngineResultDto($"{UnifiedContactStaticStrings.CONTACT_ID_PREFIX_DATABASE}_{databaseContact.Id}", UnifiedContactsSource.DATABASE)
                {
                    SubSource = databaseContact.Source,
                    DisplayName = databaseContact.DisplayName,
                    JobTitle = databaseContact.JobTitle,
                    Department = databaseContact.Department,
                    CompanyName = databaseContact.CompanyName
                };

                if (!string.IsNullOrWhiteSpace(databaseContact.ImAddresses))
                {
                    searchResult.ImAddresses.AddRange(databaseContact.ImAddresses.Split(LIST_CELL_DELIMITER));
                }
                if (!string.IsNullOrWhiteSpace(databaseContact.MailAddresses))
                {
                    searchResult.MailAddresses.AddRange(databaseContact.MailAddresses.Split(LIST_CELL_DELIMITER));
                }
                if (!string.IsNullOrWhiteSpace(databaseContact.HomePhoneNumbers))
                {
                    searchResult.PhoneNumbers.Home.AddRange(databaseContact.HomePhoneNumbers.Split(LIST_CELL_DELIMITER));
                }
                if (!string.IsNullOrWhiteSpace(databaseContact.BusinessPhoneNumbers))
                {
                    searchResult.PhoneNumbers.Business.AddRange(databaseContact.BusinessPhoneNumbers.Split(LIST_CELL_DELIMITER));
                }
                if (!string.IsNullOrWhiteSpace(databaseContact.MobilePhoneNumbers))
                {
                    searchResult.PhoneNumbers.Mobile.AddRange(databaseContact.MobilePhoneNumbers.Split(LIST_CELL_DELIMITER));
                }
                if (!string.IsNullOrWhiteSpace(databaseContact.AddressFullString) || !string.IsNullOrWhiteSpace(databaseContact.AddressStreetAddress) || !string.IsNullOrWhiteSpace(databaseContact.AddressPostalCode) || !string.IsNullOrWhiteSpace(databaseContact.AddressCity) || !string.IsNullOrWhiteSpace(databaseContact.AddressCountry))
                {
                    searchResult.Addresses.Business.Add(new SearchResultAddress(databaseContact.AddressStreetAddress, databaseContact.AddressPostalCode, databaseContact.AddressCity, databaseContact.AddressCountry, databaseContact.AddressFullString));
                }
                returnDtos.Add(searchResult);
            }
            return returnDtos;
        }

        public async Task<IEnumerable<SearchEngineResultDto>> GetFavoritesAsync(IEnumerable<string> contactIds, string delegatedToken, ClaimsPrincipal userPrincipal)
        {
            List<string> databaseContactIds = contactIds.Where(contactId => contactId.StartsWith($"{UnifiedContactStaticStrings.CONTACT_ID_PREFIX_DATABASE}_")).ToList().ConvertAll(contactId => contactId.Replace($"{UnifiedContactStaticStrings.CONTACT_ID_PREFIX_DATABASE}_", string.Empty));
            IEnumerable<DatabaseContactDB> favorites = await _databaseContactsRepository.GetDatabaseContactsByIdsAsync(databaseContactIds);
            return ConvertToSearchEngineResults(favorites);
        }

        public async Task<IEnumerable<SearchEngineResultDto>> SearchContactAsync(string searchQuery, string delegatedToken, ClaimsPrincipal userPrincipal)
        {
            try
            {
                IEnumerable<DatabaseContactDB> contacts = await _databaseContactsRepository.SearchDatabaseContactsAsync(searchQuery);
                return ConvertToSearchEngineResults(contacts);
            }
            catch (DatabaseNotConfiguredException) // If Database is not configured, return empty list not Error as this might be a valid case for free version usage (Especially for the SaaS version)
            {
                return Enumerable.Empty<SearchEngineResultDto>();
            }
        }
    }
}
