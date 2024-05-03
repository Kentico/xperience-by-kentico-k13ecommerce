using System.ComponentModel.DataAnnotations;

using Kentico.Xperience.StoreApi.Customers;

namespace Kentico.Xperience.StoreApi.ShoppingCart;

/// <summary>
/// Model representing shopping cart delivery details.
/// </summary>
public class KShoppingCartDeliveryDetails
{
    [Required]
    public KCustomer Customer { get; set; }

    [Required]
    public KAddress BillingAddress { get; set; }

    public KAddress ShippingAddress { get; set; }

    public int ShippingOptionId { get; set; }

    public int PaymentOptionId { get; set; }
}
