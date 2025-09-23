using Dapper.Contrib.Extensions;

namespace UnifiedContacts.Models.DbTables
{
    [Table("UnifiedContactsCustom.Contacts")]
    public class DatabaseContactDB
    {
        [ExplicitKey]
        public string Id { get; set; }
        public string? DisplayName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public string? CompanyName { get; set; }
        public string? MailAddresses { get; set; }
        public string? ImAddresses { get; set; }
        public string? MobilePhoneNumbers { get; set; }
        public string? BusinessPhoneNumbers { get; set; }
        public string? HomePhoneNumbers { get; set; }
        public string? AddressFullString { get; set; }
        public string? AddressStreetAddress { get; set; }
        public string? AddressPostalCode { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressCountry { get; set; }
        public string? Source { get; set; }
        public DateTime? InsertionDate { get; set; }

        public DatabaseContactDB(string id, string? displayName, string? jobTitle, string? department, string? companyName, string? mailAddresses, string? imAddresses, string? mobilePhoneNumbers, string? businessPhoneNumbers, string? homePhoneNumbers, string? addressFullString, string? addressStreetAddress, string? addressPostalCode, string? addressCity, string? addressCountry, string? source, DateTime? insertionDate)
        {
            Id = id;
            DisplayName = displayName;
            JobTitle = jobTitle;
            Department = department;
            CompanyName = companyName;
            MailAddresses = mailAddresses;
            ImAddresses = imAddresses;
            MobilePhoneNumbers = mobilePhoneNumbers;
            BusinessPhoneNumbers = businessPhoneNumbers;
            HomePhoneNumbers = homePhoneNumbers;
            AddressFullString = addressFullString;
            AddressStreetAddress = addressStreetAddress;
            AddressPostalCode = addressPostalCode;
            AddressCity = addressCity;
            AddressCountry = addressCountry;
            Source = source;
            InsertionDate = insertionDate;
        }

        public DatabaseContactDB(string id, CustomContactBase contact)
        {
            Id = id;
            DisplayName = contact.DisplayName;
            JobTitle = contact.JobTitle;
            Department = contact.Department;
            CompanyName = contact.CompanyName;
            if (contact.MailAddresses != null)
            {
                MailAddresses = string.Join(";", contact.MailAddresses);
            }
            if (contact.ImAddresses != null)
            {
                ImAddresses = string.Join(";", contact.ImAddresses);
            }
            if (contact.MobilePhoneNumbers != null)
            {
                MobilePhoneNumbers = string.Join(";", contact.MobilePhoneNumbers);
            }
            if (contact.BusinessPhoneNumbers != null)
            {
                BusinessPhoneNumbers = string.Join(";", contact.BusinessPhoneNumbers);
            }
            if (contact.HomePhoneNumbers != null)
            {
                HomePhoneNumbers = string.Join(";", contact.HomePhoneNumbers);
            }
            AddressFullString = contact.AddressFullString;
            AddressStreetAddress = contact.AddressStreetAddress;
            AddressPostalCode = contact.AddressPostalCode;
            AddressCity = contact.AddressCity;
            AddressCountry = contact.AddressCountry;
            Source = contact.Source;
        }

        public void Merge(DatabaseContactDB source)
        {
            Type t = typeof(DatabaseContactDB);

            IEnumerable<System.Reflection.PropertyInfo> properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (System.Reflection.PropertyInfo? prop in properties)
            {
                object? targetValue = prop.GetValue(this);
                object? sourceValue = prop.GetValue(source);
                if (targetValue == null && sourceValue != null)
                {
                    prop.SetValue(this, sourceValue);
                }
            }
        }
    }
}
