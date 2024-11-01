using Duende.AccessTokenManagement;

using IdentityModel.Client;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;
using Kentico.Xperience.K13Ecommerce.Activities;
using Kentico.Xperience.K13Ecommerce.Admin;
using Kentico.Xperience.K13Ecommerce.Config;
using Kentico.Xperience.K13Ecommerce.Countries;
using Kentico.Xperience.K13Ecommerce.Customers;
using Kentico.Xperience.K13Ecommerce.Orders;
using Kentico.Xperience.K13Ecommerce.Products;
using Kentico.Xperience.K13Ecommerce.ShoppingCart;
using Kentico.Xperience.K13Ecommerce.SiteStore;
using Kentico.Xperience.K13Ecommerce.StoreApi;
using Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;
using Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;
using Kentico.Xperience.K13Ecommerce.Synchronization.ProductPages;
using Kentico.Xperience.K13Ecommerce.Synchronization.Products;
using Kentico.Xperience.K13Ecommerce.Synchronization.ProductVariants;
using Kentico.Xperience.K13Ecommerce.Users;
using Kentico.Xperience.K13Ecommerce.Users.UserSynchronization;
using Kentico.Xperience.K13Ecommerce.WebPageFolders;
using Kentico.Xperience.K13Ecommerce.WebsiteChannel;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.K13Ecommerce;

/// <summary>
/// Service collection extensions for Kentico Store.
/// </summary>
public static class KenticoStoreServiceCollectionExtensions
{
    /// <summary>
    /// Adds Kentico Store services to the service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddKenticoStoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KenticoStoreConfig>().Bind(configuration.GetSection("CMS" + nameof(KenticoStoreConfig)));

        // default cache for token management
        services.AddDistributedMemoryCache();
        //add token management config
        services.AddClientCredentialsTokenManagement();
        //customize token endpoint service
        services.AddTransient<IClientCredentialsTokenEndpointService, CustomClientCredentialsTokenEndpointService>();
        //decorate default cache implementation to use session storage as primary storage for scoped requests 
        services.Decorate<IClientCredentialsTokenCache, SessionClientCredentialsTokenCache>();

        services.AddOptions<ClientCredentialsClient>(TokenManagementConstants.StoreApiClientName)
            .Configure<IServiceProvider>((client, sp) =>
            {
                var config = sp.GetRequiredService<IOptionsMonitor<KenticoStoreConfig>>().CurrentValue;

                client.TokenEndpoint = config.StoreApiUrl.TrimEnd('/') + "/api/store/auth/token";
                client.ClientId = config.ClientId;
                client.ClientSecret = config.ClientSecret;
                client.ClientCredentialStyle = ClientCredentialStyle.PostBody;
            });

        services.AddHttpClient<IKenticoStoreApiClient, KenticoStoreApiClient>((sp, client) =>
        {
            var config = sp.GetRequiredService<IOptionsMonitor<KenticoStoreConfig>>().CurrentValue;
            if (string.IsNullOrWhiteSpace(config.StoreApiUrl))
            {
                throw new InvalidOperationException($"Configuration '{nameof(KenticoStoreConfig)}:{nameof(KenticoStoreConfig.StoreApiUrl)}' is not set");
            }

            client.BaseAddress = new Uri(config.StoreApiUrl);
        })
        .AddClientCredentialsTokenHandler(TokenManagementConstants.StoreApiClientName);

        services.AddTransient<H>();
        services.AddHttpClient(ClientCredentialsTokenManagementDefaults.BackChannelHttpClientName)
            .AddHttpMessageHandler<H>();

        services.AddTransient<ITokenManagementService, TokenManagementService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IContentItemService, ContentItemServiceBase>();
        services.AddScoped<IProductImageSynchronizationService, ProductImageSynchronizationService>();
        services.AddScoped<IProductVariantSynchronizationService, ProductVariantSynchronizationService>();
        services.AddScoped<IProductSynchronizationService, ProductSynchronizationService>();
        services.AddScoped<IProductPageSynchronizationService, ProductPageSynchronizationService>();
        services.AddScoped<IUserSynchronizationService, UserSynchronizationService>();
        services.AddScoped<IShoppingCartSessionStorage, ShoppingCartSessionStorage>();
        services.AddScoped<IShoppingCartClientStorage, ShoppingCartClientStorage>();
        services.AddScoped<IShoppingService, ShoppingService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddSingleton<ICountryService, CountryService>();
        services.AddSingleton<IEcommerceActivityLogger, EcommerceActivityLogger>();
        services.AddScoped<ISiteStoreService, SiteStoreService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IWebPageFolderService, WebPageFolderService>();
        services.AddScoped<IContentItemFolderSynchronizationService, ContentItemFolderSynchronizationService>();

        services.AddSingleton<IK13EcommerceModuleInstaller, K13EcommerceModuleInstaller>();
        services.AddSingleton<IWebsiteChannelProvider, WebsiteChannelProvider>();

        return services;
    }
}

internal class H(ILogger<H> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is not null)
        {
            var c = await request.Content.ReadAsStringAsync();

            logger.LogError(c);
        }


        var response = await base.SendAsync(request, cancellationToken);

        if (response.Content is not null)
        {
            var c = await response.Content.ReadAsStringAsync();
            logger.LogError(c);
        }

        return response;
    }
}
