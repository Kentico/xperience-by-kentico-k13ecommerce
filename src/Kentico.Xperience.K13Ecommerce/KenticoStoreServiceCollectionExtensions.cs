using Kentico.Xperience.K13Ecommerce.Config;
using Kentico.Xperience.K13Ecommerce.KenticoStoreApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.K13Ecommerce;
public static class KenticoStoreServiceCollectionExtensions
{
    public static IServiceCollection AddKenticoStoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KenticoStoreConfig>().Bind(configuration.GetSection("CMS" + nameof(KenticoStoreConfig)));
        services.AddHttpClient<IKStoreApiService, KStoreApiService>((sp, client) =>
        {
            var config = sp.GetRequiredService<IOptionsMonitor<KenticoStoreConfig>>().CurrentValue;
            if (string.IsNullOrWhiteSpace(config.StoreApiUrl))
            {
                throw new InvalidOperationException($"Configuration '{nameof(KenticoStoreConfig)}:{nameof(KenticoStoreConfig.StoreApiUrl)}' is not set");
            }

            client.BaseAddress = new Uri(config.StoreApiUrl);
        });

        return services;
    }
}
