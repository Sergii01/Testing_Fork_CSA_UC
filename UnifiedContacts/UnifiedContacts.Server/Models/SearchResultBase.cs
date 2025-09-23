using System.Text.RegularExpressions;

namespace UnifiedContacts.Models
{
    /// <summary>
    /// Phone numbers of the contact
    /// </summary>
    public class SearchResultPhoneNumbers
    {
        /// <summary>
        /// Private/home phone numbers
        /// </summary>
        public List<string> Home { get; set; } = new List<string>();
        /// <summary>
        /// Business phone numbers
        /// </summary>
        public List<string> Business { get; set; } = new List<string>();
        /// <summary>
        /// Mobile phone numbers
        /// </summary>
        public List<string> Mobile { get; set; } = new List<string>();
        /// <summary>
        /// Other phone numbers
        /// </summary>
        public List<string> Other { get; set; } = new List<string>();
    }

    /// <summary>
    /// Class that holds all Address information
    /// </summary>
    public class SearchResultAddress
    {
        /// <summary>
        /// Street address of contact
        /// </summary>
        public string? StreetAddress { get; set; }
        /// <summary>
        /// Postal code of contact
        /// </summary>
        public string? PostalCode { get; set; }
        /// <summary>
        /// City of contact
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Country of contact
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// In case the above categories can't be served - This string contains all information in one string
        /// </summary>
        public string? AddressAltString { get; set; }

        public SearchResultAddress(string? streetAddress = null, string? postalCode = null, string? city = null, string? country = null, string? addressAltString = null)
        {
            if (!string.IsNullOrWhiteSpace(streetAddress))
            {
                StreetAddress = streetAddress;
            }
            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                PostalCode = postalCode;
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                City = city;
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                Country = country;
            }

            if (addressAltString == null)
            {
                if (!string.IsNullOrWhiteSpace(streetAddress) || !string.IsNullOrWhiteSpace(postalCode) || !string.IsNullOrWhiteSpace(city) || !string.IsNullOrWhiteSpace(country))
                {
                    AddressAltString = $"{$"{streetAddress}".Trim()}, {$"{postalCode}".Trim()} {$"{city}".Trim()}, {$"{country}".Trim()}";
                    AddressAltString = Regex.Replace(AddressAltString, " {2,}", " "); // removes multiple spaces
                    AddressAltString = Regex.Replace(AddressAltString, "(,( |)){2,}", ", "); // removes repeating commas
                    AddressAltString = AddressAltString.Trim(' ', ',');
                }
            }
            else
            {
                AddressAltString = addressAltString;
            }
        }
    }

    /// <summary>
    /// Street addresses of the contact
    /// </summary>
    public class SearchResultAddresses
    {
        /// <summary>
        /// Private/home addresses
        /// </summary>
        public List<SearchResultAddress> Home { get; set; } = new List<SearchResultAddress>();
        /// <summary>
        /// Business addresses
        /// </summary>
        public List<SearchResultAddress> Business { get; set; } = new List<SearchResultAddress>();
        /// <summary>
        /// Other addresses
        /// </summary>
        public List<SearchResultAddress> Other { get; set; } = new List<SearchResultAddress>();
    }

    public abstract class SearchResultBase
    {
        /// <summary>
        /// Source location where the contact was found
        /// </summary>
        public UnifiedContactsSource Source { get; set; }
        /// <summary>
        /// More detailed sub source location where the contact was found
        /// </summary>
        public string? SubSource { get; set; }
        /// <summary>
        /// Unique Id for every search result.
        /// Template: &lt;UnifiedContactStaticStrings&gt;.&lt;sourceprefix&gt;_&lt;ContactId&gt;
        /// The ContactId must be chosen so the image endpoint can acquire an image on base of this Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Displayname of the contact
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Job title of the contact
        /// </summary>
        public string? JobTitle { get; set; }
        /// <summary>
        /// Department of the contact
        /// </summary>
        public string? Department { get; set; }
        /// <summary>
        /// Company name of the contact
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// E-Mail addresses of the contact
        /// </summary>
        public List<string> MailAddresses { get; set; } = new List<string>();
        /// <summary>
        /// Instant messaging addresses of the contact - These are used for the Teams conversation deeplink
        /// </summary>
        public List<string> ImAddresses { get; set; } = new List<string>();
        /// <summary>
        /// Phone numbers of the contact
        /// </summary>
        public SearchResultPhoneNumbers PhoneNumbers { get; set; } = new SearchResultPhoneNumbers();
        /// <summary>
        /// Street addresses of the contact
        /// </summary>
        public SearchResultAddresses Addresses { get; set; } = new SearchResultAddresses();

        protected SearchResultBase(string id, UnifiedContactsSource source)
        {
            Id = id;
            Source = source;
        }
    }
}
