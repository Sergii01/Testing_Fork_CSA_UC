using System.Text.Json;
using System.Text.Json.Serialization;
using UnifiedContacts.Server.Models.DbTables;

namespace UnifiedContacts.Models.Dto
{
    public class RuntimeInfoSBCLookup
    {
        /// <summary>
        /// Is the SBC lookup endpoint enabled
        /// </summary>
        public bool EndpointEnabled { get; set; }

        /// <summary>
        /// Is the AnyNode SBC lookup endpoint enabled
        /// </summary>
        public bool AnyNodeEndpointEnabled { get; set; }

        /// <summary>
        /// Hashed Credential for SBC Lookup
        /// </summary>
        public string? HashedAuthenticationCredential { get; set; }

        public bool IsIpAuthenticationEnabled { get; set; } = true;
        public HashSet<string>? AllowedIpAddresses { get; set; }
    }

    public class RuntimeInfoEntraIdFilter
    {
        public string FilterString { get; set; }

        public bool? IsValid { get; set; }

        [JsonConstructor]
        public RuntimeInfoEntraIdFilter(string filterString, bool isValid)
        {
            FilterString = filterString;
            IsValid = isValid;
        }

        public static List<RuntimeInfoEntraIdFilter> FromJsonString(string value)
        {
            List<EntraIdFilterDB> filtersFromDb = JsonSerializer.Deserialize<List<EntraIdFilterDB>>(value) ?? [];
            List<RuntimeInfoEntraIdFilter> returnList = new();
            foreach (EntraIdFilterDB filter in filtersFromDb)
            {
                returnList.Add(new RuntimeInfoEntraIdFilter(filter.FilterString, filter.IsValid ?? false));
            }
            return returnList;
        }
    }

    public class RuntimeInfoDto
    {
        public bool DatabaseConfigured { get; set; } = false;
        public string? DatabaseErrorMessage { get; set; }
        public bool DbUpSuccessfull { get; set; } = false;
        public string? DbUpErrorMessage { get; set; }
        public bool KeyVaultConfigured { get; set; } = false;
        public string? KeyVaultErrorMessage { get; set; }

        /// <summary>
        /// Sources that are enabled for search. If this is null all Sources should be treated as allowed
        /// </summary>
        public HashSet<string>? EnabledSources;

        public RuntimeInfoSBCLookup SBCLookup { get; set; } = new RuntimeInfoSBCLookup();
        public List<RuntimeInfoEntraIdFilter> EntraIdFilter { get; set; } = new List<RuntimeInfoEntraIdFilter>();
    }
}