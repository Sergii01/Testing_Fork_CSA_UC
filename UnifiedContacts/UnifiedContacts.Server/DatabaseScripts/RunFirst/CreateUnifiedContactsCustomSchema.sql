IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.RunFirst.CreateUnifiedContactsCustomSchema.sql')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'UnifiedContactsCustom')
    BEGIN
        EXEC( 'CREATE SCHEMA UnifiedContactsCustom' );
    END
END