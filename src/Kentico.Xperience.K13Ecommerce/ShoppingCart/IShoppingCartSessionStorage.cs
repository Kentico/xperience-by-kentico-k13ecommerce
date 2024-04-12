namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;
//
// Summary:
//     Interface for storing shopping cart GUID in session.
public interface IShoppingCartSessionStorage
{
    //
    // Summary:
    //     Gets the GUID of the CMS.Ecommerce.ShoppingCartInfo stored in session.
    //
    // Returns:
    //     Shopping cart GUID from session or System.Guid.Empty when not found.
    Guid GetCartGuid();

    //
    // Summary:
    //     Stores the GUID of the CMS.Ecommerce.ShoppingCartInfo to the session.
    //
    // Parameters:
    //   cartGuid:
    //     GUID of the shopping cart to be stored to the session.
    //
    // Remarks:
    //     It is recommended to save the value to the session only if the value has changed.
    void SetCartGuid(Guid cartGuid);


    /// <summary>
    /// Delete cart guid from session
    /// </summary>
    void ClearCartGuid();
}
