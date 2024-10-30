Import-Module (Resolve-Path Utilities) `
    -Function `
    Get-WebProjectPath, `
    Invoke-ExpressionWithException, `
    Write-Status `
    -Force

$projectPath = Get-WebProjectPath
$repositoryPath = Join-Path $projectPath "App_Data/CIRepository"
if ($Env:ASPNETCORE_ENVIRONMENT -eq "CI") {
    $launchProfile = "K13Ecommerce.WebCI"
    $configuration = "Release"
} else {
    $launchProfile = "DancingGoat"
    $configuration = "Debug"
}

$command = "dotnet run " + `
    "--launch-profile $launchProfile " + `
    "-c $configuration " + `
    "--no-build " + `
    "--no-restore " + `
    "--project $projectPath " + `
    "--kxp-update " + `
    "--skip-confirmation"

Invoke-ExpressionWithException $command

Write-Host "`n"
Write-Status "Updated DB to latest hotfix"
Write-Host "`n"
