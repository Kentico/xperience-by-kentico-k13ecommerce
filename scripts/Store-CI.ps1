<#
.Synopsis
    Stores all objects from the local database to the CI repository
#>

Import-Module (Resolve-Path Settings) `
    -Function `
    Get-AppSettings `
    -Force

Import-Module (Resolve-Path Utilities) `
    -Function `
    Invoke-ExpressionWithException, `
    Write-Status, `
    Write-Error `
    -Force

try {
    $appSettings = Get-AppSettings

    $command = "dotnet run " + `
        "--launch-profile $($appSettings.LaunchProfile) " + `
        "-c $($appSettings.Configuration) " + `
        "--no-build " + `
        "--no-restore " + `
        "--project $($appSettings.XbKProjectPath) " + `
        "--kxp-ci-store"

    Invoke-ExpressionWithException $command

    Write-Host "`n"
    Write-Status "CI files stored"
    Write-Host "`n"
}
catch {
    Write-Error "CI store error: $($_.Exception.Message)"
    throw
}