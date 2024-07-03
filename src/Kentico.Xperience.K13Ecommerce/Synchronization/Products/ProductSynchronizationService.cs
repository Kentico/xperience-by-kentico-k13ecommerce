using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Membership;

using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;
using Kentico.Xperience.K13Ecommerce.Products;
using Kentico.Xperience.K13Ecommerce.SiteStore;
using Kentico.Xperience.K13Ecommerce.StoreApi;
using Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;
using Kentico.Xperience.K13Ecommerce.Synchronization.ProductVariants;

using Microsoft.Extensions.Logging;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products;

internal class ProductSynchronizationService(
    ILogger<ProductSynchronizationService> logger,
    IContentItemService contentItemService,
    IProductService productService,
    IProductVariantSynchronizationService variantSynchronizationService,
    IProductImageSynchronizationService productImageSynchronizationService,
    IHttpClientFactory httpClientFactory,
    ISiteStoreService siteStoreService) : SynchronizationServiceCommon(httpClientFactory), IProductSynchronizationService
{
    public async Task SynchronizeProducts()
    {
        //Current limitations: only synchronization from default culture is supported. XByK must have same language enabled as default culture in K13
        string defaultCultureCode = ((await siteStoreService.GetCultures()).FirstOrDefault(c => c.CultureIsDefault)?.CultureCode)
            ?? throw new InvalidOperationException("No default culture found on K13 Store");

        string language = defaultCultureCode[..2];

        var kenticoStoreProducts =
            await productService.GetProductPages(new ProductPageRequest
            {
                Path = "/",
                Culture = defaultCultureCode,
                Limit = 1000,
                WithVariants = true,
                WithLongDescription = true,
                NoLinks = true
            });

        var kenticoStandaloneProducts = (await productService.GetStandaloneProducts(new ProductRequest
        {
            Culture = defaultCultureCode,
            Limit = 1000,
            WithVariants = true
        })).Select(p => new KProductNode(p));

        var contentItemProducts =
            await contentItemService.GetContentItems<ProductSKU>(ProductSKU.CONTENT_TYPE_NAME, linkedItemsLevel: 2);

        var (toCreate, toUpdate, toDelete) =
            ClassifyItems<KProductNode, ProductSKU, int>(kenticoStoreProducts.Concat(kenticoStandaloneProducts), contentItemProducts);

        int adminUserId = UserInfoProvider.AdministratorUser.UserID;
        foreach (var productToCreate in toCreate)
        {
            try
            {
                //transaction is used to prevent variant/images orphans
                using var transaction = new CMSTransactionScope();

                var variantGuids = await variantSynchronizationService.ProcessVariants(
                    productToCreate.Sku?.Variants ?? Enumerable.Empty<KProductVariant>(),
                    Enumerable.Empty<ProductVariant>(), language, adminUserId);

                var imagesGuids = await productImageSynchronizationService.ProcessImages(GetImageDtos(productToCreate),
                    Enumerable.Empty<ProductImage>(), language, adminUserId);

                await CreateProduct(
                    GetProductSynchronizationItem(productToCreate, variantGuids, imagesGuids),
                    language, adminUserId);

                transaction.Commit();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during creating product with SKUID: {SKUID}", productToCreate.Sku?.Skuid);
            }
        }

        foreach (var (storeProduct, contentItemProduct) in toUpdate)
        {
            try
            {
                //transaction is used to prevent variant/images orphans
                using var transaction = new CMSTransactionScope();

                var variantGuid = await variantSynchronizationService.ProcessVariants(storeProduct.Sku?.Variants ??
                    Enumerable.Empty<KProductVariant>(),
                    contentItemProduct.ProductVariants, language, adminUserId);

                var imagesGuids = await productImageSynchronizationService.ProcessImages(GetImageDtos(storeProduct),
                    contentItemProduct.ProductImages, language, adminUserId);

                await UpdateProduct(
                    GetProductSynchronizationItem(storeProduct, variantGuid, imagesGuids),
                    contentItemProduct, language, adminUserId);

                transaction.Commit();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during updating product with SKUID: {SKUID}", storeProduct.Sku?.Skuid);
            }
        }

        await DeleteNotExistingProducts(toDelete, language, adminUserId);
    }


    private async Task CreateProduct(ProductSynchronizationItem productSyncItem, string languageName, int userID)
    {
        var addParams = new ContentItemAddParams()
        {
            ContentItem = productSyncItem,
            LanguageName = languageName,
            UserID = userID
        };

        await CreateContentItem(addParams);
    }


    private async Task UpdateProduct(ProductSynchronizationItem productSyncItem, ProductSKU existingProduct,
        string languageName, int userID)
    {
        if (productSyncItem.GetModifiedProperties(existingProduct, out var modifiedProps))
        {
            var updateParams = new ContentItemUpdateParams
            {
                ContentItemParams = modifiedProps,
                ContentItemID = existingProduct.SystemFields.ContentItemID,
                LanguageName = languageName,
                UserID = userID,
                VersionStatus = existingProduct.SystemFields.ContentItemCommonDataVersionStatus
            };

            await UpdateContentItem(updateParams);
        }
    }


    /// <summary>
    /// Change this method when product has more images (API is extended)
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    private IEnumerable<ProductImageDto> GetImageDtos(KProductNode product)
        => product.Sku != null ?
            new List<ProductImageDto>
            {
                new() { ImageUrl = product.Sku.MainImageUrl!, ImageDescription = product.Sku.SkuName! }
            }.Where(i => !string.IsNullOrWhiteSpace(i.ExternalId)) :
            Enumerable.Empty<ProductImageDto>();


    private ProductSynchronizationItem GetProductSynchronizationItem(KProductNode product, IEnumerable<Guid> variants,
        IEnumerable<Guid> images)
        => new()
        {
            Item = product,
            Variants = variants.Select(v => new ContentItemReference() { Identifier = v }).ToList(),
            Images = images.Select(i => new ContentItemReference() { Identifier = i }).ToList()
        };


    private async Task CreateContentItem(ContentItemAddParams addParams)
    {
        if (await contentItemService.AddContentItem(addParams) == 0)
        {
            throw new InvalidOperationException("Could not create product " + addParams.ContentItem.DisplayName);
        }
    }


    private async Task UpdateContentItem(ContentItemUpdateParams updateParams)
    {
        if (!await contentItemService.UpdateContentItem(updateParams))
        {
            throw new InvalidOperationException("Could not update product with content item ID " + updateParams.ContentItemID);
        }
    }


    private async Task DeleteNotExistingProducts(IEnumerable<ProductSKU> productsToDelete, string languageName,
        int userId)
    {
        await contentItemService.DeleteContentItems(productsToDelete.Select(p => p.SystemFields.ContentItemID),
            languageName, userId);

        var childItemsToDelete = productsToDelete
            .SelectMany(p =>
                p.ProductVariants.Select(v => v.SystemFields.ContentItemID)
                    .Concat(p.ProductImages.Select(i => i.SystemFields.ContentItemID)));

        await contentItemService.DeleteContentItems(childItemsToDelete, languageName, userId);
    }
}
