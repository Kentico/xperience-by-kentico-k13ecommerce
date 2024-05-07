using CMS.Ecommerce;

using Kentico.Xperience.StoreApi.Currencies;

namespace Kentico.Xperience.StoreApi.ShoppingCart;

/// <summary>
/// Model representing shopping cart content.
/// </summary>
public class KShoppingCartContent : ShoppingCartBaseResponse
{
    public decimal GrandTotal { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal TotalItemsPrice { get; set; }

    public decimal OtherPayments { get; set; }

    public decimal OrderDiscount { get; set; }

    public IEnumerable<KSummaryItem> TaxSummary { get; set; }

    public IEnumerable<KSummaryItem> OrderDiscountSummary { get; set; }

    public decimal ItemsDiscount { get; set; }

    public decimal TotalTax { get; set; }

    public decimal TotalShipping { get; set; }

    public decimal RemainingAmountForFreeShipping { get; set; }

    public KCurrency Currency { get; set; }

    public IEnumerable<ICouponCode> CouponCodes { get; set; }

    public IEnumerable<KShoppingCartItem> CartProducts { get; set; }
}
