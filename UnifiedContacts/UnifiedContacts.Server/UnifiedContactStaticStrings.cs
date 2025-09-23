namespace UnifiedContacts
{
    public static class UnifiedContactStaticStrings
    {
        /// <summary>
        /// Prefix used for ConatctId with AzureAD contacts
        /// </summary>
        public const string CONTACT_ID_PREFIX_AZUREAD = "aad";
        /// <summary>
        /// Prefix used for ConatctId with org contact contacts
        /// </summary>
        public const string CONTACT_ID_PREFIX_ORG_CONTACT = "orgCon";
        /// <summary>
        /// Prefix used for ConatctId with user contact contacts
        /// </summary>
        public const string CONTACT_ID_PREFIX_USER_CONTACT = "usrCon";
        /// <summary>
        /// Prefix used for ConatctId with SharePoint contacts
        /// </summary>
        public const string CONTACT_ID_PREFIX_SHAREPOINT = "sp";
        /// <summary>
        /// Prefix used for ContactId with Database contacts
        /// </summary>
        public const string CONTACT_ID_PREFIX_DATABASE = "db";

        public const string SOURCE_AZURE_ID = "azuread";
        public const string SOURCE_USER_CONTACTS = "usercontacts";
        public const string SOURCE_ORG_CONTACTS = "orgcontacts";
        public const string SOURCE_SHAREPOINT = "sharepoint";
        public const string SOURCE_DATABASE = "database";

        public const string SETTINGS_SBC_LOOKUP_CATEGORY_ID = "sbcLookup";
        public const string SETTINGS_SBC_LOOKUP_CREDENTIALS_SETTING_ID = "endpointCredentials";
        public const string SETTINGS_SBC_LOOKUP_CREDENTIALS_LAST_MODIFIED_BY_ID = "endpointCredentialsLastModifiedBy";
        public const string SETTINGS_SBC_LOOKUP_CREDENTIALS_LAST_MODIFIED_ID = "endpointCredentialsLastModified";
        public const string SETTINGS_SBC_LOOKUP_IP_AUTHENTICATION_ENABLED_SETTING_ID = "ipAuthenticationEnabled";
        public const string SETTINGS_SBC_LOOKUP_ALLOWED_IP_ADDRESSES_SETTING_ID = "allowedIpAddresses";
        public const string SETTINGS_SBC_LOOKUP_ENDPOINT_ENABLED_SETTING_ID = "endpointEnabled";
        public const string SETTINGS_SBC_LOOKUP_ANY_NODE_ENDPOINT_ENABLED_SETTING_ID = "anyNodeEndpointEnabled";

        public const string SETTINGS_ENTRAIDFILTER_ID = "entraIdFilters";
        public const string SETTINGS_ENTRAIDFILTER_CATEGORY = "filterAttributes";

        public const string HEALTH_STATE_DISPLAY_NAME_TEAMS_APP_REGISTRATION = "Teams App Registration";
        public const string HEALTH_STATE_DISPLAY_NAME_KEY_VAULT = "Key Vault";
        public const string HEALTH_STATE_DISPLAY_NAME_DATABASE = "Database";
        public const string HEALTH_STATE_DISPLAY_NAME_LICENSE_VALIDATION = "License Validation";
        public const string HEALTH_STATE_DISPLAY_NAME_STORAGE_ACCOUNT = "Storage Account";
        public const string HEALTH_STATE_DISPLAY_NAME_ADMIN_PAGE_BACKEND = "Admin Page Backend";

    }
}
