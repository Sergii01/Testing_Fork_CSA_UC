using System.Text.Json.Serialization;

namespace UnifiedContacts.Server.Models.Payloads.ReverseNumberLookup
{
    public class AnyNodeSourceAddress
    {
        [JsonPropertyName("dialString")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DialString { get; set; }

        [JsonPropertyName("displayName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        [JsonConstructor]
        public AnyNodeSourceAddress(string? dialString, string? displayName)
        {
            DialString = dialString;
            DisplayName = displayName;
        }
    }
}
