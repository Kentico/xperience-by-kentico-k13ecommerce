using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

/// <summary>
/// Provides interface for integration with shopping cart on K13 (communicates via store api).
/// </summary>
public interface IShoppingService
{
    /// <summary>
    /// Get current cart content content.
    /// </summary>
    Task<KShoppingCartContent> GetCurrentShoppingCartContent();


    /// <summary>
    /// Get current cart details.
    /// </summary>
    Task<KShoppingCartDetails> GetCurrentShoppingCartDetails();


    /// <summary>
    /// Get current cart summary (cart content + details).
    /// </summary>
    Task<KShoppingCartSummary> GetCurrentShoppingCartSummary();


    /// <summary>
    /// Add item to cart.
    /// </summary>
    /// <param name="skuId">The SKU ID of the item to add.</param>
    /// <param name="quantity">The quantity of the item to add.</param>
    Task<KShoppingCartItem?> AddItemToCart(int skuId, int quantity);


    /// <summary>
    /// Update item quantity in cart.
    /// </summary>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <param name="quantity">The new quantity of the item.</param>
    Task UpdateItemQuantity(int itemId, int quantity);


    /// <summary>
    /// Remove item from cart.
    /// </summary>
    /// <param name="itemId">The ID of the item to remove.</param>
    Task RemoveItemFromCart(int itemId);


    /// <summary>
    /// Add coupon code to cart.
    /// </summary>
    /// <param name="couponCode">The coupon code to add.</param>
    /// <returns>True when coupon code has been added else False.</returns>
    Task<bool> AddCouponCode(string couponCode);


    /// <summary>
    /// Remove coupon code from cart.
    /// </summary>
    /// <param name="couponCode">The coupon code to remove.</param>
    Task RemoveCouponCode(string couponCode);


    /// <summary>
    /// Get current customer.
    /// </summary>
    Task<KCustomer?> GetCurrentCustomer();


    /// <summary>
    /// Set customer to cart.
    /// </summary>
    /// <param name="customer">The customer to set.</param>
    Task SetCustomer(KCustomer customer);


    /// <summary>
    /// Set shipping option to cart.
    /// </summary>
    /// <param name="shippingOptionId">The ID of the shipping option to set.</param>
    Task SetShippingOption(int shippingOptionId);


    /// <summary>
    /// Set payment option to cart.
    /// </summary>
    /// <param name="paymentOptionId">The ID of the payment option to set.</param>
    Task SetPaymentOption(int paymentOptionId);


    /// <summary>
    /// Set shipping and payment option to cart.
    /// </summary>
    /// <param name="shippingOptionId">The ID of the shipping option to set.</param>
    /// <param name="paymentOptionId">The ID of the payment option to set.</param>
    /// <returns></returns>
    Task SetShippingAndPayment(int shippingOptionId, int paymentOptionId);


    /// <summary>
    /// Set billing address to cart.
    /// </summary>
    /// <param name="billingAddress">Billing address.</param>
    Task SetBillingAddress(KAddress billingAddress);


    /// <summary>
    /// Set shipping address to cart.
    /// </summary>
    /// <param name="shippingAddress">Shipping address.</param>
    Task SetShippingAddress(KAddress shippingAddress);


    /// <summary>
    /// Set delivery details (customer, shipping address, billing address, shipping option, payment option) to cart.
    /// </summary>
    /// <param name="deliveryDetails">Delivery details to set.</param>
    Task SetDeliveryDetails(KShoppingCartDeliveryDetails deliveryDetails);


    /// <summary>
    /// Create order from cart.
    /// </summary>
    /// <param name="note">Order note.</param>
    Task<KOrder> CreateOrder(string? note = null);


    /// <summary>
    /// Get customer from authenticated user or create new customer.
    /// </summary>
    /// <param name="customer">Existing customer.</param>    
    Task<KCustomer?> GetCustomerOrCreateFromAuthenticatedUser(KCustomer? customer = null);


    /// <summary>
    /// Clear caches.
    /// </summary>
    void ClearCaches();


    /// <summary>
    /// Validates shopping cart items and returns collection of validation messages.
    /// </summary>
    Task<ICollection<KShoppingCartItemValidationError>> ValidateShoppingCartItems();


    /// <summary>
    /// Set currency to cart when currency is different than previous.
    /// </summary>
    /// <param name="currencyCode">Currency code.</param> 
    Task SetCurrency(string currencyCode);
}
