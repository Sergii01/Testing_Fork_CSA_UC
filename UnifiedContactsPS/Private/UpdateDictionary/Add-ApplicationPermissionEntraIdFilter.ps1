function Add-ApplicationPermissionEntraIdFilter {
    param(
        [Parameter(Mandatory = $true)]
        [string]$AppServiceName,
        [Parameter(Mandatory = $true)]
        [string]$ResourceGroupName
    )
    $ErrorActionPreference = "Stop"
    $appService = Get-AzWebApp -ResourceGroupName  $ResourceGroupName -Name $AppServiceName
    $app = Get-AzADApplication -ObjectId ($appService.SiteConfig.AppSettings | Where-Object { $_.Name -eq "AppRegistrationTeamsAppId" }).value
    if ($null -eq $app) {
        throw "Could not find Teams App Registration" 
        exit 1 
    }
    $TenantId = (Get-AzContext).Tenant.Id
    $permissions = Get-AzAdAppPermission -ObjectId $App.Id
    if (-not ($permissions.Id -contains "df021288-bdef-4463-88db-98f22de89214")) {
        Add-AzADAppPermission -ObjectId $App.Id -ApiId "00000003-0000-0000-c000-000000000000" -PermissionId "df021288-bdef-4463-88db-98f22de89214" -Type Role | Out-Null
    }
    $consentTeams = "https://login.microsoftonline.com/$TenantId/adminconsent?client_id=$($App.AppId)"
    Write-Host "Please grant the Teams app registration permission via this link:  $consentTeams" -ForegroundColor Cyan
}
    