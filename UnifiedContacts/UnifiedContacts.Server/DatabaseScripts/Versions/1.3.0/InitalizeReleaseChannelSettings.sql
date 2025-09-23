
IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._3._0.InitalizeReleaseChannelSettings.sql')
BEGIN
    BEGIN
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'releaseChannel' AND category = 'update')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('releaseChannel', 'update', 'stable')
        END
    END
END