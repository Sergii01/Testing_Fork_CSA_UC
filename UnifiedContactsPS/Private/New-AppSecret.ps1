function New-AppSecret {
    param(
        $ObjectId,
        $SecretName 
    )
    $ErrorActionPreference = "Stop"
    $startDate = Get-Date
    $endDate = $startDate.AddYears(10)
    $secret = New-AzADAppCredential -ObjectId $ObjectId -CustomKeyIdentifier $SecretName -StartDate $startDate -EndDate $endDate
    return $secret.SecretText
}