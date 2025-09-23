IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.RunFirst.InitializeDatabase.sql')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'UnifiedContacts')
    BEGIN
        EXEC( 'CREATE SCHEMA UnifiedContacts' );
    END
END