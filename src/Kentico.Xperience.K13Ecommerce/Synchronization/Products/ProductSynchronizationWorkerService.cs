using Kentico.Xperience.K13Ecommerce.Settings;
using Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;
using Kentico.Xperience.K13Ecommerce.Synchronization.ProductPages;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products
{
    internal class ProductSynchronizationWorkerService(
        IProductSynchronizationService productSynchronizationService,
        IContentItemFolderSynchronizationService contentItemFolderSynchronizationService,
        IProductPageSynchronizationService productPageSynchronizationService,
        IEcommerceSettingsService ecommerceSettingsService) : IProductSynchronizationWorkerService
    {

        public async Task SynchronizeProducts()
        {
            var ecommerceSettings = (await ecommerceSettingsService.GetEcommerceSettings())
                ?? throw new InvalidOperationException("No K13 Ecommerce settings is found");
            await productSynchronizationService.SynchronizeProducts(ecommerceSettings);
            await contentItemFolderSynchronizationService.SynchronizeContentItemFolders(ecommerceSettings);
            await productPageSynchronizationService.SynchronizeProductPages();
        }
    }
}
