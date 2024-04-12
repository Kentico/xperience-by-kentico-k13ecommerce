namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Partial class for shopping cart details extension
/// </summary>
public partial class KShoppingCartDetails
{
    public KShippingOption? ShippingOption => AvailableShippingOptions?.FirstOrDefault(
        c => c.ShippingOptionId == ShippingOptionId);


    public KPaymentOption? PaymentOption => AvailablePaymentOptions?.FirstOrDefault(
        c => c.PaymentOptionId == PaymentOptionId);
}
