function Update-UnifiedContacts {
    [CmdletBinding()]
    Param(
        [Parameter (Mandatory = $true, HelpMessage = "The Url you can copy from your browser when you open the App Service. Format: https://portal.azure.com/#<domain>/resource/subscriptions/<subscriptionId>/resourceGroups/<ressourceGroupName>/providers/Microsoft.Web/sites/<appServiceName>/appServices")][string]$AppServiceAzureUrl,
        [Parameter (Mandatory = $false, HelpMessage = "Choose your release channel.")][string]$ReleaseChannel = $null
    )
    $ErrorActionPreference = "Stop"
    #$Script:AppServiceAzureUrl = $AppServiceAzureUrl;
    $resourceGroup = (($AppServiceAzureUrl -Split "resourceGroups/")[1] -Split "/")[0]
    $subscriptionId = (($AppServiceAzureUrl -Split "subscriptions/")[1] -Split "/")[0]
    $appServiceName = (($AppServiceAzureUrl -Split "sites/")[1] -Split "/")[0]


    Write-Host "ResourceGroup: $resourceGroup`nSubscriptionId: $subscriptionId`nAppServiceName: $appServiceName"
    Import-Module Az.Resources
    try {
        Get-LatestModuleVersion
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
        az account set --subscription $subscriptionId | Out-Null
        $response = Invoke-WebRequest -Uri $Script:versionManifesUri -UseBasicParsing
        if ($response.StatusCode -lt 200 -or $response.StatusCode -ge 300) {
            throw "Could not accquire versionManifest"
        }
        Write-Progress -Activity "Update Unified Contacts" -Status "Get Web App Information" -PercentComplete 5
        $versionManifest = $response.Content | ConvertFrom-Json
        $appService = Get-AzWebApp -ResourceGroupName  $resourceGroup -Name $appServiceName
        if ($null -eq ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq $Script:AppServiceAzureUrlPropertyName })) {
            $settings = @{}
            foreach ($appsetting in $appService.SiteConfig.AppSettings) {
                $settings.add($appsetting.Name, $appsetting.Value)
            }
            $settings.add($Script:appServiceAzureUrlPropertyName, $AppServiceAzureUrl)
            Set-AzWebApp -ResourceGroupName $resourceGroup -Name $appServiceName -AppSettings $settings
        }
        
        $storageAccountName = ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq $Script:storageAccountPropertyName }).value 
        $token = Get-AzAccessToken -ResourceUrl "https://storage.azure.com/" -AsSecureString
        $destContext = New-AzStorageContext -StorageAccountName $storageAccountName -SasToken $token
        $me = Get-AzADUser -UserPrincipalName (az account show --query user.name --output tsv)
        
        Write-Progress -Activity "Update Unified Contacts" -Status "Check necessary permissions" -PercentComplete 15
        $role = Get-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Blob Data Contributor"
        if ($null -eq $role) {
            New-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Blob Data Contributor" | Out-Null
            $roleReady = $null 
            while ($null -eq $roleReady) {
                Start-Sleep -Seconds 10
                $roleReady = Get-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Blob Data Contributor"
            }
        }

        Write-Progress -Activity "Update Unified Contacts" -Status "Get selected release channel" -PercentComplete 25

        # Get releases from GitHub
        $repoReleases = Invoke-RestMethod -Uri "https://github.com/glueckkanja/Unified-Contacts/releases"
        
        # Default to latest stable release if no channel specified
        if ($null -eq $releaseChannel) {
            $title = "From which release channel do you want to update?"
            $prompt = "Enter your choice"
            $default = 0
            $choices = [System.Management.Automation.Host.ChoiceDescription[]] @(
                [System.Management.Automation.Host.ChoiceDescription]"&Release"
                [System.Management.Automation.Host.ChoiceDescription]"&Prerelease"
            )
            $releaseChannel = $host.UI.PromptForChoice($title, $prompt, $choices, $default)
        }
        
        # Select the appropriate release based on channel
        if ($releaseChannel -eq 0) {
            # Latest stable release
            $selectedRelease = $repoReleases | Where-Object { -not $_.prerelease } | Select-Object -First 1
        } else {
            # Latest prerelease
            $selectedRelease = $repoReleases | Select-Object -First 1
        }
        
        $selectedChannel = @{
            name = if ($releaseChannel -eq 0) { "release" } else { "prerelease" }
            latestVersion = $selectedRelease.tag_name
            latestVersionRef = ($selectedRelease.assets | Where-Object { $_.name -eq "binaries.zip" }).browser_download_url
        }

        Write-Progress -Activity "Update Unified Contacts" -Status "Update Infrastructure" -PercentComplete 40
        Update-Infrastructure -SubscriptionId  $subscriptionId -destContext $destContext -resourceGroupName $resourceGroup -storageAcccountName $storageAccountName -AppServiceName $appServiceName -selectedVersion $selectedChannel.latestVersion

        $currentVersion = ((Get-Version -AppService $appService).Content | ConvertFrom-Json).version
        if ($currentVersion -eq $selectedChannel.latestVersion) {
            Write-Host "The latest version is already deployed." -ForegroundColor Yellow
            return; 
        }
        Write-Progress -Activity "Update Unified Contacts" -Status "Copy Binaries" -PercentComplete 65
        Start-AzStorageBlobCopy -AbsoluteUri $selectedChannel.latestVersionRef -DestContainer "unified-contacts" -DestBlob "binaries.zip" -DestContext $destContext -Force | Out-Null
        $role = Get-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Table Data Contributor"
        if ($null -eq $role) {
            New-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Table Data Contributor" | Out-Null
            $roleReady = $null 
            while ($null -eq $roleReady) {
                Start-Sleep -Seconds 10  #wait for roleassignment to be present in Entra Id
                $roleReady = Get-AzRoleAssignment -ObjectId $me.Id -Scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName/" -RoleDefinitionName "Storage Table Data Contributor"
            }
        }

        Write-Progress -Activity "Update Unified Contacts" -Status "Restart Web App" -PercentComplete 75
        Restart-AzWebApp -ResourceGroupName $resourceGroup -Name $appServiceName | Out-Null
        Start-Sleep -Seconds 20
        $timeout = (Get-Date).AddMinutes(5)
        $responseReady = $null 
        $timeout = (Get-Date).AddMinutes(10)
        $restartedWebApp = $false
        while ((Get-Date) -lt $timeout) {
            Start-Sleep -Seconds 5 
            if ((Get-AzWebApp -Name $appServiceName -ResourceGroupName $resourceGroup).State -eq "Running") {
                # AppService has successfully restarted if we get a 2XX returned with the current version
                $responseReady = $null
                try {
                    $responseReady = Get-Version -AppService $appService -ErrorAction 'SilentlyContinue'
                }
                catch [Microsoft.PowerShell.Commands.HttpResponseException] {
                    $responseReady = $_.Exception.Response
                }
                catch {
                    Write-Warning "No Http-Error was thrown: $_"
                }
                if ($null -ne $responseReady -and $responseReady.StatusCode -ge 200 -and $responseReady.StatusCode -le 299 -and $null -ne $responseReady.Content -and ($responseReady.Content | ConvertFrom-Json).version -eq $selectedChannel.latestVersion) {
                    Write-Host "AppService successfully restarted" -ForegroundColor Green
                    $restartedWebApp = $true
                    break;
                }
            }
        }
        if ($restartedWebApp) { 
            Write-Host "Go to https://$($appService.hostnames[0]) to upload the Manifest."  -ForegroundColor Yellow
            Write-Host "Please refresh the Unified Contacts Admin Portal using Shift+F5 to refresh the cache."  -ForegroundColor Yellow
        }
        else {
            Write-Error "Restarting of App Service failed. Please try to restart the App Service manually ($appServiceAzureUrl) or try to update Unified Contacts again."
        }
    }
    catch {
        Write-Error "Something went wrong please. Try again later. Error: $_" -ErrorAction 'Continue'
    }
}


