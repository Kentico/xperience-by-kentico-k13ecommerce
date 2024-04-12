using CMS.Helpers;
using CMS.Membership;

using Kentico.Xperience.K13Ecommerce.Activities;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Http;

namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

internal class ShoppingService(
    IShoppingCartClientStorage clientStorage,
    IShoppingCartSessionStorage sessionStorage,
    IKenticoStoreApiClient storeApiClient,
    IHttpContextAccessor httpContextAccessor,
    IMemberInfoProvider memberInfoProvider,
    IProgressiveCache progressiveCache,
    IEcommerceActivityLogger activityLogger) : IShoppingService
{
    private const int CacheMinutes = 2;

    public async Task<KShoppingCartContent> GetCurrentShoppingCartContent()
    {
        //when cart guid retrieved from client, call with anonymize parameter to ensure same security as K13 e-comm
        //in current PoC solution this issue does not have priority

        return await progressiveCache.LoadAsync(async cs =>
        {
            var res = await ProcessAction(async () =>
                await storeApiClient.GetCurrentCartContentAsync(ShoppingCartGuid), clearCaches: false);
            cs.BoolCondition = ShoppingCartGuid != Guid.Empty;
            return res;
        }, new CacheSettings(CacheMinutes, nameof(GetCurrentShoppingCartContent), ShoppingCartGuid));
    }

    public async Task<KShoppingCartDetails> GetCurrentShoppingCartDetails()
        => await ProcessAction(async () => await storeApiClient.GetCurrentCartDetailsAsync(ShoppingCartGuid),
            clearCaches: false);


    public async Task<KShoppingCartSummary> GetCurrentShoppingCartSummaryAsync()
    {
        if (ShoppingCartGuid == Guid.Empty)
        {
            throw new InvalidOperationException("Shopping cart identifier is empty");
        }

        var response = await storeApiClient.GetCurrentCartSummaryAsync(ShoppingCartGuid);
        StoreCart(response.CartContent?.ShoppingCartGuid);
        return response;
    }


    public async Task<KCustomer?> GetCurrentCustomer()
        => (await ProcessAction(async () => await storeApiClient.GetCurrentCustomerAsync(ShoppingCartGuid),
            clearCaches: false)).Value;


    public async Task SetCustomer(KCustomer customer)
    {
        if (ShoppingCartGuid == Guid.Empty)
        {
            throw new InvalidOperationException("Shopping cart identifier is empty");
        }

        await storeApiClient.SetCustomerAsync(ShoppingCartGuid, customer);
    }


    public async Task<KShoppingCartItem?> AddItemToCart(int skuId, int quantity)
        => (await ProcessAction(async () =>
        {
            var response = await storeApiClient.AddItemToCartAsync(ShoppingCartGuid, skuId, quantity);

            activityLogger.LogProductAddedToShoppingCartActivity(response.Value.ProductSKU,
                response.Value.CartItemUnits, response.Value.VariantSKU);
            return response;
        })).Value;


    public async Task UpdateItemQuantity(int itemId, int quantity)
        => await ProcessAction(async () =>
            {
                var response = await storeApiClient.UpdateItemQuantityAsync(ShoppingCartGuid, itemId, quantity);

                var cart = await GetCurrentShoppingCartContent();
                var cartItem = cart.CartProducts!.FirstOrDefault(p => p.CartItemId == itemId);
                if (cartItem == null)
                {
                    return response;
                }

                if (cartItem.CartItemUnits > quantity)
                {
                    activityLogger.LogProductRemovedFromShoppingCartActivity(cartItem.ProductSKU,
                        cartItem.CartItemUnits - quantity,
                        cartItem.VariantSKU);
                }
                else
                {
                    activityLogger.LogProductAddedToShoppingCartActivity(cartItem.ProductSKU,
                        quantity - cartItem.CartItemUnits, cartItem.VariantSKU);
                }


                return response;
            }
            , cartMustBeStored: true);


    public async Task RemoveItemFromCart(int itemId)
        => await ProcessAction(async () =>
            {
                var cart = await GetCurrentShoppingCartContent();
                var response = await storeApiClient.RemoveItemFromCartAsync(ShoppingCartGuid, itemId);

                var cartItem = cart.CartProducts!.FirstOrDefault(p => p.CartItemId == itemId);
                if (cartItem != null)
                {
                    activityLogger.LogProductRemovedFromShoppingCartActivity(cartItem.ProductSKU,
                        cartItem.CartItemUnits,
                        cartItem.VariantSKU);
                }

                return response;
            },
            cartMustBeStored: true);


    public async Task<bool> AddCouponCode(string couponCode)
        => (await ProcessAction(async () => await storeApiClient.AddCouponCodeAsync(couponCode, ShoppingCartGuid)))
            .Value;


    public async Task RemoveCouponCode(string couponCode)
        => await ProcessAction(async () => await storeApiClient.RemoveCouponCodeAsync(couponCode, ShoppingCartGuid));


    public async Task SetShippingOption(int shippingOptionId)
        => await ProcessAction(
            async () => await storeApiClient.SetShippingOptionAsync(ShoppingCartGuid, shippingOptionId),
            cartMustBeStored: true);


    public async Task SetPaymentOption(int paymentOptionId)
        => await ProcessAction(
            async () => await storeApiClient.SetPaymentOptionAsync(ShoppingCartGuid, paymentOptionId),
            cartMustBeStored: true);


    public async Task SetShippingAndPayment(int shippingOptionId, int paymentOptionId)
        => await ProcessAction(
            async () => await storeApiClient.SetShippingAndPaymentAsync(ShoppingCartGuid, shippingOptionId,
                paymentOptionId), cartMustBeStored: true);


    public async Task SetBillingAddress(KAddress billingAddress)
        => await ProcessAction(
            async () => await storeApiClient.SetBillingAddressAsync(ShoppingCartGuid, billingAddress),
            cartMustBeStored: true);


    public async Task SetShippingAddress(KAddress shippingAddress)
        => await ProcessAction(
            async () => await storeApiClient.SetShippingAddressAsync(ShoppingCartGuid, shippingAddress),
            cartMustBeStored: true);


    public async Task SetDeliveryDetails(KShoppingCartDeliveryDetails deliveryDetails)
        => await ProcessAction(
            async () => await storeApiClient.SetDeliveryDetailsAsync(ShoppingCartGuid, deliveryDetails),
            cartMustBeStored: true);


    public async Task<KOrder> CreateOrder(string? note = null)
    {
        if (ShoppingCartGuid == Guid.Empty)
        {
            throw new InvalidOperationException("Shopping cart identifier is empty");
        }

        var cart = await GetCurrentShoppingCartContent();
        var order = await storeApiClient.CreateOrderAsync(ShoppingCartGuid, note: note ?? string.Empty);

        sessionStorage.ClearCartGuid();
        clientStorage.ClearCartGuid();

        foreach (var cartItem in cart.CartProducts!)
        {
            activityLogger.LogPurchasedProductActivity(cartItem.ProductSKU, cartItem.CartItemUnits, cartItem.VariantSKU);
        }

        activityLogger.LogPurchaseActivity(order.OrderId, order.OrderGrandTotalInMainCurrency,
            string.Format(cart.Currency.CurrencyFormatString!, order.OrderGrandTotalInMainCurrency));

        return order;
    }


    public async Task<KCustomer?> GetCustomerOrCreateFromAuthenticatedUser()
    {
        var cartDetails = await GetCurrentShoppingCartDetails();

        if (cartDetails.Customer != null)
        {
            return cartDetails.Customer;
        }


        var user = httpContextAccessor.HttpContext?.User?.Identity;

        if (user is not { IsAuthenticated: true })
        {
            return null;
        }

        var member = (await memberInfoProvider.Get().TopN(1)
            .WhereEquals(nameof(MemberInfo.MemberName), user.Name)
            .GetEnumerableTypedResultAsync()).FirstOrDefault();
        if (member != null)
        {
            return new KCustomer { CustomerEmail = member.MemberEmail };
        }

        return null;
    }


    public void ClearCaches() => CacheHelper.Remove(CacheHelper.GetCacheItemName(null,
        nameof(GetCurrentShoppingCartContent),
        ShoppingCartGuid));


    protected virtual async Task<TResponse> ProcessAction<TResponse>(Func<Task<TResponse>> func,
        bool cartMustBeStored = false, bool clearCaches = true)
        where TResponse : IShoppingCartIdentifier
    {
        if (cartMustBeStored && ShoppingCartGuid == Guid.Empty)
        {
            throw new InvalidOperationException("Shopping cart identifier is empty");
        }

        var response = await func();
        if (response.ShoppingCartGuid == Guid.Empty && ShoppingCartGuid != Guid.Empty)
        {
            sessionStorage.ClearCartGuid();
            clientStorage.ClearCartGuid();
        }

        StoreCart(response.ShoppingCartGuid);

        if (clearCaches)
        {
            CacheHelper.Remove(CacheHelper.GetCacheItemName(null, nameof(GetCurrentShoppingCartContent),
                ShoppingCartGuid));
        }

        return response;
    }


    private Guid ShoppingCartGuid
    {
        get
        {
            var cartGuid = sessionStorage.GetCartGuid();

            if (cartGuid == Guid.Empty)
            {
                cartGuid = clientStorage.GetCartGuid();
            }

            return cartGuid;
        }
    }


    private void StoreCart(Guid? shoppingCartGuid)
    {
        if (!shoppingCartGuid.HasValue || shoppingCartGuid == Guid.Empty)
        {
            return;
        }

        clientStorage.SetCartGuid(shoppingCartGuid.Value);
        sessionStorage.SetCartGuid(shoppingCartGuid.Value);
    }
}
