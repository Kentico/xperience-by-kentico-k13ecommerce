using Kentico.Xperience.StoreApi.Products.SKU;

namespace Kentico.Xperience.StoreApi.ShoppingCart;
public class KShoppingCartItem
{
    public int CartItemId { get; set; }
    public int CartItemUnits { get; set; }
    public DateTime CartItemValidTo { get; set; }
    public int CartItemAutoAddedUnits { get; set; }
    public string CartItemText { get; set; }
    public IEnumerable<KSummaryItem> UnitDiscountSummary { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitTotalDiscount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalPriceIncludingOptions { get; set; }
    public decimal TotalDiscount { get; set; }
    public IEnumerable<KSummaryItem> DiscountSummary { get; set; }
    public double UnitWeight { get; set; }
    public double TotalWeight { get; set; }
    public List<KShoppingCartItem> ProductOptions { get; set; } = new();
    public List<KShoppingCartItem> BundleItems { get; set; } = new();

    public KProductSKU ProductSKU { get; set; }
    public KProductVariant VariantSKU { get; set; }
}
