using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;
using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductVariants;

internal class ProductVariantSynchronizationItem : ContentItemSynchronizationBase,
    ISynchronizationItem<KProductVariant, ProductVariant>
{
    protected override string DisplayNameInternal => Item.SkuName!;


    /// <inheritdoc/>
    public override string ContentTypeName => ProductVariant.CONTENT_TYPE_NAME;


    /// <inheritdoc/>
    public required KProductVariant Item { get; set; }


    /// <inheritdoc/>
    public bool GetModifiedProperties(ProductVariant contentItem, out Dictionary<string, object?> modifiedProps)
    {
        modifiedProps = [];
        SetPropsIfDiff(contentItem.SKUName, Item.SkuName, nameof(ProductVariant.SKUName), modifiedProps);
        SetPropsIfDiff(contentItem.SKUNumber, Item.SkuNumber, nameof(ProductVariant.SKUNumber), modifiedProps);
        SetPropsIfDiff(contentItem.SKUAvailableItems, Item.SkuAvailableItems, nameof(ProductVariant.SKUAvailableItems),
            modifiedProps);
        SetPropsIfDiff(contentItem.SKUEnabled, Item.SkuEnabled, nameof(ProductVariant.SKUEnabled), modifiedProps);

        return modifiedProps.Count > 0;
    }


    /// <inheritdoc/>
    public override Dictionary<string, object?> ToDict()
        => new()
        {
            [nameof(ProductVariant.SKUID)] = Item.Skuid,
            [nameof(ProductVariant.SKUName)] = Item.SkuName,
            [nameof(ProductVariant.SKUNumber)] = Item.SkuNumber,
            [nameof(ProductVariant.SKUAvailableItems)] = Item.SkuAvailableItems,
            [nameof(ProductVariant.SKUEnabled)] = Item.SkuEnabled
        };
}
