using Kentico.Xperience.StoreApi.Currencies;
using Kentico.Xperience.StoreApi.ShoppingCart;

namespace Kentico.Xperience.StoreApi.Products.Prices;

/// <summary>
/// Represents product catalog prices from catalog price calculator
/// </summary>
public class KProductCatalogPrices
{
    public KCurrency Currency { get; set; }
    public decimal StandardPrice { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public decimal ListPrice { get; set; }
    public IEnumerable<KSummaryItem> Discounts { get; set; }
}
