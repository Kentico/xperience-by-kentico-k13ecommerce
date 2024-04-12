using CMS.ContentEngine;

using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;
using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

internal class ProductImageSynchronizationItem : ContentItemSynchronizationBase, ISynchronizationItem<ProductImageDto, ProductImage>
{
    protected override string DisplayNameInternal => Item.ImageDescription;


    public override string ContentTypeName => ProductImage.CONTENT_TYPE_NAME;


    public required ProductImageDto Item { get; set; }


    public ContentItemAssetMetadataWithSource? ImageAsset { get; set; }


    public bool GetModifiedProperties(ProductImage contentItem, out Dictionary<string, object?> modifiedProps)
    {
        modifiedProps = [];
        SetPropsIfDiff(contentItem.ProductImageDescription, Item.ImageDescription, nameof(ProductImage.ProductImageDescription), modifiedProps);
        SetPropsIfDiff(contentItem.ProductImageOriginalGUID, Item.ExternalId, nameof(ProductImage.ProductImageOriginalGUID), modifiedProps);

        return modifiedProps.Count > 0;
    }


    public override Dictionary<string, object?> ToDict() => new()
    {
        [nameof(ProductImage.ProductImageDescription)] = Item.ImageDescription,
        [nameof(ProductImage.ProductImageOriginalGUID)] = Item.ExternalId,
        [nameof(ProductImage.ProductImageAsset)] = ImageAsset
    };
}
