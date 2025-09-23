function Compare-SemVer {
    param (
        [string]$Version1,
        [string]$Version2
    )

    $v1 = [version]$Version1
    $v2 = [version]$Version2

    if ($v1 -ge $v2) {
        return $true
    }
    elseif ($v1 -lt $v2) {
        return $false
    } 
}