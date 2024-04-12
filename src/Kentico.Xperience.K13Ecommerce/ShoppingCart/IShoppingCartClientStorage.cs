namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

//
// Summary:
//     Interface for storing shopping cart GUID on the client's side.
public interface IShoppingCartClientStorage
{
    //
    // Summary:
    //     Gets the GUID of the CMS.Ecommerce.ShoppingCartInfo stored on client.
    //
    // Returns:
    //     Shopping cart GUID from client or System.Guid.Empty when not found.
    Guid GetCartGuid();


    //
    // Summary:
    //     Stores the GUID of the CMS.Ecommerce.ShoppingCartInfo to the client.
    //
    // Parameters:
    //   cartGuid:
    //     GUID of the shopping cart to be stored on the client's side.
    //
    // Remarks:
    //     It is recommended to save the value to the client only if the value has changed.
    void SetCartGuid(Guid cartGuid);


    /// <summary>
    /// Clear cart guid from client
    /// </summary>
    void ClearCartGuid();
}
