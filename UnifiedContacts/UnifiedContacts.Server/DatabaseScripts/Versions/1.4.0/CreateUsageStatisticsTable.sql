IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._4._0.CreateUsageStatisticsTable.sql')
BEGIN
    BEGIN
        CREATE TABLE [UnifiedContacts].[UsageStatistics] (
            [userId]     UNIQUEIDENTIFIER NOT NULL,
            [tenantId]   UNIQUEIDENTIFIER NOT NULL,
            [lastAccessDateTime] DATETIME2 (0)    CONSTRAINT [DEFAULT_UsageStatistics_lastAccess] DEFAULT GETUTCDATE() NOT NULL,
            CONSTRAINT [PK_UsageStatistics] PRIMARY KEY CLUSTERED ([userId] ASC, [tenantId] ASC)
        );
    END
END