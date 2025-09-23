IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._2.1-CreateTelemetryCacheTables.sql')
BEGIN
    BEGIN
        IF OBJECT_ID('[UnifiedContactsCustom].[TelemetryTenantCore]', 'U') IS NULL
        BEGIN
            CREATE TABLE [UnifiedContacts].[TelemetryTenantCore] (
                [id]                  SMALLINT      CONSTRAINT [DEFAULT_TelemetryTenantCore_id] DEFAULT 1 NOT NULL,
                [tenantName]          NVARCHAR (50) NULL,
                [firstStartTimeStamp] DATETIME2 (0) CONSTRAINT [DEFAULT_TelemetryTenantMaster_firstStartTimeStamp] DEFAULT GETUTCDATE() NULL,
                CONSTRAINT [PK_TelemetryTenantCore] PRIMARY KEY CLUSTERED ([id] ASC)
            );
        END

        IF OBJECT_ID('[UnifiedContactsCustom].[TelemetryTenantHistory]', 'U') IS NULL
        BEGIN
            CREATE TABLE [UnifiedContacts].[TelemetryTenantHistory] (
                [date]                      DATE CONSTRAINT [DEFAULT_TelemetryTenantHistory_date] DEFAULT GETUTCDATE() NOT NULL,
                [resultCountAad]            INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_resultCountAad] DEFAULT 0 NOT NULL,
                [resultCountUserContact]    INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_resultCountUserContact] DEFAULT 0 NOT NULL,
                [resultCountOrgContact]     INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_resultCountOrgContact] DEFAULT 0 NOT NULL,
                [resultCountSharePoint]     INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_resultCountSharePoint] DEFAULT 0 NOT NULL,
                [resultCountDatabase]       INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_resultCountDatabase] DEFAULT 0 NOT NULL,
                [requestCount]              INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_requestCount] DEFAULT 0 NOT NULL,
                [activeUserCountLast30Days] INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_activeUserCountLast30Days] DEFAULT 0 NOT NULL,
                [requestCountAndroid]       INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_requestCountAndroid] DEFAULT 0 NOT NULL,
                [requestCountIos]           INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_requestCountIos] DEFAULT 0 NOT NULL,
                [requestCountWeb]           INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_requestCountWeb] DEFAULT 0 NOT NULL,
                [requestCountDesktop]       INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_requestCountDesktop] DEFAULT 0 NOT NULL,
                [requestCountUnknown]       INT  CONSTRAINT [DEFAULT_TelemetryTenantHistory_requestCountUnknown] DEFAULT 0 NOT NULL,
                CONSTRAINT [PK_TelemetryTenantHistory] PRIMARY KEY CLUSTERED ([date] ASC)
            );
        END
    END
END