namespace Kentico.Xperience.StoreApi.ShoppingCart;

/// <summary>
/// Model encapsulating all shopping cart data (content with details).
/// </summary>
public class KShoppingCartSummary
{
    public KShoppingCartContent CartContent { get; set; }

    public KShoppingCartDetails CartDetails { get; set; }
}
