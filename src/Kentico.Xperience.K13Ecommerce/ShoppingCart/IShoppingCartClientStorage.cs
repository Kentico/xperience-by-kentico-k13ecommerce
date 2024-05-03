namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

/// <summary>
/// Interface for storing shopping cart GUID on the client's side.
/// </summary>
public interface IShoppingCartClientStorage
{
    /// <summary>
    /// Get the GUID of the Shopping cart stored on client.
    /// </summary>
    /// <returns>Shopping cart GUID from client or System.Guid.Empty when not found.</returns>
    Guid GetCartGuid();


    /// <summary>
    /// Stores the GUID of the Shopping cart to the client.
    /// </summary>
    /// <param name="cartGuid">GUID of the shopping cart to be stored on the client's side.</param>
    void SetCartGuid(Guid cartGuid);


    /// <summary>
    /// Clear cart guid from client.
    /// </summary>
    void ClearCartGuid();
}
