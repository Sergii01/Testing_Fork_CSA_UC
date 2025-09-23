BEGIN
    IF NOT EXISTS (SELECT * FROM [UnifiedContacts].[AdminSettings] WHERE [key] = 'entraIdFilters' AND category = 'filterAttributes')
    BEGIN
        INSERT INTO [UnifiedContacts].[AdminSettings]([key], category, [value]) VALUES ('entraIdFilters', 'filterAttributes', '[ {"id": "72e58fb7-3995-4f14-a7e6-97cab5055982", "filterAttribute": "userType", "condition": "{2} eq ''{1}''", "filterValue": "Member", "isValid": true, "validationMessage": "", "filterString": "userType eq ''Member''" }, {"id": "72e58fb7-3995-4f14-a7e6-97cab5055983", "filterAttribute": "accountEnabled", "condition": "{2} eq ''{1}''", "filterValue": "true", "isValid": true, "validationMessage": "", "filterString": "accountEnabled eq true" } ]')
    END
END