using System.ComponentModel.DataAnnotations;

namespace Kentico.Xperience.StoreApi.ShoppingCart;

/// <summary>
/// Model representing all shopping cart details
/// </summary>
public class KShoppingCartDetails : KShoppingCartDeliveryDetails
{
    [Required]
    public Guid ShoppingCartGuid { get; set; }

    public string Note { get; set; }

    public bool IsShippingNeeded { get; set; }

    public IEnumerable<KShippingOption> AvailableShippingOptions { get; set; }

    public IEnumerable<KPaymentOption> AvailablePaymentOptions { get; set; }

    public Dictionary<string, object> CustomData { get; set; }
}
