IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._0.2-AddSourceColumn.sql')
BEGIN
    BEGIN
        ALTER TABLE [UnifiedContactsCustom].[Contacts]
        ADD [source] NVARCHAR(64) NULL
    END
END