param (
    [switch]$CIEnabled
)

Import-Module (Resolve-Path Utilities) `
    -Function `
    Invoke-ExpressionWithException, `
    Write-Status `
    -Force

# Use a conditional expression to set KeyValue based on the boolean $Argument
$keyValue = if ($CIEnabled) { 'True' } else { 'False' }


# Check the environment variable
if ($Env:ASPNETCORE_ENVIRONMENT -eq "CI") {
    $command = " sqlcmd " + `
        "-S localhost " + `
        "-d XByK_DancingGoat_K13Ecommerce " + `
        "-U sa " + `
        "-P Pass@12345 " + `
        "-Q ""UPDATE CMS_SettingsKey SET KeyValue='$keyValue' WHERE KeyName='CMSEnableCI'"" "
} else {
    $command = "sqlcmd " + `
        "-S localhost\SQLEXPRESS " + `
        "-d XByK_DancingGoat_K13Ecommerce " + `
        "-E " + `
        "-Q ""UPDATE CMS_SettingsKey SET KeyValue='$keyValue' WHERE KeyName='CMSEnableCI'"" "
}

Invoke-ExpressionWithException $command

Write-Host "`n"
Write-Status "CI restore for Enabled=$keyValue"
Write-Host "`n"