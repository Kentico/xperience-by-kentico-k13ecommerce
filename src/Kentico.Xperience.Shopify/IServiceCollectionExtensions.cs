using Kentico.Xperience.Shopify.Config;
using Kentico.Xperience.Shopify.Services;
using Kentico.Xperience.Shopify.Services.ProductService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShopifySharp.Extensions.DependencyInjection;

namespace Kentico.Xperience.Shopify;
public static class IServiceCollectionExtensions
{
    public static void RegisterShopifyServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Get Shopify config from appsettings.json
        services.Configure<ShopifyConfig>(configuration.GetSection(nameof(ShopifyConfig)));

        // ShopifySharp dependency injection
        services.AddShopifySharpServiceFactories();

        services.AddScoped<IShopifyProductService, ShopifyProductService>();
        services.AddScoped<IShopifyCollectionService, ShopifyCollectionService>();
        services.AddScoped<IShopifyCurrencyService, ShopifyCurrencyService>();
    }
}
