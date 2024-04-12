namespace Kentico.Xperience.StoreApi.Products.Prices;

/// <summary>
/// Prices for product and it's variants
/// </summary>
public class ProductPricesResponse
{
    public int ProductSkuId { get; set; }

    public KProductCatalogPrices Prices { get; set; }

    public Dictionary<int, KProductCatalogPrices> VariantPrices { get; set; }
}
