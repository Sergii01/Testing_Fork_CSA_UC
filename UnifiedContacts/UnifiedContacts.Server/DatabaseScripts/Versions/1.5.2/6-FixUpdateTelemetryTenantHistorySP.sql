IF NOT EXISTS (SELECT * FROM [dbo].[SchemaVersions] WHERE [ScriptName] = 'Unified_Contacts.DatabaseScripts.Versions._1._5._2.6-FixUpdateTelemetryTenantHistorySP.sql')
BEGIN
    EXEC('CREATE OR ALTER PROCEDURE UnifiedContacts.UpdateTelemetryTenantHistory
        @resultCountAad int,
        @resultCountUserContact int,
        @resultCountOrgContact int,
        @resultCountSharePoint int,
        @resultCountDatabase int,
        @requestCount int,
        @requestCountAndroid int,
        @requestCountIos int,
        @requestCountWeb int,
        @requestCountDesktop int,
        @requestCountUnknown int
    AS
    BEGIN
        DECLARE @HistoryIncrementTable TABLE ([date] date,
            resultCountAad int,
            resultCountUserContact int,
            resultCountOrgContact int,
            resultCountSharePoint int,
            resultCountDatabase int,
            requestCount int,
            requestCountAndroid int,
            requestCountIos int,
            requestCountWeb int,
            requestCountDesktop int,
            requestCountUnknown int,
            activeUserCountLast30Days int)

        INSERT INTO @HistoryIncrementTable
        SELECT *
        FROM (
                VALUES
                (CONVERT([date], GETUTCDATE()), @resultCountAad, @resultCountUserContact, @resultCountOrgContact, @resultCountSharePoint, @resultCountDatabase, @requestCount, @requestCountAndroid, @requestCountIos, @requestCountWeb, @requestCountDesktop, @requestCountUnknown)
            ) AS t ([date], resultCountAad, resultCountUserContact, resultCountOrgContact, resultCountSharePoint, resultCountDatabase, requestCount, requestCountAndroid, requestCountIos, requestCountWeb, requestCountDesktop, requestCountUnknown)
            LEFT JOIN (
                SELECT COUNT(userId)
            FROM [UnifiedContacts].[UsageStatistics]
            WHERE DATEDIFF(day, lastAccessDateTime, GETUTCDATE()) < 30
            ) AS t2 (activeUserCountLast30Days)
            ON 1 = 1

        UPDATE trg 
        SET trg.resultCountAad = trg.resultCountAad + src.resultCountAad, trg.resultCountUserContact = trg.resultCountUserContact + src.resultCountUserContact, trg.resultCountOrgContact = trg.resultCountOrgContact + src.resultCountOrgContact, trg.resultCountSharePoint = trg.resultCountSharePoint + src.resultCountSharePoint, trg.resultCountDatabase = trg.resultCountDatabase + src.resultCountDatabase, trg.requestCount = trg.requestCount + src.requestCount, trg.requestCountAndroid = trg.requestCountAndroid + src.requestCountAndroid, trg.requestCountIos = trg.requestCountIos + src.requestCountIos, trg.requestCountWeb = trg.requestCountWeb + src.requestCountWeb, trg.requestCountDesktop = trg.requestCountDesktop + src.requestCountDesktop, trg.requestCountUnknown = trg.requestCountUnknown + src.requestCountUnknown, trg.activeUserCountLast30Days = src.activeUserCountLast30Days 
        FROM [UnifiedContacts].[TelemetryTenantHistory] trg JOIN @HistoryIncrementTable src on trg.[date]=src.[date]
        IF @@ROWCOUNT = 0 
        BEGIN
            INSERT INTO [UnifiedContacts].[TelemetryTenantHistory]
                ([date], resultCountAad, resultCountUserContact, resultCountOrgContact, resultCountSharePoint, resultCountDatabase, requestCount, requestCountAndroid, requestCountIos, requestCountWeb, requestCountDesktop, requestCountUnknown, activeUserCountLast30Days)
            SELECT [date], resultCountAad, resultCountUserContact, resultCountOrgContact, resultCountSharePoint, resultCountDatabase, requestCount, requestCountAndroid, requestCountIos, requestCountWeb, requestCountDesktop, requestCountUnknown, activeUserCountLast30Days
            FROM @HistoryIncrementTable
        END
    END')
END