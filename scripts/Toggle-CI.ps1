param (
    [switch]$CIEnabled
)

# Use a conditional expression to set KeyValue based on the boolean $Argument
$keyValue = if ($CIEnabled) { 'True' } else { 'False' }


# Check the environment variable
if ($Env:ASPNETCORE_ENVIRONMENT -eq "CI") {
    sqlcmd `
        -S localhost `
        -d XByK_DancingGoat_K13Ecommerce `
        -U "sa" `
        -P "Pass@12345" `
        -Q "UPDATE CMS_SettingsKey SET KeyValue='$keyValue' WHERE KeyName='CMSEnableCI'"
} else {
    sqlcmd `
        -S localhost\SQLEXPRESS `
        -d XByK_DancingGoat_K13Ecommerce `
        -E `
        -Q "UPDATE CMS_SettingsKey SET KeyValue='$keyValue' WHERE KeyName='CMSEnableCI'"
}

Write-Host "CI restore for Enabled=$keyValue" -ForegroundColor Green
