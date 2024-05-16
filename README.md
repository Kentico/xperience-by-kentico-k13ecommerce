# Xperience by Kentico - KX 13 E-Commerce integration

[![Kentico Labs](https://img.shields.io/badge/Kentico_Labs-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik05NTYuMjg4IDgwNC40OEw2NDAgMjc3LjQ0VjY0aDMyYzE3LjYgMCAzMi0xNC40IDMyLTMycy0xNC40LTMyLTMyLTMyaC0zMjBjLTE3LjYgMC0zMiAxNC40LTMyIDMyczE0LjQgMzIgMzIgMzJIMzg0djIxMy40NEw2Ny43MTIgODA0LjQ4Qy00LjczNiA5MjUuMTg0IDUxLjIgMTAyNCAxOTIgMTAyNGg2NDBjMTQwLjggMCAxOTYuNzM2LTk4Ljc1MiAxMjQuMjg4LTIxOS41MnpNMjQxLjAyNCA2NDBMNDQ4IDI5NS4wNFY2NGgxMjh2MjMxLjA0TDc4Mi45NzYgNjQwSDI0MS4wMjR6IiAgLz48L3N2Zz4=)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#labs-limited-support)
[![CI: Build and Test](https://github.com/Kentico/xperience-by-kentico-ecommerce/actions/workflows/ci.yml/badge.svg)](https://github.com/Kentico/xperience-by-kentico-ecommerce/actions/workflows/ci.yml)

| Name | Package |
| ------------- |:-------------:|
| Kentico.Xperience.Ecommerce.Common | [![NuGet Package](https://img.shields.io/nuget/v/Kentico.Xperience.Ecommerce.Common.svg)](https://www.nuget.org/packages/Kentico.Xperience.Ecommerce.Common) |
| Kentico.Xperience.K13Ecommerce | [![NuGet Package](https://img.shields.io/nuget/v/Kentico.Xperience.K13Ecommerce.svg)](https://www.nuget.org/packages/Kentico.Xperience.K13Ecommerce) |
| Kentico.Xperience.Store.Rcl | [![NuGet Package](https://img.shields.io/nuget/v/Kentico.Xperience.Store.Rcl.svg)](https://www.nuget.org/packages/Kentico.Xperience.Store.Rcl) |
| Kentico.Xperience.StoreApi  | [![NuGet Package](https://img.shields.io/nuget/v/Kentico.Xperience.StoreApi.svg)](https://www.nuget.org/packages/Kentico.Xperience.StoreApi) |

## Description

This repository contains solution with Xperience By Kentico (XbyK) integration to Kentico Xperience 13 (KX 13) E-Commerce features
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
      - Kentico13_DancingGoat.csproj - KX 13 Dancing Goat example with configured Store API to demonstrate how you can set up
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
 Page types are prepared to CI restore, details info in [this section of User Guide](./docs/Usage-Guide.md#dancing-goat-example---setup).

## Screenshots

![Cart content](./images/screenshots/cart_content.png "Cart content")
![Products in content hub](./images/screenshots/products_content_hub.png "Products in content hub")

## Library Version Matrix

Summary of libraries which are supported by the following versions Xperince by Kentico / Kentico Xperience 13

### Kentico Xperience 13 E-Commerce integration

| Library                            | Xperience Version | Library Version |
|------------------------------------|-------------------| --------------- |
| Kentico.Xperience.Ecommerce.Common | \>= 29.0.1        | 1.0.0           |
| Kentico.Xperience.K13Ecommerce     | \>= 29.0.1        | 1.0.0           |
| Kentico.Xperience.Store.Rcl        | \>= 29.0.1        | 1.0.0           |
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

Add this package to your Kentico Xperience 13 ASP.NET.Core application (live site or create standalone application 
when your KX 13 live site is not running)

```powershell
dotnet add package Kentico.Xperience.StoreApi
```

## Quick Start

### Kentico Xperience 13 E-Commerce integration

**First set up your Kentico 13 ASP.NET Core application**:

1. Set up your own settings for Store REST API authentication (based on JWT and OAuth client credentials flow)
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

**Then set up your Xperience By Kentico application**

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
5. Restore CI repository files to database (reusable content types, custom activities). CI files are located in
`.\examples\DancingGoat-K13Ecommerce\App_Data\CIRepository\` and you need to copy these files to your application.
```powershell
dotnet run --kxp-ci-restore
```
6. Start to use on your live site

## Full Instructions

View the [Usage Guide](./docs/Usage-Guide.md) for more detailed instructions.

## Contributing

To see the guidelines for Contributing to Kentico open source software, please see [Kentico's `CONTRIBUTING.md`](https://github.com/Kentico/.github/blob/main/CONTRIBUTING.md) for more information and follow the [Kentico's `CODE_OF_CONDUCT`](https://github.com/Kentico/.github/blob/main/CODE_OF_CONDUCT.md).

Instructions and technical details for contributing to **this** project can be found in [Contributing Setup](./docs/Contributing-Setup.md).

## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

This contribution has __Kentico Labs limited support__.

See [`SUPPORT.md`](https://github.com/Kentico/.github/blob/main/SUPPORT.md#labs-limited-support) for more information.

For any security issues see [`SECURITY.md`](https://github.com/Kentico/.github/blob/main/SECURITY.md).
