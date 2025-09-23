namespace UnifiedContacts
{
    /// <summary>
    /// Source location where the contact was found
    /// </summary>
    public enum UnifiedContactsSource
    {
        /// <summary>
        /// Contact is from Azure Active Directory - https://docs.microsoft.com/en-us/graph/api/user-list?view=graph-rest-1.0
        /// </summary>
        AZURE_AD,

        /// <summary>
        /// Contact is from org contacts (Exchange global address list) - https://docs.microsoft.com/en-us/graph/api/orgcontact-list?view=graph-rest-1.0
        /// </summary>
        ORG_CONTACT,

        /// <summary>
        /// Contact is from personal contacts (Outlook address book) - https://docs.microsoft.com/en-us/graph/api/user-list-contacts?view=graph-rest-1.0
        /// </summary>
        USER_CONTACT,

        /// <summary>
        /// Contact is from SharePoint
        /// </summary>
        SHAREPOINT,

        /// <summary>
        /// Contact is from Database
        /// </summary>
        DATABASE
    }

    /// <summary>
    /// Available client types where Teams runs on
    /// </summary>
    public enum TeamsClientType
    {
        /// <summary>
        /// Teams Desktop
        /// </summary>
        DESKTOP,

        /// <summary>
        /// Teams in Browser
        /// </summary>
        WEB,

        /// <summary>
        /// Teams on mobile - Android
        /// </summary>
        ANDROID,

        /// <summary>
        /// Teams on mobile - iOS
        /// </summary>
        IOS,

        /// <summary>
        /// Teams on non of the above (e.g. Surface Hub)
        /// </summary>
        UNKNOWN
    }

    /// <summary>
    /// Format of image data
    /// </summary>
    public enum UnifiedContactsImageType
    {
        /// <summary>
        /// ImageData is a url that can be embedded to show the image
        /// </summary>
        URL,

        /// <summary>
        /// ImageData is the image itself base64 encoded
        /// </summary>
        BASE64,

        /// <summary>
        /// Contact does not have any ImageData
        /// </summary>
        NONE
    }

    public static class UnifiedContactsEnumConverter
    {
        public static bool TryConvertIdStringPrefixToUnifiedContactsSource(string stringToConvert, out UnifiedContactsSource? output)
        {
            // That syntax is needed here as the UnifiedContactStaticStrings aren't treated as constants here
            switch (stringToConvert)
            {
                case var value when value == UnifiedContactStaticStrings.CONTACT_ID_PREFIX_AZUREAD:
                    output = UnifiedContactsSource.AZURE_AD;
                    return true;

                case var value when value == UnifiedContactStaticStrings.CONTACT_ID_PREFIX_ORG_CONTACT:
                    output = UnifiedContactsSource.ORG_CONTACT;
                    return true;

                case var value when value == UnifiedContactStaticStrings.CONTACT_ID_PREFIX_USER_CONTACT:
                    output = UnifiedContactsSource.USER_CONTACT;
                    return true;

                case var value when value == UnifiedContactStaticStrings.CONTACT_ID_PREFIX_SHAREPOINT:
                    output = UnifiedContactsSource.SHAREPOINT;
                    return true;

                case var value when value == UnifiedContactStaticStrings.CONTACT_ID_PREFIX_DATABASE:
                    output = UnifiedContactsSource.DATABASE;
                    return true;

                default:
                    output = null;
                    return false;
            }
        }
    }
}