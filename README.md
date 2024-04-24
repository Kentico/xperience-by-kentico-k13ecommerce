# Xperience by Kentico - KX 13 E-Commerce integration

[![7-day bug-fix policy](https://img.shields.io/badge/-7--days_bug--fixing_policy-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik04ODguNDkgMjIyLjY4NnYtMzEuNTRsLTY1LjY3Mi0wLjk1NWgtMC4yMDVhNDY1LjcxNSA0NjUuNzE1IDAgMCAxLTE0NC4zMTUtMzEuMzM0Yy03Ny4wMDUtMzEuMTk4LTEyNi4yOTQtNjYuNzY1LTEyNi43MDMtNjcuMTA3bC0zOS44LTI4LjY3Mi0zOS4xODUgMjguNDY4Yy0yLjA0OCAxLjUwMS00OS45MDMgMzYuMDQ0LTEyNi45MDggNjcuMzFhNDQ3LjQyIDQ0Ny40MiAwIDAgMS0xNDQuNTIgMzEuMzM1bC02NS44NzcgMC45NTZ2Mzc4Ljg4YzAgODcuMDQgNDkuODM0IDE4NC42NjEgMTM3LjAxIDI2Ny44MSAzNy41NDcgMzUuODQgNzkuMjU4IDY2LjM1NSAxMjAuODMzIDg4LjIgNDMuMjggMjIuNzMzIDg0LjI0IDM0LjYxMiAxMTguODUyIDM0LjYxMiAzNC40MDYgMCA3NS43NzYtMTIuMTUyIDExOS42MDMtMzUuMTU4YTU0Ny45NzcgNTQ3Ljk3NyAwIDAgMCAxMjAuMDEzLTg3LjY1NCA1MTUuMjA5IDUxNS4yMDkgMCAwIDAgOTYuMTg4LTEyMi44OGMyNy4xMDItNDkuNTYyIDQwLjgyMy05OC4zMDQgNDAuODIzLTE0NC45OTlsLTAuMTM2LTM0Ny4yMDR6TTUxMC4wOSAxNDMuNDI4bDEuNzA2LTEuMzY1IDEuNzc1IDEuMzY1YzUuODAzIDQuMTY1IDU5LjUyOSA0MS44NDggMTQwLjM1NiA3NC43NTIgNzkuMTkgMzIuMDg2IDE1My42IDM1LjYzNSAxNjcuNjYzIDM2LjA0NWwyLjU5NCAwLjA2OCAwLjIwNSAzMTUuNzM0YzAuMTM3IDY5LjQ5NS00Mi41OTggMTUwLjE4Ni0xMTcuMDc3IDIyMS40NTdDNjQxLjU3IDg1NC4yODkgNTYzLjEzIDg5Ni40NzggNTEyIDg5Ni40NzhjLTIzLjY4OSAwLTU1LjU3LTkuODk5LTg5LjcwMi0yNy43ODVhNDc4LjgyMiA0NzguODIyIDAgMCAxLTEwNS42MDktNzcuMjc4QzI0Mi4yMSA3MjAuMjEzIDE5OS40NzUgNjM5LjUyMiAxOTkuNDc1IDU2OS44OVYyNTQuMjI1bDIuNzMtMC4xMzZjMy4yNzggMCA4Mi42MDQtMS41MDIgMTY3LjY2NC0zNS45NzdhNzM5Ljk0MiA3MzkuOTQyIDAgMCAwIDE0MC4yMi03NC42MTV2LTAuMDY5eiIgIC8+PHBhdGggZD0iTTcxMy4zMTggMzY4LjY0YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzI5IDBMNDQ5LjE5NSA1ODcuNDM1bC05My4xODQtOTMuMTE2YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzMgMCAzMi4yMjIgMzIuMjIyIDAgMCAwIDAgNDUuMjZsMTE1Ljg1IDExNS44NWEzMi4yOSAzMi4yOSAwIDAgMCA0NS4zMjggMEw3MTMuMzIgNDEzLjlhMzIuMjIyIDMyLjIyMiAwIDAgMCAwLTQ1LjMzeiIgIC8+PC9zdmc+)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support)  
[![CI: Build and Test](https://github.com/Kentico/repo-template/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Kentico/repo-template/actions/workflows/ci.yml)

## Description

Repository contains solution with Xperience By Kentico integration to Kentico Xperience 13 E-Commerce features
to create E-Commerce solution on XByK.
Currently there are 2 solutions:
- Kentico.Xperience.K13Ecommerce.sln
  - It shows the possibility of XbyK integration on Kentico 13 E-Commerce solution.
  - Consists of these parts:
    - Library for Kentico 13 that exposes a REST API for an E-Commerce site (Kentico.Xperience.StoreApi).
    - Library for XbyK connecting to the REST Store API running under Kentico 13 (Kentico.Xperience.K13Ecommerce)
    - Razor Class Library for selector components (Kentico.Xperience.Store.Rcl)
    - Sample Dancing Goat sites
      - DancingGoat.csproj - XbyK Dancing Goat enriched with integration to KX 13 Dancing Goat to show how to
create simple e-shop with product listing, product detail and checkout process on XByK
      - Kentico13_DancingGoat.csproj - KX 13 Dancing Goat example with configured Store API to show you how you can setup
REST Store API on you own KX 13 e-commerce solution 
- Kentico.Xperience.K13Ecommerce.Libs.sln
  - Contains only libraries without sample sites

Solution covers several scenarios according to the complexity of integration between XByK and KX 13:
- [Product listing widget example](./examples/DancingGoat-K13Ecommerce/Components/Widgets/Store/ProductListWidget)
  - Used to display products directly from the KX 13, purchase itself still takes place 
  on Kentico 13
- Full scale e-commerce solution
  - Product data (with variants and images) are synchronized to Content hub (can be turned off) 
  - Product listing, detail and checkout process is placed on XbyK (shopping cart is calculated still on KX 13)
  - Linking products to categories in Pages channels needs to be done manually from Content hub. 
 Page types are prepared to CI restore, details info in [this section of User Guide](./docs/Usage-Guide.md#store-setup).

## Screenshots

---If the integration has Administration UI or an impact on the UX/design of the live site, include some compelling screenshots here---

## Library Version Matrix

Summary of libraries which are supported by the following versions Xperince by Kentico / Kentico Xperience 13

### Kentico Xperience 13 E-Commerce integration

| Library                            | Xperience Version | Library Version |
|------------------------------------|-------------------| --------------- |
| Kentico.Xperience.Ecommerce.Common | \>= 28.2.1        | 1.0.0           |
| Kentico.Xperience.K13Ecommerce     | \>= 28.2.1        | 1.0.0           |
| Kentico.Xperience.Store.Rcl        | \>= 28.2.1        | 1.0.0           |
| Kentico.Xperience.StoreApi         | \>= 13.0.131      | 1.0.0           |

### Dependencies

#### Kentico 13 E-Commerce

Xperience by Kentico application:
- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.xperience.io/xp/changelog)

Kentico Xperience 13 application (or standalone API app):
- [ASP.NET Core 6.0](https://dotnet.microsoft.com/en-us/download)
- [Kentico Xperience 13 Refresh 11](https://docs.kentico.com/13/release-notes-xperience-13)

## Package Installation

### Kentico Xperience 13 E-Commerce integration

Add these packages to your XbyK application using the .NET CLI

```powershell
dotnet add package Kentico.Xperience.K13Ecommerce
dotnet add package Kentico.Xperience.Store.Rcl
```

Add this package to your Kentico Xperience 13 ASP.NET.Core application (live site or create standalone application for 
when your KX 13 live site is not running)

```powershell
dotnet add package Kentico.Xperience.StoreApi
```

## Quick Start

### Kentico Xperience 13 E-Commerce integration

**First setup your Kentico 13 ASP.NET Core application**:

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

**Then setup your Xperience By Kentico application**

1. Fill settings to connect your Kentico Xperience 13 instance
```json
{
  "CMSKenticoStoreConfig": {
    "StoreApiUrl": "http://dev.dancinggoat.com:65375",
    "ClientId": "3ef7fe1b-696c-4afa-8b56-d3176b7bea95",
    "ClientSecret": "********************",
    "ProductSyncEnabled": true,
    "ProductSyncInterval": 10
  }
}
```
2. Add K13Ecommerce library to the application services
```csharp
// Program.cs

// Registers Kentico Store API and services for e-commerce support
builder.Services.AddKenticoStoreServices(builder.Configuration);
```
3. For most simple scenario: copy product listing widget from Dancing Goat example project to your project and configure
properties to display products from Kentico 13. Sample widget is located [here](./examples/DancingGoat-K13Ecommerce/Components/Widgets/Store/ProductListWidget).
4. For more complex scenario with full e-shop, you can inspire how Dancing Goat sample Store on XbyK is implemented.
Check [Usage guide](./docs/Usage-Guide.md#store-setup) for detailed instructions how to configure categories, products and cart steps.
5. Start to use on your live site

## Full Instructions

View the [Usage Guide](./docs/Usage-Guide.md) for more detailed instructions.

## Contributing

To see the guidelines for Contributing to Kentico open source software, please see [Kentico's `CONTRIBUTING.md`](https://github.com/Kentico/.github/blob/main/CONTRIBUTING.md) for more information and follow the [Kentico's `CODE_OF_CONDUCT`](https://github.com/Kentico/.github/blob/main/CODE_OF_CONDUCT.md).

Instructions and technical details for contributing to **this** project can be found in [Contributing Setup](./docs/Contributing-Setup.md).

## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

[![7-day bug-fix policy](https://img.shields.io/badge/-7--days_bug--fixing_policy-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik04ODguNDkgMjIyLjY4NnYtMzEuNTRsLTY1LjY3Mi0wLjk1NWgtMC4yMDVhNDY1LjcxNSA0NjUuNzE1IDAgMCAxLTE0NC4zMTUtMzEuMzM0Yy03Ny4wMDUtMzEuMTk4LTEyNi4yOTQtNjYuNzY1LTEyNi43MDMtNjcuMTA3bC0zOS44LTI4LjY3Mi0zOS4xODUgMjguNDY4Yy0yLjA0OCAxLjUwMS00OS45MDMgMzYuMDQ0LTEyNi45MDggNjcuMzFhNDQ3LjQyIDQ0Ny40MiAwIDAgMS0xNDQuNTIgMzEuMzM1bC02NS44NzcgMC45NTZ2Mzc4Ljg4YzAgODcuMDQgNDkuODM0IDE4NC42NjEgMTM3LjAxIDI2Ny44MSAzNy41NDcgMzUuODQgNzkuMjU4IDY2LjM1NSAxMjAuODMzIDg4LjIgNDMuMjggMjIuNzMzIDg0LjI0IDM0LjYxMiAxMTguODUyIDM0LjYxMiAzNC40MDYgMCA3NS43NzYtMTIuMTUyIDExOS42MDMtMzUuMTU4YTU0Ny45NzcgNTQ3Ljk3NyAwIDAgMCAxMjAuMDEzLTg3LjY1NCA1MTUuMjA5IDUxNS4yMDkgMCAwIDAgOTYuMTg4LTEyMi44OGMyNy4xMDItNDkuNTYyIDQwLjgyMy05OC4zMDQgNDAuODIzLTE0NC45OTlsLTAuMTM2LTM0Ny4yMDR6TTUxMC4wOSAxNDMuNDI4bDEuNzA2LTEuMzY1IDEuNzc1IDEuMzY1YzUuODAzIDQuMTY1IDU5LjUyOSA0MS44NDggMTQwLjM1NiA3NC43NTIgNzkuMTkgMzIuMDg2IDE1My42IDM1LjYzNSAxNjcuNjYzIDM2LjA0NWwyLjU5NCAwLjA2OCAwLjIwNSAzMTUuNzM0YzAuMTM3IDY5LjQ5NS00Mi41OTggMTUwLjE4Ni0xMTcuMDc3IDIyMS40NTdDNjQxLjU3IDg1NC4yODkgNTYzLjEzIDg5Ni40NzggNTEyIDg5Ni40NzhjLTIzLjY4OSAwLTU1LjU3LTkuODk5LTg5LjcwMi0yNy43ODVhNDc4LjgyMiA0NzguODIyIDAgMCAxLTEwNS42MDktNzcuMjc4QzI0Mi4yMSA3MjAuMjEzIDE5OS40NzUgNjM5LjUyMiAxOTkuNDc1IDU2OS44OVYyNTQuMjI1bDIuNzMtMC4xMzZjMy4yNzggMCA4Mi42MDQtMS41MDIgMTY3LjY2NC0zNS45NzdhNzM5Ljk0MiA3MzkuOTQyIDAgMCAwIDE0MC4yMi03NC42MTV2LTAuMDY5eiIgIC8+PHBhdGggZD0iTTcxMy4zMTggMzY4LjY0YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzI5IDBMNDQ5LjE5NSA1ODcuNDM1bC05My4xODQtOTMuMTE2YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzMgMCAzMi4yMjIgMzIuMjIyIDAgMCAwIDAgNDUuMjZsMTE1Ljg1IDExNS44NWEzMi4yOSAzMi4yOSAwIDAgMCA0NS4zMjggMEw3MTMuMzIgNDEzLjlhMzIuMjIyIDMyLjIyMiAwIDAgMCAwLTQ1LjMzeiIgIC8+PC9zdmc+)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support)


This project has **Full support by 7-day bug-fix policy**

See [`SUPPORT.md`](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support) for more information.

For any security issues see [`SECURITY.md`](https://github.com/Kentico/.github/blob/main/SECURITY.md).
