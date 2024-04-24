# Usage Guide

## Table of contents
1. [Screenshots](#screenshots)
2. [Store API (Kentico Xperience 13)](#store-api-kentico-xperience-13)
3. [K13 Ecommerce integration (Xperience by Kentico)](#)
4. [Product listing widget](#product-listing-widget)
5. [Dancing Goat example - setup](#dancing-goat-example-setup)

## Screenshots

-- screenshots here --

## Store API (Kentico Xperience 13)

Store API (library `Kentico.Xperience.StoreApi`) is REST API which exposes KX 13 E-Commerce features to consume then from another sources 
(primary intended for Xperience By Kentico, but you are completely free to use it any way you want).

API is exposed via Swagger (Open API 3 standard) on relative path `/swagger/storeapi/swagger.json`

We recommend to use this API in combination with `Kentico.Xperience.K13Ecommerce` library for XByK applications,
because there are services to simplify e-commerce integration (like `IShoppingService`) and NSwag API client is
already generated there.

### Authentication

API is intended to use with OAuth 2.0 client credentials flow, when ClientId and ClientSecret is shared between
client application (XByK) and KX 13 application. Access tokens are generated in JWT standard (from endpoint `/api/store/auth/token`).
Token request can contain `username` parameter to identify for which user token is generated.
This user name is validated that exists and then embedded in token as `sub` and `name` claims. All subsequent 
requests needs to be sent with Bearer token in Authorization header.

All API controllers are secured by custom authorization attribute and filter `AuthorizeStore`. This filter checks
user claim and when this user exists and is enabled, then is assigned to `MembershipContext.AuthenticatedUser`. When
specific user name isn't provided, AuthenticatedUser remains as public user.

### Products

These endpoints have prefix `/api/store/products` and cover these domains:
- Getting product pages based on parameters (returned data can be [customized](#todo))
- Getting all product categories for given culture
- Getting prices and inventory info

### Shopping cart

These endpoints have prefix `api/store/cart` and cover work with current shopping cart. Many actions correspond
to functionality in KX 13 `CMS.Ecommerce.IShoppingService` (adding/removing items to cart, set delivery data, creating order etc.).
All endpoints use `ShoppingCartGUID` parameter sent 
in HTTP header to identify current shopping cart. Client application needs to manage this identifier (this already covers
`Kentico.Xperience.K13Ecommerce` library for XByK applications). 

All calls internally use IShoppingService with some
noticeable customizations to handle [retrieving cart](https://docs.kentico.com/13/e-commerce-features/customizing-on-line-stores/shopping-cart-related-customizing/retrieving-the-current-shopping-cart) in RESTful manner.
This customizations are applied only on request with `api/store` prefix to not break default e-commerce functionality:
- Custom `IShoppingCartCache` - session usage is removed, cache key for cart's cache token identifier (`jti` claim) is
used instead.
So cache duration is also determined by current token expiration time and very short time for token expiration can cause
more frequent retrieving from database.
- Custom `ICurrentShoppingCartService` - session and cookie access is removed, current shopping cart is retrieved from
`ShoppingCartGUID` header value.

In all API responses current `ShoppingCartGuid` is always sent to ensure correct shopping cart is always saved on client
in cases like user log in/log out.

#### Discounts
All KX 13 discounts and coupon codes are supported.

#### Currencies
By default shopping is calculated in main site's currency. Cart's currency can be changes via `api/store/cart/set-currency`.
All enabled currencies can be retrieved from `api/store/site/currencies`.

#### Current known limitations
Not all cart's data can be changed, f.e. custom data (properties like ShoppingCartCustomData) cannot be currenly changed
via API.

### Orders
- Endpoint `api/store/order/list` for retrieving list of orders based on request (supports paging)

### Customers
- Endpoint `api/store/customer/addresses` for retrieving customer's addresses

### Store site
- Endpoint `api/store/site/cultures` returns all enabled site cultures
- Endpoint `api/store/site/currencies` returns all enabled site currencies

### User synchronization
- Endpoint `api/store/synchronization/user-synchronization` creates new user
  - Client app should use this to ensure all new users on client's are synchronized to KX 13, this is necessary when client's
e-commerce solution allows users to log in. Users are created with random generated password and are used only for
API authorization and assigning to MembershipContext.

#### Current known limitations
User's roles synchronization isn't currently supported. We assume before start of using this API, users are already synchronized
between client and KX app.

### Setup

**How to setup your Kentico 13 ASP.NET Core application**:

Add this package to your Kentico Xperience 13 ASP.NET.Core application (live site or create standalone application for
when your KX 13 live site is not running)

```powershell
dotnet add package Kentico.Xperience.StoreApi
```

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

## K13 Ecommerce integration in Xperience By Kentico

Library `Kentico.Xperience.K13Ecommerce` encapsulates Store API calls and exposes several services for KX 13 e-commerce
integration:

- `IShoppingService`
- `ICustomerService`
- `IProductService`
- `ISiteStoreService`
- `ICountryService`

### Products synchronization

### Setup

Add these packages to your XbyK application using the .NET CLI

```powershell
dotnet add package Kentico.Xperience.K13Ecommerce
dotnet add package Kentico.Xperience.Store.Rcl
```

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

### Library matrix

| Library                            | Xperience Version | Library Version |
|------------------------------------|-------------------| --------------- |
| Kentico.Xperience.Ecommerce.Common | \>= 28.2.1        | 1.0.0           |
| Kentico.Xperience.K13Ecommerce     | \>= 28.2.1        | 1.0.0           |
| Kentico.Xperience.Store.Rcl        | \>= 28.2.1        | 1.0.0           |

