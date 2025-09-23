using System.Text.Json.Serialization;

namespace UnifiedContacts.Models.Responses
{
    public class GetVersionResponse
    {
        /// <summary>
        /// Version of UnifiedContacts environment
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="version">Version of UnifiedContacts environemnt</param>
        /// <param name="edition">Edition of the UnifiedContacts environment</param>
        [JsonConstructor]
        public GetVersionResponse(string version)
        {
            Version = version;
        }
    }
}
