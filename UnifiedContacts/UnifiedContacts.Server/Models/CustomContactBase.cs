using System.ComponentModel.DataAnnotations;
using UnifiedContacts.Models.DbTables;

namespace UnifiedContacts.Models
{
    // example.test is used as a example domain here as test TLD is reserved/blocked to never go to market by RFC2606,RFCxxx
    public class CustomContactBase
    {
        /// <summary>
        /// DisplayName of the contact
        /// </summary>
        /// <example>John Doe</example>
        [StringLength(256, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? DisplayName { get; set; }
        /// <summary>
        /// JobTitle of the contact
        /// </summary>
        /// <example>Software Developer</example>
        [StringLength(256, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? JobTitle { get; set; }
        /// <summary>
        /// Department of the contact
        /// </summary>
        /// <example>Development</example>
        [StringLength(256, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? Department { get; set; }
        /// <summary>
        /// Name of company assosiated with the contact
        /// </summary>
        /// <example>Fantastic Company Inc.</example>
        [StringLength(256, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? CompanyName { get; set; }
        /// <summary>
        /// Email addresses of the contact
        /// </summary>
        /// <example>
        /// ["john.doe@example.test", "john.doe-alt@example.test"]
        /// </example>
        public IEnumerable<string>? MailAddresses { get; set; }
        /// <summary>
        /// Instant messaging addresses of the contact. Used for initiating a chat in Teams. If multiple imAddresses provided, the first one is used for intite a chat. If not provided the first eMail address of the contact is used.
        /// </summary>
        /// <example>
        /// ["john.doe@example.test", "john.doe-alt@example.test"]
        /// </example>
        public IEnumerable<string>? ImAddresses { get; set; }
        /// <summary>
        /// Mobile phone numbers of the contact
        /// </summary>
        /// <example>
        /// ["+123456789", "+198765432"]
        /// </example>
        public IEnumerable<string>? MobilePhoneNumbers { get; set; }
        /// <summary>
        /// Business phone numbers of the contact
        /// </summary>
        /// <example>
        /// ["+123456789", "+198765432"]
        /// </example>
        public IEnumerable<string>? BusinessPhoneNumbers { get; set; }
        /// <summary>
        /// Home phone numbers of the contact
        /// </summary>
        /// <example>
        /// ["+123456789", "+198765432"]
        /// </example>
        public IEnumerable<string>? HomePhoneNumbers { get; set; }
        /// <summary>
        /// Full address of the contact. If this is set it is used for displaying the address in the contact card. If not set the address is build from the other address properties.
        /// </summary>
        /// <example>Any Street 1, 12345 Any City, Any Country</example>
        [StringLength(256, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? AddressFullString { get; set; }
        /// <summary>
        /// Street address of the contact
        /// </summary>
        /// <example>Any Street 1</example>
        [StringLength(128, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? AddressStreetAddress { get; set; }
        /// <summary>
        /// Postal code of the contact
        /// </summary>
        /// <example>12345</example>
        [StringLength(32, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? AddressPostalCode { get; set; }
        /// <summary>
        /// City of the contact
        /// </summary>
        /// <example>Any City</example>
        [StringLength(128, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? AddressCity { get; set; }
        /// <summary>
        /// Country of the contact
        /// </summary>
        /// <example>Any Country</example>
        [StringLength(128, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? AddressCountry { get; set; }
        /// <summary>
        /// Sub source of the contact. This is used to identify the source of the contact if you are using multiple sources, that are synced into the database.
        /// </summary>
        /// <example>SAP</example>
        [StringLength(64, ErrorMessage = "{0} can have a maximum of {1} characters")]
        public string? Source { get; set; }

        public CustomContactBase() { }

        public CustomContactBase(DatabaseContactDB databaseContact)
        {
            DisplayName = databaseContact.DisplayName;
            JobTitle = databaseContact.JobTitle;
            Department = databaseContact.Department;
            CompanyName = databaseContact.CompanyName;
            MailAddresses = databaseContact.MailAddresses?.Split(";");
            ImAddresses = databaseContact.ImAddresses?.Split(";");
            MobilePhoneNumbers = databaseContact.MobilePhoneNumbers?.Split(";");
            BusinessPhoneNumbers = databaseContact.BusinessPhoneNumbers?.Split(";");
            HomePhoneNumbers = databaseContact.HomePhoneNumbers?.Split(";");
            AddressFullString = databaseContact.AddressFullString;
            AddressStreetAddress = databaseContact.AddressStreetAddress;
            AddressPostalCode = databaseContact.AddressPostalCode;
            AddressCity = databaseContact.AddressCity;
            AddressCountry = databaseContact.AddressCountry;
            Source = databaseContact.Source;
        }
    }
}
