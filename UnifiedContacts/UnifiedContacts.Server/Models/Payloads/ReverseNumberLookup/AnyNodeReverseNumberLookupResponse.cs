using System.Text.Json.Serialization;

namespace UnifiedContacts.Server.Models.Payloads.ReverseNumberLookup
{
    public class AnyNodeReverseNumberLookupResponse
    {
        [JsonPropertyName("routeContinue")]
        public bool RouteContinue { get; set; }
        [JsonPropertyName("sourceAddress")]
        public AnyNodeSourceAddress SourceAddress { get; set; }

        [JsonConstructor]
        public AnyNodeReverseNumberLookupResponse(bool routeContinue, AnyNodeSourceAddress sourceAddress)
        {
            RouteContinue = routeContinue;
            SourceAddress = sourceAddress;
        }
    }
}
