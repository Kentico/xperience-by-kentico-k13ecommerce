using CMS.Integration.K13Ecommerce;

using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.Extensions.Logging;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductVariants;

internal class ProductVariantSynchronizationService : SynchronizationServiceCommon,
    IProductVariantSynchronizationService
{
    private readonly IContentItemService contentItemService;
    private readonly ILogger<ProductVariantSynchronizationService> logger;


    /// <inheritdoc/>
    public ProductVariantSynchronizationService(IContentItemService contentItemService,
        ILogger<ProductVariantSynchronizationService> logger,
        IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        this.contentItemService = contentItemService;
        this.logger = logger;
    }


    /// <inheritdoc/>
    public async Task<IReadOnlySet<Guid>> ProcessVariants(IEnumerable<KProductVariant> variants,
        IEnumerable<ProductVariant> existingVariants,
        K13EcommerceSettingsInfo ecommerceSettings, string language, int userId)
    {
        var (toCreate, toUpdate, toDelete) =
            ClassifyItems<KProductVariant, ProductVariant, int>(variants, existingVariants);
        var newContentIDs = new HashSet<int>();
        foreach (var variantToCreate in toCreate)
        {
            if (await CreateVariant(GetProductSynchronizationItem(variantToCreate), ecommerceSettings, language, userId) is int id and > 0)
            {
                newContentIDs.Add(id);
            }
        }

        foreach (var (storeVariant, contentItemVariant) in toUpdate)
        {
            await UpdateVariant(GetProductSynchronizationItem(storeVariant), contentItemVariant, language, userId);
        }

        await DeleteNotExistingVariants(toDelete, language, userId);

        var newVariants = await contentItemService.GetContentItems<ProductVariant>(ProductVariant.CONTENT_TYPE_NAME,
            q =>
                q.Where(x => x.WhereIn(nameof(ProductVariant.SystemFields.ContentItemID), newContentIDs)));

        return newVariants.Concat(toUpdate.Select(i => i.ContentItem))
            .OrderBy(v => v.SKUName)
            .Select(v => v.SystemFields.ContentItemGUID)
            .ToHashSet();
    }


    private async Task<int> CreateVariant(ProductVariantSynchronizationItem variantSyncItem, K13EcommerceSettingsInfo ecommerceSettings,
        string languageName, int userId)
    {
        int itemID = await contentItemService.AddContentItem(new ContentItemAddParams()
        {
            ContentItem = variantSyncItem,
            LanguageName = languageName,
            UserID = userId,
            WorkspaceName = ecommerceSettings.K13EcommerceSettingsEffectiveWorkspaceName
        });

        if (itemID == 0)
        {
            logger.LogError("Could not add content item {DisplayName}", variantSyncItem.DisplayName);
        }

        return itemID;
    }


    private async Task UpdateVariant(ProductVariantSynchronizationItem variantSyncItem, ProductVariant existingVariant,
        string languageName, int userId)
    {
        if (!variantSyncItem.GetModifiedProperties(existingVariant, out var modifiedProps))
        {
            return;
        }

        if (!await contentItemService.UpdateContentItem(new ContentItemUpdateParams
        {
            ContentItemParams = modifiedProps,
            ContentItemID = existingVariant.SystemFields.ContentItemID,
            LanguageName = languageName,
            UserID = userId,
            VersionStatus = existingVariant.SystemFields.ContentItemCommonDataVersionStatus
        }))
        {
            logger.LogError("Could not update item {DisplayName}", variantSyncItem.DisplayName);
        }
    }


    private ProductVariantSynchronizationItem GetProductSynchronizationItem(KProductVariant productVariant)
        => new() { Item = productVariant };


    private async Task DeleteNotExistingVariants(IEnumerable<ProductVariant> variantsToDelete, string languageName,
        int userId)
    {
        await contentItemService.DeleteContentItems(variantsToDelete.Select(p => p.SystemFields.ContentItemID),
            languageName, userId);
    }
}
