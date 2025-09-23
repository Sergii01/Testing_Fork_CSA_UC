IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._3._0.CreateFavoritesTable.sql')
BEGIN
    CREATE TABLE [UnifiedContacts].[AdminSettings] (
        [key]      NVARCHAR (256) NOT NULL,
        [category] NVARCHAR (256) NOT NULL,
        [value]    NVARCHAR (MAX) NULL,
        CONSTRAINT [PK_AdminSettings] PRIMARY KEY CLUSTERED ([key] ASC, [category] ASC)
    );
END