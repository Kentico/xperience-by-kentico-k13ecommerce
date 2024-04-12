using System.Diagnostics;

using CMS.Base;
using CMS.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products;

internal class ProductSynchronizationWorker : ThreadWorker<ProductSynchronizationWorker>
{
    private ILogger<ProductSynchronizationWorker> logger = null!;

    protected override int DefaultInterval { get; } = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;


    protected override void Initialize()
    {
        base.Initialize();
        logger = Service.Resolve<ILogger<ProductSynchronizationWorker>>();
    }


    /// <summary>Method processing actions.</summary>
    protected override void Process()
    {
        Debug.WriteLine($"Worker {GetType().FullName} running");

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
    }


    protected override void Finish()
    {
    }
}
