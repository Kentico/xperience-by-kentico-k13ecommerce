using System.ComponentModel.DataAnnotations;

using Kentico.Xperience.StoreApi.Currencies;
using Kentico.Xperience.StoreApi.Customers;
using Kentico.Xperience.StoreApi.ShoppingCart;

namespace Kentico.Xperience.StoreApi.Orders;

/// <summary>
/// Dto for <see cref="CMS.Ecommerce.OrderInfo" />.
/// </summary>
public class KOrder
{
    [Required] public int OrderId { get; set; }

    [Required] public decimal OrderTotalTax { get; set; }

    public string OrderTaxSummary { get; set; }

    public string OrderInvoiceNumber { get; set; }

    public KCurrency OrderCurrency { get; set; }

    public KShippingOption OrderShippingOption { get; set; }

    public KPaymentOption OrderPaymentOption { get; set; }

    public string OrderNote { get; set; }

    public KCustomer OrderCustomer { get; set; }

    public KOrderStatus OrderStatus { get; set; }

    [Required] public decimal OrderGrandTotal { get; set; }

    public decimal OrderGrandTotalInMainCurrency { get; set; }

    [Required] public decimal OrderTotalPrice { get; set; }

    public decimal OrderTotalPriceInMainCurrency { get; set; }

    public decimal OrderTotalShipping { get; set; }

    public DateTime OrderDate { get; set; }

    public bool OrderIsPaid { get; set; }

    public string OrderCulture { get; set; }

    public string OrderDiscounts { get; set; }

    public string OrderOtherPayments { get; set; }

    public string OrderCouponCodes { get; set; }

    public string OrderPaymentFormattedResult { get; set; }

    public string OrderTrackingNumber { get; set; }

    public int OrderSiteId { get; set; }

    public DateTime OrderLastModified { get; set; }

    public Dictionary<string, object> OrderCustomData { get; set; }

    public KAddress OrderBillingAddress { get; set; }

    public KAddress OrderShippingAddress { get; set; }

    public KAddress OrderCompanyAddress { get; set; }

    public IEnumerable<KOrderItem> OrderItems { get; set; }

    public KPaymentResult OrderPaymentResult { get; set; }
}
