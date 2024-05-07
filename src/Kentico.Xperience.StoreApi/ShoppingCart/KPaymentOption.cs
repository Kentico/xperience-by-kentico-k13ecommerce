namespace Kentico.Xperience.StoreApi.ShoppingCart;

/// <summary>
/// Dto for <see cref="CMS.Ecommerce.PaymentOptionInfo"/>.
/// </summary>
public class KPaymentOption
{
    public int PaymentOptionId { get; set; }

    public string PaymentOptionName { get; set; }

    public string PaymentOptionDisplayName { get; set; }

    public string PaymentOptionDescription { get; set; }
}
