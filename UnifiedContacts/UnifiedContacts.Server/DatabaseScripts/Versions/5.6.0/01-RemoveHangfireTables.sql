BEGIN
    DECLARE @sql nvarchar(max) = ''
    DECLARE @dropForeignKeySql nvarchar(max) = ''

    SELECT 
    @dropForeignKeySql = @dropForeignKeySql + 'ALTER TABLE [' +  OBJECT_SCHEMA_NAME(parent_object_id) +
    '].[' + OBJECT_NAME(parent_object_id) + 
    '] DROP CONSTRAINT [' + name + '];'
    FROM sys.foreign_keys
    WHERE referenced_object_id = object_id('HangFire.Job')

    SELECT @sql = @sql + 'DROP TABLE ' + QUOTENAME('HangFire') + '.' + QUOTENAME(t.name) + ';'
    FROM sys.tables t
    WHERE t.type = 'U' AND t.schema_id = SCHEMA_ID('HangFire')

    IF LEN(@dropForeignKeySql) > 0 -- We don't have to check if @sql is null because LEN(NULL) > 0 is false. Also @sql is initialized as empty string, hence never null.
        EXEC (@dropForeignKeySql)

    IF LEN(@sql) > 0 -- We don't have to check if @sql is null because LEN(NULL) > 0 is false. Also @sql is initialized as empty string, hence never null.
        EXEC (@sql)

    DROP SCHEMA IF EXISTS [Hangfire]
END