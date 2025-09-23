IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._0.1-CreateContactsTable.sql')
BEGIN
    BEGIN
        IF OBJECT_ID('[UnifiedContactsCustom].[Contacts]', 'U') IS NULL
        BEGIN
            CREATE TABLE [UnifiedContactsCustom].[Contacts] (
                [id]                   NVARCHAR (256) NOT NULL,
                [displayName]          NVARCHAR (256) NULL,
                [jobTitle]             NVARCHAR (256) NULL,
                [department]           NVARCHAR (256) NULL,
                [companyName]          NVARCHAR (256) NULL,
                [mailAddresses]        NVARCHAR (MAX) NULL,
                [imAddresses]          NVARCHAR (MAX) NULL,
                [mobilePhoneNumbers]   NVARCHAR (MAX) NULL,
                [businessPhoneNumbers] NVARCHAR (MAX) NULL,
                [homePhoneNumbers]     NVARCHAR (MAX) NULL,
                [addressFullString]    NVARCHAR (256) NULL,
                [addressStreetAddress] NVARCHAR (128) NULL,
                [addressPostalCode]    NVARCHAR (32)  NULL,
                [addressCity]          NVARCHAR (128) NULL,
                [addressCountry]       NVARCHAR (128) NULL,
                CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([id] ASC)
        );
        END
    END
END