using UnifiedContacts.Interfaces;

namespace UnifiedContacts.Engines.SearchEngines
{
    public class SearchEngineFactory
    {
        public readonly Dictionary<UnifiedContactsSource, ISearchEngine> AllSearchEnginesDict;
        public SearchEngineFactory(AzureADSearchEngine azureADSearchEngine, OrgContactsSearchEngine orgContactsSearchEngine, UserContactsSearchEngine userContactsSearchEngine, SharePointSearchEngine sharepointSearchEngine, DatabaseSearchEngine databaseSearchEngine)
        {
            AllSearchEnginesDict = new Dictionary<UnifiedContactsSource, ISearchEngine>
            {
                { UnifiedContactsSource.AZURE_AD, azureADSearchEngine },
                { UnifiedContactsSource.ORG_CONTACT, orgContactsSearchEngine },
                { UnifiedContactsSource.USER_CONTACT, userContactsSearchEngine },
                { UnifiedContactsSource.SHAREPOINT, sharepointSearchEngine },
                { UnifiedContactsSource.DATABASE, databaseSearchEngine }
            };
        }
    }
}
