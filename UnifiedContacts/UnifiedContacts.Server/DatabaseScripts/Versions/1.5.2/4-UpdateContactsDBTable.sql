IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._2.4-UpdateContactsDBTable.sql')
BEGIN
    ALTER TABLE [UnifiedContactsCustom].[Contacts]
        ADD insertionDate DATETIME2(0) NOT NULL DEFAULT GETUTCDATE()
END