function Get-LatestModuleVersion {

    $ModuleName = "UnifiedContactsPS"
    $installedModule = Get-InstalledModule -Name $ModuleName -ErrorAction SilentlyContinue
    if ($installedModule) {
        $latestModule = Find-Module -Name $ModuleName

        if ($latestModule) {
            $installedVersion = $installedModule.Version
            $latestVersion = $latestModule.Version

            if ($installedVersion -lt $latestVersion) {
                Write-Warning "You have version $installedVersion of $ModuleName installed, but the latest version is $latestVersion."
                exit 1;
            }
        }
    }

    $ModuleName = "AzTable"
    if ($null -eq (Get-InstalledModule -Name $ModuleName -ErrorAction SilentlyContinue)) {
        Write-Host "Installing required module $($ModuleName)" -ForegroundColor Green
        Install-Module AzTable
        if ($null -eq (Get-InstalledModule -Name $ModuleName -ErrorAction SilentlyContinue)) {
            Write-Error "Failed to install module $($ModuleName)"
            exit 1;
        }
    }
}


