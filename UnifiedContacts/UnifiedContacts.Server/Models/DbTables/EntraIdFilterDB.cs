using System.Text.Json.Serialization;

namespace UnifiedContacts.Server.Models.DbTables
{
    public class EntraIdFilterDB
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("filterAttribute")]
        public string FilterAttribute { get; set; }

        [JsonPropertyName("condition")]
        public string Condition { get; set; }

        [JsonPropertyName("filterValue")]
        public string FilterValue { get; set; }

        [JsonPropertyName("isValid")]
        public bool? IsValid { get; set; }

        [JsonPropertyName("validationMessage")]
        public string? ValidationMessage { get; set; }

        [JsonPropertyName("filterString")]
        public string FilterString { get; set; }

        [JsonConstructor]
        public EntraIdFilterDB(Guid id, string filterAttribute, string condition, string filterValue, bool? isValid, string? validationMessage, string filterString)
        {
            Id = id;
            FilterAttribute = filterAttribute;
            Condition = condition;
            FilterValue = filterValue;
            IsValid = isValid;
            ValidationMessage = validationMessage;
            FilterString = filterString;
        }

        public EntraIdFilterDB(string filterAttribute, string condition, string filterValue, string filterString, bool? isValid = null, string? validationMessage = null)
        {
            Id = Guid.NewGuid();
            FilterAttribute = filterAttribute;
            Condition = condition;
            FilterValue = filterValue;
            IsValid = isValid;
            ValidationMessage = validationMessage;
            FilterString = filterString;
        }
    }
}