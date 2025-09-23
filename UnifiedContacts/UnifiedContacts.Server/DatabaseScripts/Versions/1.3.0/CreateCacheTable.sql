IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._3._0.CreateCacheTable.sql')
BEGIN
    CREATE TABLE [UnifiedContacts].[Cache] (
        [key]      NVARCHAR (256) NOT NULL,
        [value]    NVARCHAR (MAX) NULL,
        CONSTRAINT [PK_Cache] PRIMARY KEY CLUSTERED ([key] ASC)
    );
END