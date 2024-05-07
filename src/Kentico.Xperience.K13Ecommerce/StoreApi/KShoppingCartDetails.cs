namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Partial class for shopping cart details extension.
/// </summary>
public partial class KShoppingCartDetails
{
    /// <summary>
    /// Selected shipping option.
    /// </summary>
    public KShippingOption? ShippingOption => AvailableShippingOptions?.FirstOrDefault(
        c => c.ShippingOptionId == ShippingOptionId);

    /// <summary>
    /// Selected payment option.
    /// </summary>
    public KPaymentOption? PaymentOption => AvailablePaymentOptions?.FirstOrDefault(
        c => c.PaymentOptionId == PaymentOptionId);
}
