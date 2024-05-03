namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Partial class for shopping cart content extension.
/// </summary>
public partial class KShoppingCartContent
{
    /// <summary>
    /// Returns true if the shopping cart is empty.
    /// </summary>
    public bool IsEmpty => CartProducts == null || CartProducts.Count == 0;
}
