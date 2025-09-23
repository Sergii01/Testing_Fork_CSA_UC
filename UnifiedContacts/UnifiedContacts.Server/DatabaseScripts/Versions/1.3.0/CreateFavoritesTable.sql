IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._3._0.CreateFavoritesTable.sql')
BEGIN
    CREATE TABLE [UnifiedContacts].[Favorites](
        [userId] [uniqueidentifier] NOT NULL,
        [contactId] [nvarchar](256) NOT NULL,
        [tenantId] [uniqueidentifier] NOT NULL,
     CONSTRAINT [PK_Favorites] PRIMARY KEY CLUSTERED 
    (
        [userId] ASC,
        [tenantId] ASC,
        [contactId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END