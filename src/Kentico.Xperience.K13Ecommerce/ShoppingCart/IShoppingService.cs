using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

/// <summary>
/// Provides interface for integration with shopping cart K13 (communicate via store api)
/// </summary>
public interface IShoppingService
{
    /// <summary>
    /// Get current cart content content
    /// </summary>
    /// <returns></returns>
    Task<KShoppingCartContent> GetCurrentShoppingCartContent();


    /// <summary>
    /// Get current cart details
    /// </summary>
    /// <returns></returns>
    Task<KShoppingCartDetails> GetCurrentShoppingCartDetails();


    /// <summary>
    /// Get current cart summary (cart content + details)
    /// </summary>
    /// <returns></returns>
    Task<KShoppingCartSummary> GetCurrentShoppingCartSummaryAsync();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="skuId"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    Task<KShoppingCartItem?> AddItemToCart(int skuId, int quantity);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    Task UpdateItemQuantity(int itemId, int quantity);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    Task RemoveItemFromCart(int itemId);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="couponCode"></param>
    /// <returns></returns>
    Task<bool> AddCouponCode(string couponCode);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="couponCode"></param>
    /// <returns></returns>
    Task RemoveCouponCode(string couponCode);


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<KCustomer?> GetCurrentCustomer();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    Task SetCustomer(KCustomer customer);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="shippingOptionId"></param>
    /// <returns></returns>
    Task SetShippingOption(int shippingOptionId);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="paymentOptionId"></param>
    /// <returns></returns>
    Task SetPaymentOption(int paymentOptionId);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="shippingOptionId"></param>
    /// <param name="paymentOptionId"></param>
    /// <returns></returns>
    Task SetShippingAndPayment(int shippingOptionId, int paymentOptionId);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="billingAddress"></param>
    /// <returns></returns>
    Task SetBillingAddress(KAddress billingAddress);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="shippingAddress"></param>
    /// <returns></returns>
    Task SetShippingAddress(KAddress shippingAddress);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="deliveryDetails"></param>
    /// <returns></returns>
    Task SetDeliveryDetails(KShoppingCartDeliveryDetails deliveryDetails);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    Task<KOrder> CreateOrder(string? note = null);


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<KCustomer?> GetCustomerOrCreateFromAuthenticatedUser(KCustomer? customer = null);


    /// <summary>
    /// 
    /// </summary>
    void ClearCaches();


    /// <summary>
    /// Validates shopping cart items and returns collection of validation messages
    /// </summary>
    /// <returns></returns>
    Task<ICollection<KShoppingCartItemValidationError>> ValidateShoppingCartItems();
}
