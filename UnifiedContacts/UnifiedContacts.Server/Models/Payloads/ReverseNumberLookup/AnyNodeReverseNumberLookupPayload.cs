using System.Text.Json.Serialization;

namespace UnifiedContacts.Server.Models.Payloads.ReverseNumberLookup
{
    public class AnyNodeReverseNumberLookupPayload
    {
        [JsonPropertyName("sourceAddress")]
        public AnyNodeSourceAddress SourceAddress { get; set; }

        [JsonConstructor]
        public AnyNodeReverseNumberLookupPayload(AnyNodeSourceAddress sourceAddress)
        {
            SourceAddress = sourceAddress;
        }
    }
}
