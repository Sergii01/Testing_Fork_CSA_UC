
function New-KeyVaultSecret {
    param(
        $SecretValue,
        $SecretName,
        $VaultName
    )
   
    $ErrorActionPreference = "Stop"
    $SecretValue = ConvertTo-SecureString $SecretValue -AsPlainText -Force
    $secret = Set-AzKeyVaultSecret -VaultName $VaultName  -Name $SecretName -SecretValue $SecretValue
    return $secret
}
