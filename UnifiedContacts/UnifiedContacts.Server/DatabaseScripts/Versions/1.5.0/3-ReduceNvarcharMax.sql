IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._0.3-ReduceNvarcharMax.sql')
BEGIN
    BEGIN
        ALTER TABLE [UnifiedContactsCustom].[Contacts]
        ALTER COLUMN [mailAddresses] NVARCHAR(4000) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts]
        ALTER COLUMN [imAddresses] NVARCHAR(4000) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts]
        ALTER COLUMN [mobilePhoneNumbers] NVARCHAR(4000) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts]
        ALTER COLUMN [businessPhoneNumbers] NVARCHAR(4000) NULL;
        ALTER TABLE [UnifiedContactsCustom].[Contacts]
        ALTER COLUMN [homePhoneNumbers] NVARCHAR(4000) NULL;
    END
END