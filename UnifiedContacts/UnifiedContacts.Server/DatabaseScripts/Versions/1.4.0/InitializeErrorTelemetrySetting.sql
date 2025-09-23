IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._3._0.InitializeErrorTelemetrySetting.sql')
BEGIN
    BEGIN
        IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'errorTelemetryLevel' AND category = 'telemetry')
        BEGIN
            INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('errorTelemetryLevel', 'telemetry', '1')
        END
    END
END