function Install-UnifiedContacts {
    [CmdletBinding()]
    Param(
        [Parameter (Mandatory = $true, HelpMessage = "The Url you can copy from your browser when you open the App Service. Format: https://portal.azure.com/#<domain>/resource/subscriptions/<subscriptionId>/resourceGroups/<ressourceGroupName>/providers/Microsoft.Web/sites/<appServiceName>/appServices")][string]$AppServiceAzureUrl,
        [Parameter (Mandatory = $true, HelpMessage = "user name and password of the sql user")][PSCredential]$SqlCredential
    )
    $ErrorActionPreference = "Stop"

    Import-Module Az.Resources
    try {
        $resourceGroup = (($AppServiceAzureUrl -Split "resourceGroups/")[1] -Split "/")[0]
        $subscriptionId = (($AppServiceAzureUrl -Split "subscriptions/")[1] -Split "/")[0]
        $appServiceName = (($AppServiceAzureUrl -Split "sites/")[1] -Split "/")[0]
        
        Get-LatestModuleVersion
       
        Write-Host "ResourceGroup1: $resourceGroup`nSubscriptionId: $subscriptionId`nAppServiceName: $appServiceName"
        Write-Progress -Activity "Deployment Unified Contacts" -Status "Get Web App Information" -PercentComplete 5

        try {
            $context = Get-AzContext
        }
        catch {
            #fall through
        }
        if ($null -eq $context) {
            [void] (Connect-AzAccount -Subscription $subscriptionId)
        }
        elseif ($context.Subscription.Id -ne $subscriptionId) {
            [void] (Set-AzContext -Subscription $subscriptionId)
        }

        #refresh Context
        $context = Get-AzContext
        $User = (az account show --query user.name --output tsv)
        $TenantId = $context.Tenant.Id
        $appService = Get-AzWebApp -ResourceGroupName  $resourceGroup -Name $appServiceName 
        $redirectUri = "https://$($appService.EnabledHostNames[0])"
        
        if ($appservice.EnabledHostNames.Count -gt 2) {
            $i = 0
            while ($i -lt $appservice.EnabledHostNames.Count) {
                Write-Host "[$i] $($appservice.EnabledHostNames[$i])" 
                $i++
            }
            $redirectUriNumber = Read-Host -Prompt "Please enter the number of your custom domain" 
            do {
                $redirectUri = "https://$($appService.EnabledHostNames[$redirectUriNumber])"
            }while (-not ($redirectUriNumber -le $appservice.EnabledHostNames.Count -and $redirectUriNumber -ge 0))
        }

        Write-Progress -Activity "Deployment Unified Contacts" -Status "Create Admin Page App registration" -PercentComplete 10

        $appRegAdminPage = New-AppRegistration -AppRegistrationName ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "AppRegistrationNameAdminPage" }).value -RedirectUri @($redirectUri) -AvailableToOtherTenants $false
        Update-AppRegistration -App $appRegAdminPage -PermissionScopeValue "AdminPage.ReadWrite.All" -Permissions @("a154be20-db9c-4678-8ab7-66f6cc099a59", "c79f8feb-a9db-4090-85f9-90d820caa0eb", "1ca167d5-1655-44a1-8adf-1414072e1ef9") -AdminConsentDisplayName "AdminPage.ReadWrite.All" -AdminConsentDescription "Allows to access and use the UnifiedContacts Admin page" -ApplicationIdUri "api://$($appRegAdminPage.AppId)" -RequestedAccessTokenVersion 1 -AddAppRole $true
    
        Write-Progress -Activity "Deployment Unified Contacts" -Status "Create Teams App registration" -PercentComplete 20
    
        $appRegTeams = New-AppRegistration -appRegistrationName ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "AppRegistrationNameTeamsApp" }).value -redirectUri @($redirectUri) -AvailableToOtherTenants $true
        Update-AppRegistration -App $appRegTeams -PermissionScopeValue "access_as_user" -Permissions @("e1fe6dd8-ba31-4d61-89e7-88639da4683d", "242b9d9e-ed24-4d09-9a52-f43769beb9d4", "9c7a330d-35b3-4aa1-963d-cb2b9f927841", "08432d1b-5911-483c-86df-7980af5cdee0", "205e70e5-aba6-4c52-a976-6d2d46c48043", "a154be20-db9c-4678-8ab7-66f6cc099a59") -AdminConsentDisplayName "access_as_user" -AdminConsentDescription "Default access" -ApplicationIdUri "api://$($appService.EnabledHostNames[0])/$($appRegTeams.AppId)" -RequestedAccessTokenVersion 2 -AddAppRole $false

        Start-Sleep -Seconds 10
        #Refresh appReg Object
        $appRegTeams = Get-AzADApplication -ObjectId $appRegTeams.Id
        Set-TeamsAsAuthorizedClientApplication -App $appRegTeams
    
        Write-Progress -Activity "Deployment Unified Contacts" -Status "Create App secrets" -PercentComplete 30

        $appSecretAdminPage = New-AppSecret -ObjectId $appRegAdminPage.Id -SecretName "main"
        $appSecretTeamsApp = New-AppSecret -ObjectId $appRegTeams.Id -SecretName "main"
        Write-Progress -Activity "Deployment Unified Contacts" -Status "Set SQL Server Firewall Rule" -PercentComplete 40

        New-AzSqlServerFirewallRule -ResourceGroupName $resourceGroup -ServerName ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "SQLServer" }).value -AllowAllAzureIPs | Out-Null

        Write-Progress -Activity "Deployment Unified Contacts" -Status "Set Key Vault User Permissions" -PercentComplete 50

        $VaultName = ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "KeyVaultName" }).value
        Set-AzKeyVaultAccessPolicy -VaultName $VaultName -UserPrincipalName $User -PermissionsToSecrets get, set, delete | Out-Null
        Write-Progress -Activity "Deployment Unified Contacts" -Status "Set Key Vault Secrets" -PercentComplete 60
        $connectionString = "Server=tcp:$(($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "SQLServer" }).value).database.windows.net,1433;Initial Catalog=$(($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "SQLDatabase" }).value);Persist Security Info=False;User ID=$($SqlCredential.UserName);password='$($SqlCredential.GetNetworkCredential().Password)';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
        $requiredKeyVaultSecrets = @{
            "AuthSettings--AdminPageClientId"        = $appRegAdminPage.AppId 
            "AuthSettings--AdminPageTenantId"        = $TenantId
            "AuthSettings--AdminPageClientSecret"    = $appSecretAdminPage
            "AuthSettings--ClientId"                 = $appRegTeams.AppId
            "AuthSettings--TenantId"                 = $TenantId
            "AuthSettings--ClientSecret"             = $appSecretTeamsApp
            "AuthSettings--DatabaseConnectionString" = $connectionString
        }
       
        foreach ($requiredKeyVaultSecretKey in $requiredKeyVaultSecrets.Keys) {
            Set-AzKeyVaultSecret -VaultName $VaultName -Name $requiredKeyVaultSecretKey -SecretValue  (ConvertTo-SecureString -String $requiredKeyVaultSecrets[$requiredKeyVaultSecretKey] -AsPlainText -Force) | Out-Null
        }
        
        Write-Progress -Activity "Deployment Unified Contacts" -Status "Copy Binaries to storage Account" -PercentComplete 75
    
        $me = Get-AzADUser -UserPrincipalName $User
        $storageAccountName = ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "StorageAccountName" }).value 
        New-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Blob Data Contributor" | Out-Null
    
        Copy-Binaries -Destination $storageAccountName
        Write-Progress -Activity "Deployment Unified Contacts" -Status "Restart Web Apps" -PercentComplete 90

    ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "WEBSITE_RUN_FROM_PACKAGE" }).value = "https://$(($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "AppSettings__BlobStorageUrl" }).value)/unified-contacts/binaries.zip"
        $settings = @{}
        foreach ($appsetting in $appService.SiteConfig.AppSettings) {
            $settings.add($appsetting.Name, $appsetting.Value)
        }
        $settings.add("AppServiceAzureUrl", $AppServiceAzureUrl)
        $concentTeams = "https://login.microsoftonline.com/$TenantId/adminconsent?client_id=$($appRegTeams.AppId)"
        $consentAdminPage = "https://login.microsoftonline.com/$TenantId/adminconsent?client_id=$($appRegAdminPage.AppId)"
        Set-AzWebApp -ResourceGroupName $resourceGroup -Name $appServiceName -AppSettings $settings | Out-Null
        Start-Sleep -Seconds 10

        Write-Host "Please grant the Admin Page app registration permission via this link:  $consentAdminPage" -ForegroundColor Cyan
        Write-Host "Please grant the Teams app registration permission via this link:  $concentTeams" -ForegroundColor Cyan

        $automateGrant = Read-Host -Prompt "Do you want to grant the permissions automatically: [y] yes [n] no" 
        if ($automateGrant.ToLower() -like "y") {
            az login 
            az ad app permission admin-consent --id $appRegTeams.Id | Out-Null
            az ad app permission admin-consent  --id $appRegAdminPage.Id | Out-Null
        }
        $role = Get-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Table Data Contributor"
        if ($null -eq $role) {
            New-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Table Data Contributor" | Out-Null
            $roleReady = $null 
            while ($null -eq $roleReady) {
                Start-Sleep -Seconds 10
                $roleReady = Get-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Table Data Contributor"
            }
        }
        $token = Get-AzAccessToken -ResourceUrl "https://storage.azure.com/" -AsSecureString
        $destContext = New-AzStorageContext -StorageAccountName $storageAccountName -SasToken $token
        $releaseInfo = Copy-Binaries -Destination $storageAccountName -Channel 'release'
        Update-Infrastructure -SubscriptionId $subscriptionId -DestContext $destContext -ResourceGroupName $resourceGroup -StorageAcccountName $storageAccountName -AppServiceName $appServiceName -SelectedVersion $releaseInfo.version
        
        Write-Host "Your Deployment was succesfull. Go to $AppServiceAzureUrl to restart your App Service." -ForegroundColor Green
    }
    catch {
        Write-Error "Your Deployment has failed. Please try to run Install-UnifiedContacts again. Error: $_" -ErrorAction 'Continue'
        #clean up all resources
        if ($appRegAdminPage.Id) {
            Remove-AzADApplication -ObjectId $appRegAdminPage.Id 
        }
        if ($appRegTeams.Id) {
            Remove-AzADApplication -ObjectId $appRegTeams.Id
        }
        if ($storageAccountName) {
            $me = Get-AzADUser -UserPrincipalName $User
            Remove-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Blob Data Contributor"
        }
        if ($appService) {
 ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "WEBSITE_RUN_FROM_PACKAGE" }).value = "https://unifiedcontacts.blob.core.windows.net/unified-contacts-releases/stable/1.4.0-beta.7/ucPublish_1.4.0-beta.7_824b8e82-0fd0-4b7e-b220-bdc90fdf4f69.zip"
            $settings = @{}
            foreach ($appsetting in $appService.SiteConfig.AppSettings) {
                $settings.add($appsetting.Name, $appsetting.Value)
            }
            Set-AzWebApp -ResourceGroupName $resourceGroup -Name $appServiceName -AppSettings $settings
       
            $fireWallRule = Get-AzSqlServerFirewallRule -ResourceGroupName $resourceGroup -ServerName ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "SQLServer" }).value -FirewallRuleName AllowAllAzureIPs
            if ($fireWallRule) { 
                Remove-AzSqlServerFirewallRule -ResourceGroupName $resourceGroup -ServerName ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "SQLServer" }).value -FirewallRuleName $fireWallRule.FirewallRuleName | Out-Null
            }
            try {
                Remove-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Blob Data Contributor"
            }
            catch {
                #fall through
            }
        }
    }
}