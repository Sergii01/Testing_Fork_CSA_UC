IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._5._3._0.1-InitializeEnabledSourcesSettings.sql')
BEGIN
    BEGIN
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'azuread' AND category = 'enabledSources')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('azuread', 'enabledSources', '1')
        END
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'usercontacts' AND category = 'enabledSources')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('usercontacts', 'enabledSources', '1')
        END
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'orgcontacts' AND category = 'enabledSources')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('orgcontacts', 'enabledSources', '1')
        END
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'sharepoint' AND category = 'enabledSources')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('sharepoint', 'enabledSources', '1')
        END
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'database' AND category = 'enabledSources')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('database', 'enabledSources', '1')
        END
    END
END