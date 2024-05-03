namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

/// <summary>
/// Interface for storing shopping cart GUID in session.
/// </summary>
public interface IShoppingCartSessionStorage
{
    /// <summary>
    /// Gets the GUID of the Shoppin cart stored in session.
    /// </summary>
    /// <returns>Shopping cart GUID from session or System.Guid.Empty when not found.</returns>
    Guid GetCartGuid();


    /// <summary>
    /// Stores the GUID of the Shopping cart to the session.
    /// </summary>
    /// <param name="cartGuid">GUID of the shopping cart to be stored to the session.</param>
    /// <remarks>It is recommended to save the value to the session only if the value has changed.</remarks>
    void SetCartGuid(Guid cartGuid);


    /// <summary>
    /// Delete cart guid from session.
    /// </summary>
    void ClearCartGuid();
}
