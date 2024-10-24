param (
    [switch]$CIEnabled
)

# Use a conditional expression to set KeyValue based on the boolean $Argument
$keyValue = if ($CIEnabled) { 'True' } else { 'False' }

sqlcmd `
    -S localhost\SQLEXPRESS `
    -d XByK_DancingGoat_K13Ecommerce `
    -E `
    -Q "UPDATE CMS_SettingsKey SET KeyValue='$keyValue' WHERE KeyName='CMSEnableCI'"