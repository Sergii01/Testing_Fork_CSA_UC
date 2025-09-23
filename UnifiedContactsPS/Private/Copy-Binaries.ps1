function Copy-Binaries {
    param(
        $Destination,
        [ValidateSet('release', 'prerelease')]
        [string]$Channel = 'release'
    )
    $ErrorActionPreference = "Stop"

    # GitHub Repository Details
    $repo = $Script:repoUrl
    $assetName = "binaries.zip"
    
    try {
        # Get the access token with the new secure string parameter
        $token = Get-AzAccessToken -ResourceUrl "https://storage.azure.com/" -AsSecureString
        
        # Setup Azure Storage context with the token
        $destContext = New-AzStorageContext -StorageAccountName $Destination -SasToken $token
        
        try { 
            New-AzStorageContainer -Name $Script:destinationContainer -Context $destContext -Permission Blob 
        }
        catch { 
            # Container might already exist, continue
        }

        # Get GitHub releases
        $releases = Invoke-RestMethod -Uri "https://api.github.com/repos/$repo/releases"
        
        # Select release based on channel
        $selectedRelease = if ($Channel -eq 'release') {
            $releases | Where-Object { -not $_.prerelease } | Select-Object -First 1
        }
        else {
            $releases  | Where-Object { $_.prerelease } | Select-Object -First 1
        }
        
        if (-not $selectedRelease) {
            throw "No $Channel found on GitHub"
        }
        
        # Find the binaries.zip asset
        $asset = $selectedRelease.assets | Where-Object { $_.name -eq $assetName }
        if (-not $asset) {
            throw "Asset '$assetName' not found in the $Channel"
        }

        Write-Host "Copying $assetName from $Channel $($selectedRelease.tag_name) to Azure Storage..."
        
        # Start the copy operation directly from GitHub to Azure Blob Storage
        Start-AzStorageBlobCopy -AbsoluteUri $asset.browser_download_url `
            -DestContainer $Script:destinationContainer `
            -DestBlob $Script:destinationBlob `
            -DestContext $destContext `
            -Force | Out-Null

        Write-Host "Successfully initiated copy to Azure Storage"
        
        # Return release information for further processing
        return @{
            version     = $selectedRelease.tag_name
            downloadUrl = $asset.browser_download_url
            releaseType = $Channel
        }
    }
    catch {
        Write-Error "Failed to copy binaries: $_"
        throw
    }
}
