if ($Env:ASPNETCORE_ENVIRONMENT -eq "CI") {
    $launchProfile = "K13Ecommerce.WebCI"
    $configuration = "Release"
} else {
    $launchProfile = "DancingGoat"
    $configuration = "Debug"
}

dotnet run `
    --launch-profile $launchProfile `
    -c $configuration `
    --no-build `
    --no-restore `
    --kxp-ci-restore

Write-Host ""
Write-Host 'CI files processed'
Write-Host ""