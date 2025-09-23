using System.Text.Json.Serialization;

namespace UnifiedContacts.Server.Models.Payloads
{
    public class EntraIdFilterPayload
    {
        [JsonPropertyName("filterAttribute")]
        public string FilterAttribute { get; set; }

        [JsonPropertyName("condition")]
        public string Condition { get; set; }

        [JsonPropertyName("filterValue")]
        public string FilterValue { get; set; }

        [JsonConstructor]
        public EntraIdFilterPayload(string filterAttribute, string condition, string filterValue)
        {
            FilterAttribute = filterAttribute;
            Condition = condition;
            FilterValue = filterValue;
        }
    }
}