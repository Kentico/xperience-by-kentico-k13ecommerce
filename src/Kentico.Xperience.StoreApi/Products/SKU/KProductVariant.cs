using Kentico.Xperience.StoreApi.Products.Prices;

namespace Kentico.Xperience.StoreApi.Products.SKU;

/// <summary>
/// Model for Kentico product variant.
/// </summary>
public class KProductVariant
{
    public int SKUID { get; set; }

    public string SKUName { get; set; }

    public string SKUNumber { get; set; }

    public decimal SKUPrice { get; set; }

    public int SKUAvailableItems { get; set; }

    public bool SKUEnabled { get; set; }

    public KProductCatalogPrices Prices { get; set; }
}
