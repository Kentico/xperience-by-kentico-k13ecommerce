using System.Diagnostics;

using CMS.Base;
using CMS.Core;

using Kentico.Xperience.K13Ecommerce.Config;
using Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;
using Kentico.Xperience.K13Ecommerce.Synchronization.ProductPages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products;

/// <summary>
/// Synchronization worker for product synchronization.
/// </summary>
internal class ProductSynchronizationWorker : ThreadWorker<ProductSynchronizationWorker>
{
    private ILogger<ProductSynchronizationWorker> logger = null!;
    private IOptionsMonitor<KenticoStoreConfig> storeConfig = null!;

    protected override int DefaultInterval => (int)TimeSpan.FromMinutes(storeConfig.CurrentValue.ProductSyncInterval).TotalMilliseconds;


    protected override void Initialize()
    {
        base.Initialize();
        logger = Service.Resolve<ILogger<ProductSynchronizationWorker>>();
        storeConfig = Service.Resolve<IOptionsMonitor<KenticoStoreConfig>>();
    }


    /// <summary>Method processing actions.</summary>
    protected override void Process()
    {
        Debug.WriteLine($"Worker {GetType().FullName} running");

        if (!storeConfig.CurrentValue.ProductSyncEnabled)
        {
            Debug.WriteLine("Product sync is disabled");
            return;
        }

        try
        {
            SynchronizeProducts();
            logger.LogInformation("K13-Store product synchronization done.");
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error occured during running '{GetType().Name}'");
        }
    }


    private void SynchronizeProducts()
    {
        using var serviceScope = Service.Resolve<IServiceProvider>().CreateScope();
        var provider = serviceScope.ServiceProvider;
        var productSynchronizationService = provider.GetRequiredService<IProductSynchronizationService>();
        productSynchronizationService.SynchronizeProducts().GetAwaiter().GetResult();
        var contentItemFolderSynchronizationService = provider.GetRequiredService<IContentItemFolderSynchronizationService>();
        contentItemFolderSynchronizationService.SynchronizeContentItemFolders().GetAwaiter().GetResult();
        var productPageSynchronizationService = provider.GetRequiredService<IProductPageSynchronizationService>();
        productPageSynchronizationService.SynchronizeProductPages().GetAwaiter().GetResult();
    }


    protected override void Finish()
    {
    }
}
