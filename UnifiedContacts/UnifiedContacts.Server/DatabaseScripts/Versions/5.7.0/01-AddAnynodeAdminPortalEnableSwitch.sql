BEGIN
    IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'anyNodeEndpointEnabled' AND category = 'sbcLookup')
    BEGIN
        INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('anyNodeEndpointEnabled', 'sbcLookup', '0')
    END
END