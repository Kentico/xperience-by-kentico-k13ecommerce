using System.Text.Json.Serialization;
using CMS.Ecommerce;
using CMS.Helpers;
using Kentico.Xperience.StoreApi.Products.Prices;

namespace Kentico.Xperience.StoreApi.Products.SKU;

/// <summary>
/// Represents model for SKU
/// </summary>
public class KProductSKU
{
    public int SKUID { get; set; }
    public string SKUName { get; set; }
    public string SKUShortDescription { get; set; }
    public string SKUNumber { get; set; }
    public bool SKUEnabled { get; set; }
    public int SKUAvailableInDays { get; set; }
    public decimal SKUPrice { get; set; }
    public decimal SKURetailPrice { get; set; }
    public int SKUSiteID { get; set; }
    [JsonIgnore] public string SKUImagePath { get; set; }
    public TrackInventoryTypeEnum SKUTrackInventory { get; set; }
    public int SKUAvailableItems { get; set; }
    public SKUProductTypeEnum SKUProductType { get; set; }
    public ValidityEnum SKUValidity { get; set; }
    public int SKUValidFor { get; set; }
    public bool SKUNeedsShipping { get; set; }
    public int SKUMinItemsInOrder { get; set; }
    public int SKUMaxItemsInOrder { get; set; }
    public DateTime SKUInStoreFrom { get; set; }
    /// <summary>
    /// Overwrite this property when product image is saved differently
    /// </summary>
    public virtual string MainImageUrl => URLHelper.GetAbsoluteUrl(SKUImagePath);
    public IReadOnlyList<KProductVariant> Variants { get; set; }
    public KProductCatalogPrices Prices { get; set; }
    //@TODO properties for brands, manufacturer, public statuses etc.
}