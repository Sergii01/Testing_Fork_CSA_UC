IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._2.5-ExtendNvarcharContactsDBTable.sql')
BEGIN
    BEGIN
        ALTER TABLE [UnifiedContactsCustom].[Contacts] ALTER COLUMN [addressCity] NVARCHAR (512) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts] ALTER COLUMN [addressCountry] NVARCHAR (512) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts] ALTER COLUMN [addressFullString] NVARCHAR (512) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts] ALTER COLUMN [addressPostalCode] NVARCHAR (512) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts] ALTER COLUMN [addressStreetAddress] NVARCHAR (512) NULL;
    END
END