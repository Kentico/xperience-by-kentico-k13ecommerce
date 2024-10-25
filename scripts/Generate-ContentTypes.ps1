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
    --kxp-codegen `
    --type ReusableContentTypes `
    --namespace K13Store `
    --location "../../src/Kentico.Xperience.K13Ecommerce/{type}/{dataClassNamespace}/{name}/" `
    --skip-confirmation `
    --include "K13Store.*"

Write-Host ""
Write-Host "Successfully generated Content types files" -ForegroundColor Green
Write-Host ""