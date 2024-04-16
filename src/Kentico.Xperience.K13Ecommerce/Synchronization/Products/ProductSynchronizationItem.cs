using System.Text.Json;

using CMS.ContentEngine;

using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;
using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;
using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products;

internal class ProductSynchronizationItem : ContentItemSynchronizationBase, ISynchronizationItem<KProductNode, ProductSKU>
{
    private readonly JsonSerializerOptions
        jsonSerializerOptions = new(JsonSerializerDefaults.Web);


    protected override string DisplayNameInternal => Item.DocumentSKUName!;


    public override string ContentTypeName => ProductSKU.CONTENT_TYPE_NAME;


    public required KProductNode Item { get; set; }

    /// <summary>
    /// Images.
    /// </summary>
    public IEnumerable<ContentItemReference> Images { get; set; } = Enumerable.Empty<ContentItemReference>();


    /// <summary>
    /// Variants.
    /// </summary>
    public IEnumerable<ContentItemReference> Variants { get; set; } = Enumerable.Empty<ContentItemReference>();


    public bool GetModifiedProperties(ProductSKU contentItem, out Dictionary<string, object?> modifiedProps)
    {
        modifiedProps = [];
        SetPropsIfDiff(contentItem.SKUName, Item.DocumentSKUName, nameof(ProductSKU.SKUName), modifiedProps);
        SetPropsIfDiff(contentItem.SKUShortDescription, Item.DocumentSKUShortDescription, nameof(ProductSKU.SKUShortDescription), modifiedProps);
        SetPropsIfDiff(contentItem.SKUDescription, Item.DocumentSKUDescription, nameof(ProductSKU.SKUDescription), modifiedProps);
        SetPropsIfDiff(contentItem.SKUNumber, Item.Sku?.SkuNumber, nameof(ProductSKU.SKUNumber), modifiedProps);
        SetPropsIfDiff(contentItem.SKUEnabled, Item.Sku?.SkuEnabled, nameof(ProductSKU.SKUEnabled), modifiedProps);
        SetPropsIfDiff(contentItem.SKUTrackInventory, Item.Sku?.SkuTrackInventory.ToString(), nameof(ProductSKU.SKUEnabled), modifiedProps);
        SetPropsIfDiff(contentItem.SKUSellOnlyAvailable, Item.Sku?.SkuSellOnlyAvailable, nameof(ProductSKU.SKUSellOnlyAvailable), modifiedProps);
        SetPropsIfDiff(contentItem.SKUReorderAt, Item.Sku?.SkuReorderAt, nameof(ProductSKU.SKUReorderAt), modifiedProps);
        SetPropsIfDiff(contentItem.SKUProductType, Item.Sku?.SkuProductType.ToString(), nameof(ProductSKU.SKUProductType), modifiedProps);
        SetPropsIfDiff(contentItem.SKUNeedsShipping, Item.Sku?.SkuNeedsShipping, nameof(ProductSKU.SKUNeedsShipping), modifiedProps);
        SetPropsIfDiff(contentItem.SKUMinItemsInOrder, Item.Sku?.SkuMinItemsInOrder, nameof(ProductSKU.SKUMinItemsInOrder), modifiedProps);
        SetPropsIfDiff(contentItem.SKUMaxItemsInOrder, Item.Sku?.SkuMaxItemsInOrder, nameof(ProductSKU.SKUMaxItemsInOrder), modifiedProps);
        SetPropsIfDiff(contentItem.SKUInStoreFrom, Item.Sku?.SkuInStoreFrom, nameof(ProductSKU.SKUInStoreFrom), modifiedProps);
        SetPropsIfDiff(contentItem.SKUAvailableItems, Item.Sku?.SkuAvailableItems, nameof(ProductSKU.SKUAvailableItems), modifiedProps);

        string customFieldsJson = JsonSerializer.Serialize(Item.CustomFields, jsonSerializerOptions);

        SetPropsIfDiff(contentItem.CustomFields, customFieldsJson, nameof(ProductSKU.CustomFields), modifiedProps);
        SetPropsIfDiff(contentItem.ClassName, Item.ClassName, nameof(ProductSKU.ClassName), modifiedProps);
        SetPropsIfDiff(contentItem.NodeAliasPath, Item.NodeAliasPath, nameof(ProductSKU.NodeAliasPath), modifiedProps);

        if (ReferenceModified(contentItem.ProductImages, Images))
        {
            modifiedProps.TryAdd(nameof(ProductSKU.ProductImages), Images);
        }

        if (ReferenceModified(contentItem.ProductVariants, Variants))
        {
            modifiedProps.TryAdd(nameof(ProductSKU.ProductVariants), Variants);
        }

        SetPropsIfDiff(contentItem.PublicStatusDisplayName, Item.Sku?.PublicStatus?.PublicStatusDisplayName ?? string.Empty, nameof(ProductSKU.PublicStatusDisplayName), modifiedProps);
        SetPropsIfDiff(contentItem.DepartmentDisplayName, Item.Sku?.Department?.DepartmentDisplayName ?? string.Empty, nameof(ProductSKU.DepartmentDisplayName), modifiedProps);
        SetPropsIfDiff(contentItem.ManufacturerDisplayName, Item.Sku?.Manufacturer?.ManufacturerDisplayName ?? string.Empty, nameof(ProductSKU.ManufacturerDisplayName), modifiedProps);

        return modifiedProps.Count > 0;
    }

