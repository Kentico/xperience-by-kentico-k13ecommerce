using CMS.Ecommerce;
using Kentico.Xperience.StoreApi.Currencies;

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
    public ValuesSummary Discounts { get; set; }
}