namespace Kentico.Xperience.StoreApi.ShoppingCart;

public class KShippingOption
{
    public int ShippingOptionId { get; set; }
    public string ShippingOptionName { get; set; }
    public string ShippingOptionDisplayName { get; set; }
    public string ShippingOptionDescription { get; set; }
    public string ShippingOptionThumbnailUrl { get; set; }
    public decimal ShippingOptionPrice { get; set; }
}
