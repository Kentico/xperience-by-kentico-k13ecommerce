using CMS.ContentEngine;

using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

internal class ProductImageSynchronizationItem : ContentItemSynchronizationBase, ISynchronizationItem<ProductImageDto, ProductImage>
{
    protected override string DisplayNameInternal => Item.ImageDescription;


    public override string ContentTypeName => ProductImage.CONTENT_TYPE_NAME;


    /// <inheritdoc/>
    public required ProductImageDto Item { get; set; }


    /// <inheritdoc/>
    public ContentItemAssetMetadataWithSource? ImageAsset { get; set; }


    /// <inheritdoc/>
    public bool GetModifiedProperties(ProductImage contentItem, out Dictionary<string, object?> modifiedProps)
    {
        modifiedProps = [];
        SetPropsIfDiff(contentItem.ProductImageDescription, Item.ImageDescription, nameof(ProductImage.ProductImageDescription), modifiedProps);
        SetPropsIfDiff(contentItem.ProductImageOriginalPath, Item.ExternalId, nameof(ProductImage.ProductImageOriginalPath), modifiedProps);

        return modifiedProps.Count > 0;
    }


    /// <inheritdoc/>
    public override Dictionary<string, object?> ToDict() => new()
    {
        [nameof(ProductImage.ProductImageDescription)] = Item.ImageDescription,
        [nameof(ProductImage.ProductImageOriginalPath)] = Item.ExternalId,
        [nameof(ProductImage.ProductImageAsset)] = ImageAsset
    };
}