    public override Dictionary<string, object?> ToDict()
        => new()
        {
            [nameof(ProductSKU.SKUID)] = Item.Sku.Skuid,
            [nameof(ProductSKU.SKUName)] = Item.DocumentSKUName,
            [nameof(ProductSKU.SKUShortDescription)] = Item.DocumentSKUShortDescription,
            [nameof(ProductSKU.SKUDescription)] = Item.DocumentSKUDescription,
            [nameof(ProductSKU.SKUNumber)] = Item.Sku.SkuNumber,
            [nameof(ProductSKU.SKUEnabled)] = Item.Sku.SkuEnabled,
            [nameof(ProductSKU.SKUTrackInventory)] = Item.Sku.SkuTrackInventory.ToString(),
            [nameof(ProductSKU.SKUSellOnlyAvailable)] = Item.Sku.SkuSellOnlyAvailable,
            [nameof(ProductSKU.SKUReorderAt)] = Item.Sku.SkuReorderAt,
            [nameof(ProductSKU.SKUProductType)] = Item.Sku.SkuProductType.ToString(),
            [nameof(ProductSKU.SKUNeedsShipping)] = Item.Sku.SkuNeedsShipping,
            [nameof(ProductSKU.SKUMinItemsInOrder)] = Item.Sku.SkuMinItemsInOrder,
            [nameof(ProductSKU.SKUMaxItemsInOrder)] = Item.Sku.SkuMaxItemsInOrder,
            [nameof(ProductSKU.SKUInStoreFrom)] = Item.Sku.SkuInStoreFrom,
            [nameof(ProductSKU.CustomFields)] = JsonSerializer.Serialize(Item.CustomFields, jsonSerializerOptions),
            [nameof(ProductSKU.ClassName)] = Item.ClassName,
            [nameof(ProductSKU.NodeAliasPath)] = Item.NodeAliasPath,
            [nameof(ProductSKU.ProductImages)] = Images,
            [nameof(ProductSKU.ProductVariants)] = Variants,
            [nameof(ProductSKU.PublicStatusDisplayName)] = Item.Sku.PublicStatus?.PublicStatusDisplayName,
            [nameof(ProductSKU.ManufacturerDisplayName)] = Item.Sku.Manufacturer?.ManufacturerDisplayName,
            [nameof(ProductSKU.DepartmentDisplayName)] = Item.Sku.Department.DepartmentDisplayName
        };
}
