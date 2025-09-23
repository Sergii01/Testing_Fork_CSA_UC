@{
    RootModule           = 'UnifiedContactsPS.psm1' 
    ModuleVersion        = '6.0.0' 
    CompatiblePSEditions = 'Desktop', 'Core' 
    GUID                 = 'dc18a919-f4bf-4da2-8c76-24b68fa33ef0' 
    # Author of this module
    Author               = 'glueckkanja AG'

    # Company or vendor of this module
    CompanyName          = 'glueckkanja AG'

    # Copyright statement for this module
    Copyright            = '(c) 2024 glueckkanja AG. All rights reserved.'
    Description          = 'UnifiedContactsPS'
    PowerShellVersion    = '7.1'
    FunctionsToExport    = @('Install-UnifiedContacts', 'Reset-UnifiedContacts', 'Uninstall-UnifiedContacts', 'Update-UnifiedContacts')
    CmdletsToExport      = @() 
    VariablesToExport    = @() 
    AliasesToExport      = @() 
    PrivateData          = @{
        PSData = @{
            IconUri = 'https://unifiedcontacts.blob.core.windows.net/arm-templates/Unified-Contacts-Pro-350.png'
        } 
    } 
}