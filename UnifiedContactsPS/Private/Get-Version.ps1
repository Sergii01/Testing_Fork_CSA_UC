function Get-Version {
    param(
        $AppService,
        [string] $ErrorAction = "Stop"
    )
    $checkApiEndpoint = "https://$($appService.hostnames[0])/v1.3.0/api/general/version"
    $headers = @{
        "Content-Type" = "application/json"
    }
    $response = Invoke-WebRequest -Uri $checkApiEndpoint -Method Get -Headers $headers -ErrorAction $ErrorAction
    return $response
}
