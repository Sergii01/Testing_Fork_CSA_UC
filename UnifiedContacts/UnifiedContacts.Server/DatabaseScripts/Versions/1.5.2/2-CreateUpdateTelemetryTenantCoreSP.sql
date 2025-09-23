IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._2.2-CreateUpdateTelemetryTenantCoreSP.sql')
BEGIN
    EXEC('CREATE OR ALTER PROCEDURE UnifiedContacts.UpdateTelemetryTenantCore
        @tenantName nvarchar(50)
    AS 
    BEGIN
        DECLARE @CurrentTelemetryTenantCoreInfo TABLE (id smallint, tenantName nvarchar(50), firstStartTimeStamp datetime2(0))

        INSERT INTO @CurrentTelemetryTenantCoreInfo SELECT * FROM (VALUES (1, @tenantName, GETUTCDATE())) AS t(id, tenantName, firstStartTimeStamp)

        UPDATE trg 
        SET trg.tenantName = ISNULL(src.tenantName, trg.tenantName)
        FROM [UnifiedContacts].[TelemetryTenantCore] AS trg JOIN @CurrentTelemetryTenantCoreInfo src on trg.id=src.id
        IF @@ROWCOUNT = 0 
        BEGIN 
            INSERT INTO [UnifiedContacts].[TelemetryTenantCore] SELECT * FROM @CurrentTelemetryTenantCoreInfo
        END
    END')
END