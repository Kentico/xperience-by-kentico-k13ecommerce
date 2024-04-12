namespace Kentico.Xperience.StoreApi.Products.Prices;

public class ProductPricesResponse
{
    public int ProductSkuId { get; set; }
    public KProductCatalogPrices Prices { get; set; }
    public Dictionary<int, KProductCatalogPrices> VariantPrices { get; set; }
}
