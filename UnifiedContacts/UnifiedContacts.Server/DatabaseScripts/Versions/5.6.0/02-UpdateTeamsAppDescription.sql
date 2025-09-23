-- We can do this edit because we never allowed the editing of this block so far - so we can safetly just overwrite it.
BEGIN
    UPDATE [UnifiedContacts].[AdminSettings] SET [value] = 'Finding ALL your Contacts in Microsoft Teams, Outlook & Microsoft 365' WHERE [key] = 'shortDescription' AND [category] = 'teamsManifest'
    UPDATE [UnifiedContacts].[AdminSettings] SET [value] = 'Unified Contacts extends Microsoft Teams, Outlook and Microsoft 365 by delivering a unified search experience for the two most popular address books: EntraId (fka: Azure Active Directory) and Microsoft Exchange Online aswell as your custom address books from SharePoint or your Unified Contacts database.

With Unified Contacts you can simultaneously search your Entra Id for corporate contacts, Exchange Online address book for personal and other organization-wide contacts and even custom contacts from SharePoint or your Unified Contacts database. Search results will be presented in a unified, well-structured, and comprehensive result page displaying contacts from all sources.

If your contacts contain multiple phone numbers, Unified Contacts allows easy selection of your desired phone number for dialing (dialing requires Microsoft Teams Phone System enabled users). In addition, you can directly initiate a Microsoft Teams call, start a Microsoft Teams chat, or write a mail from the contact card. For internal users the presence status is displayed as well.

Unified Contacts searches contacts by
- First name
- Last name
- Job title
- Department
- Organization name

For more information on features and technical details, please refer to our [documentation](https://docs.unified-contacts.com/).' WHERE [key] = 'longDescription' AND [category] = 'teamsManifest'
END