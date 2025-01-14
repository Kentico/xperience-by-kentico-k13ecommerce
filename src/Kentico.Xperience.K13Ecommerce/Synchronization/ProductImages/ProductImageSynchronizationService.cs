using System.Net.Mime;

using CMS.ContentEngine;
using CMS.Core;
using CMS.Integration.K13Ecommerce;

using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

using Microsoft.Extensions.Logging;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

internal class ProductImageSynchronizationService(IContentItemService contentItemService,
    ILogger<ProductImageSynchronizationService> logger,
    IHttpClientFactory httpClientFactory)
    : IProductImageSynchronizationService
{
    /// <inheritdoc/>
    public async Task<IReadOnlySet<Guid>> ProcessImages(IEnumerable<ProductImageDto> images,
        IEnumerable<ProductImage> existingImages, K13EcommerceSettingsInfo ecommerceSettings, string language, int userId)
    {
        var (toCreate, toUpdate, toDelete) = SynchronizationHelper.ClassifyItems<ProductImageDto, ProductImage, string>(images, existingImages);

        var newContentsIDs = new HashSet<int>();
        foreach (var imageToCreate in toCreate)
        {
            if (await CreateProductImage(imageToCreate, ecommerceSettings, language, userId) is int id and > 0)
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


    private async Task<int> CreateProductImage(ProductImageDto productImage, K13EcommerceSettingsInfo ecommerceSettings, string languageName, int userID)
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
            UserID = userID,
            WorkspaceName = ecommerceSettings.K13EcommerceSettingsEffectiveWorkspaceName
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

    protected async Task<ContentItemAssetMetadataWithSource> CreateAssetMetadata(string url, string imageDescription)
    {
        byte[] bytes;
        string? contentType;
        using (var client = httpClientFactory.CreateClient())
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            bytes = await response.Content.ReadAsByteArrayAsync();
            contentType = response.Content.Headers.ContentType?.MediaType;
        }

        long length = bytes.LongLength;
        var dataWrapper = new BinaryDataWrapper(bytes);
        var fileSource = new ContentItemAssetStreamSource(cancellationToken => Task.FromResult(dataWrapper.Stream));
        string extension = GetExtension(contentType);
        if (string.IsNullOrEmpty(extension))
        {
            logger.LogWarning("Image Creation for SKU {ImageDescription}\n" +
                "Cannot determine image extension from content type {ContentType}", imageDescription, contentType);
        }

        imageDescription += extension;

        var assetMetadata = new ContentItemAssetMetadata()
        {
            Extension = extension,
            Identifier = Guid.NewGuid(),
            LastModified = DateTime.Now,
            Name = imageDescription,
            Size = length
        };

        return new ContentItemAssetMetadataWithSource(fileSource, assetMetadata);
    }

    /// <summary>
    /// Get file extension by https://developer.mozilla.org/en-US/docs/Web/Media/Formats/Image_types
    /// </summary>
    /// <param name="contentType">Content type header</param>
    /// <returns></returns>
    private static string GetExtension(string? contentType) =>
        contentType switch
        {
            MediaTypeNames.Image.Png or "image/apng" => ".png",
            MediaTypeNames.Image.Avif => ".avif",
            MediaTypeNames.Image.Gif => ".gif",
            MediaTypeNames.Image.Jpeg => ".jpg",
            MediaTypeNames.Image.Svg => ".svg",
            MediaTypeNames.Image.Webp => ".webp",
            // Should be avoided, but still valid
            MediaTypeNames.Image.Bmp => ".bmp",
            MediaTypeNames.Image.Icon => ".ico",
            MediaTypeNames.Image.Tiff => ".tiff",
            _ => string.Empty
        };
}
