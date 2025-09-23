IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._5._3._0.2-InitializeSBCLookupSettings.sql')
BEGIN
    BEGIN
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'endpointCredentials' AND category = 'sbcLookup')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('endpointCredentials', 'sbcLookup', NULL)
        END
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'ipAuthenticationEnabled' AND category = 'sbcLookup')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('ipAuthenticationEnabled', 'sbcLookup', '1')
        END
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'allowedIpAddresses' AND category = 'sbcLookup')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('allowedIpAddresses', 'sbcLookup', NULL)
        END
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'endpointEnabled' AND category = 'sbcLookup')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('endpointEnabled', 'sbcLookup', '0')
        END
    END
END