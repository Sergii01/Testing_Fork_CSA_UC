function New-AppRegistration {
    param(
        $RedirectUri,
        $AppRegistrationName,
        $AvailableToOtherTenants
    )
    $ErrorActionPreference = "Stop"

    if ($redirectUri.length -gt 0) {
        $app = New-AzADApplication -DisplayName $AppRegistrationName -SPARedirectUri $redirectUri -AvailableToOtherTenants $AvailableToOtherTenants 
    }
    else {
        $app = New-AzADApplication -DisplayName $AppRegistrationName -AvailableToOtherTenants $AvailableToOtherTenants 
    }
    return $app
}

