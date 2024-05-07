namespace Kentico.Xperience.StoreApi.Orders;

/// <summary>
/// Dto for <see cref="CMS.Ecommerce.OrderItemInfo"/>.
/// </summary>
public class KOrderItem
{
    public int OrderItemId { get; set; }

    public decimal OrderItemUnitPrice { get; set; }

    public decimal OrderItemTotalPrice { get; set; }

    public decimal OrderItemTotalPriceInMainCurrency { get; set; }

    public int OrderItemOrderId { get; set; }

    public string OrderItemSkuName { get; set; }

    public int OrderItemSkuId { get; set; }

    public int OrderItemUnitCount { get; set; }

    public Guid OrderItemGuid { get; set; }

    public Guid OrderItemParentGuid { get; set; }

    public Guid OrderItemBundleGuid { get; set; }

    public bool OrderItemSendNotification { get; set; }

    public string OrderItemText { get; set; }

    public string OrderItemProductDiscounts { get; set; }

    public string OrderItemDiscountSummary { get; set; }
}
