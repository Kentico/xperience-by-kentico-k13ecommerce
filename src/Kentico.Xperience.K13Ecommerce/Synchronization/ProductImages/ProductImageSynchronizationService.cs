using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

using Microsoft.Extensions.Logging;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

public class ProductImageSynchronizationService : SynchronizationServiceCommon, IProductImageSynchronizationService
{
    private readonly IContentItemService contentItemService;
    private readonly ILogger<ProductImageSynchronizationService> logger;


    public ProductImageSynchronizationService(IContentItemService contentItemService,
        ILogger<ProductImageSynchronizationService> logger,
        IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        this.contentItemService = contentItemService;
        this.logger = logger;
    }


    public async Task<IReadOnlySet<Guid>> ProcessImages(IEnumerable<ProductImageDto> images,
        IEnumerable<ProductImage> existingImages, string language, int userId)
    {
        var (toCreate, toUpdate, toDelete) = ClassifyItems<ProductImageDto, ProductImage, string>(images, existingImages);

        var newContentsIDs = new HashSet<int>();
        foreach (var imageToCreate in toCreate)
        {
            if (await CreateProductImage(imageToCreate, language, userId) is int id and > 0)
            {
                newContentsIDs.Add(id);
            }
        }

        foreach (var (storeImage, contentItemImage) in toUpdate)
        {
            await UpdateProductImage(storeImage, contentItemImage, language, userId);
        }

        await DeleteNotExistingImages(toDelete, language, userId);

        var newImages = await contentItemService.GetContentItems<ProductImage>(ProductImage.CONTENT_TYPE_NAME,
            q => q.Where(x => x.WhereIn(nameof(ProductImage.SystemFields.ContentItemID), newContentsIDs)));

        return newImages.Concat(toUpdate.Select(i => i.ContentItem))
            .OrderBy(i => i.ProductImageDescription)
            .Select(i => i.SystemFields.ContentItemGUID)
            .ToHashSet();
    }


    private async Task<int> CreateProductImage(ProductImageDto productImage, string languageName, int userID)
    {
        var syncItem = new ProductImageSynchronizationItem
        {
            Item = productImage,
            ImageAsset = await CreateAssetMetadata(productImage.ImageUrl, productImage.ImageDescription),
        };

        var addParams = new ContentItemAddParams()
        {
            ContentItem = syncItem,
            LanguageName = languageName,
            UserID = userID
        };

        int itemId = await contentItemService.AddContentItem(addParams);
        if (itemId == 0)
        {
            logger.LogError("Could not add content item {DisplayName}", syncItem.DisplayName);
        }

        return itemId;
    }


    private async Task UpdateProductImage(ProductImageDto productImage, ProductImage existingImage, string languageName,
        int userId)
    {
        var syncItem = new ProductImageSynchronizationItem { Item = productImage };
        if (syncItem.GetModifiedProperties(existingImage, out var modifiedProps))
        {
            bool assetUpload = false;
            if (modifiedProps.ContainsKey(nameof(ProductImage.ProductImageOriginalPath)))
            {
                syncItem.ImageAsset = await CreateAssetMetadata(productImage.ImageUrl, productImage.ImageDescription);
                assetUpload = true;
            }

            if (!await contentItemService.UpdateContentItem(new ContentItemUpdateParams
            {
                ContentItemParams = assetUpload ? syncItem.ToDict() : modifiedProps,
                ContentItemID = existingImage.SystemFields.ContentItemID,
                LanguageName = languageName,
                UserID = userId,
                VersionStatus = existingImage.SystemFields.ContentItemCommonDataVersionStatus
            }))
            {
                logger.LogError("Could not update item {DisplayName}", syncItem.DisplayName);
            }
        }
    }


    private async Task DeleteNotExistingImages(IEnumerable<ProductImage> imagesToDelete, string languageName,
        int userId)
    {
        await contentItemService.DeleteContentItems(imagesToDelete.Select(p => p.SystemFields.ContentItemID),
            languageName, userId);
    }
}
