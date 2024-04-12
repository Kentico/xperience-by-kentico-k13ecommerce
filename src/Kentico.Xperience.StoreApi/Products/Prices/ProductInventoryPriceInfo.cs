using CMS.Ecommerce;

namespace Kentico.Xperience.StoreApi.Products.Prices;

/// <summary>
/// Model for product/variant inventory and price
/// </summary>
public class ProductInventoryPriceInfo
{
    public int SkuId { get; set; }

    public KProductCatalogPrices Prices { get; set; }

    public TrackInventoryTypeEnum SKUTrackInventory { get; set; }

    public bool SKUSellOnlyAvailable { get; set; }

    public int SKUAvailableItems { get; set; }
}
