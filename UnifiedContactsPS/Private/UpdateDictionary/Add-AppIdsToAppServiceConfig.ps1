function Add-AppIdsToAppServiceConfig {
    param(
        [Parameter(Mandatory = $true)]
        [string]$AppServiceName,
        [Parameter(Mandatory = $true)]
        [string]$ResourceGroupName
    )
    $ErrorActionPreference = "Stop"
    $appService = Get-AzWebApp -ResourceGroupName  $ResourceGroupName -Name $AppServiceName
    $adminApp = Get-AzADApplication -DisplayName ($appService.SiteConfig.AppSettings | Where-Object { $_.Name -eq "AppRegistrationNameAdminPage" }).value
    $teamsApp = Get-AzADApplication -DisplayName ($appService.SiteConfig.AppSettings | Where-Object { $_.Name -eq "AppRegistrationNameTeamsApp" }).value
   
    if ($null -eq $adminApp -or $null -eq $teamsApp) { 
        $errorMessage = ""
        if ($null -eq $adminApp) { 
            $errorMessage += "Could not find Admin App Registration`n"
        }
        if ($null -eq $teamsApp) { 
            $errorMessage += "Could not find Teams App Registration`n"
        }
        throw $errorMessage
        exit 1 
    }
   
    $settings = @{}
    foreach ($appsetting in $appService.SiteConfig.AppSettings) {
        $settings.add($appsetting.Name, $appsetting.Value)
    }
    if ( $null -eq ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "AppRegistrationAdminPageId" }).value) {
        $settings.add("AppRegistrationAdminPageId", $adminApp.Id)
    }
    if ($null -eq ($appService.SiteConfig.AppSettings | where-object { $_.Name -eq "AppRegistrationTeamsAppId" }).value) {
        $settings.add("AppRegistrationTeamsAppId", $teamsApp.Id)
    } 
    Set-AzWebApp -ResourceGroupName $ResourceGroupName -Name $AppServiceName -AppSettings $settings | Out-Null
}