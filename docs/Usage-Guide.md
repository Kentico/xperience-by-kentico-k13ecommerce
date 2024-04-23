# Usage Guide

## Table of contents
1. [Screenshots](#screenshots)
2. [Store API (Kentico Xperience 13)](#forms-data---leads-integration)
3. [K13 Ecommerce integration (Xperience by Kentico)](#)
4. [Product listing widget](#product-listing-widget)
5. [Dancing Goat example - setup](#dancing-goat-example-setup)

## Screenshots

-- screenshots here --

## Store API (Kentico Xperience 13)

Store API is REST API which exposes KX 13 E-Commerce features to consume then from another sources 
(primary intended for Xperience By Kentico, but you are completetly free to use it any way you want).

API is exposed via Swagger (Open API 3 standard) on relative path `/swagger/storeapi/swagger.json`

We recommend to use this API in combination with `Kentico.Xperience.K13Ecommerce` library for XByK applications,
because there are services to simplify e-commerce integration (like `IShoppingService`) and NSwag API client is
already generated there.

### Authentication

API is intended to use with OAuth 2.0 client credentials flow, when ClientId and ClientSecret is shared between
client application (XByK) and KX 13 application. Access tokens are generated in JWT standard (from endpoint `/api/store/auth/token`).
Token request can contain `username` parameter to identify for which user token is generated.
This user name is embedded in token as `sub` and `name` claims.


### Setup

**How to setup your Kentico 13 ASP.NET Core application**:

1. Setup your own settings for Store REST API authentication (based on JWT and OAuth client credentials flow)
```json
{
  "CMSStoreApi": {
    "Jwt": {
      "Key": "YourSecretKeyForAuthenticationOfApplicationMustBeAtLeast64CharsLong",
      "Issuer": "yourCompanyIssuer.com",
      "Audience": "XbyK-DancingGoat",
      "TokenExpiresIn": 60
    },
    "ClientId": "3ef7fe1b-696c-4afa-8b56-d3176b7bea95",
    "ClientSecret": "********************"
  }
}
```
**Setting description**

| Setting                        | Description                                                                   |
|--------------------------------|-------------------------------------------------------------------------------|
| CMSStoreApi:Jwt:Key            | Your unique secret key for signing JWT access tokens (at least 64 chars long) |
| CMSStoreApi:Jwt:Issuer         | Fill arbitrary value for this claim (like your domain)                        |
| CMSStoreApi:Jwt:Audience       | Fill arbitrary value for this claim to identify recipients                    |
| CMSStoreApi:Jwt:TokenExpiresIn | Duration in minutes for token validity                                        |
| ClientId                       | Fill your value, used for getting token (client credentials OAuth 2.0 flow)   |
| ClientSecret                   | Fill your value, used for getting token (client credentials OAuth 2.0 flow)   |   



2. Add Store API services to application services and configure Swagger
```csharp
// Startup.cs

public void ConfigureServices(IServiceCollection services)
{
    // ...
    //Store API registration
    services.AddKenticoStoreApi();
    //Registers Swagger generation
    services.AddKenticoStoreApiSwagger();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
{
    //Registers Swagger endpoint middleware and swagger UI
    app.UseStoreApiSwagger();
}
```

### Library matrix

| Library                            | Xperience Version | Library Version | NET version |
|------------------------------------|-------------------| --------------- |-------------|
| Kentico.Xperience.StoreApi         | \>= 13.0.131      | 1.0.0           | \>= .NET 6  |

