function Add-PreAuthorizedApplicationForStandAloneApp {
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

    $scope = $app.Api.Oauth2PermissionScope[0]
    $api = $app.Api
   
    # Microsoft 365 web	4765445b-32c6-49b0-83e6-1d93765276ca
    # Microsoft 365 desktop	0ec893e0-5785-4de6-99da-4ed124e5296c
    # Microsoft 365 mobile	d3590ed6-52b3-4102-aeff-aad2292ab01c
    # Outlook desktop	d3590ed6-52b3-4102-aeff-aad2292ab01c
    # Outlook web	bc59ab01-8403-45c6-8796-ac3ef710b3e3
    # Outlook mobile	27922004-5251-4030-b22d-91ecd9a37ea4

    foreach ($clientId in @("4765445b-32c6-49b0-83e6-1d93765276ca", "0ec893e0-5785-4de6-99da-4ed124e5296c", "d3590ed6-52b3-4102-aeff-aad2292ab01c", "bc59ab01-8403-45c6-8796-ac3ef710b3e3", "27922004-5251-4030-b22d-91ecd9a37ea4")) {
        if ($api.PreAuthorizedApplication.AppId -notcontains $clientId) {
            $PreAuthorizedApplicationObject = New-Object Microsoft.Azure.Powershell.Cmdlets.Resources.MSGraph.Models.ApiV10.MicrosoftGraphPreAuthorizedApplication

            $PreAuthorizedApplicationObject.AppId = $clientId
            $PreAuthorizedApplicationObject.DelegatedPermissionId = $scope.Id
            $api.PreAuthorizedApplication += $PreAuthorizedApplicationObject
        }
    }

    Update-AzADApplication -ObjectId $App.Id -Api $api 
}