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
    -- `
    --kxp-update `
    --skip-confirmation

Write-Host ""
Write-Host "Updated DB to latest hotfix" -ForegroundColor Green
Write-Host ""