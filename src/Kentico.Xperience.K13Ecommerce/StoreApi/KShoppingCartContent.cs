namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Partial class for shopping cart content extension
/// </summary>
public partial class KShoppingCartContent
{
    public bool IsEmpty => CartProducts == null || CartProducts.Count == 0;
}
