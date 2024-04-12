using System.IdentityModel.Tokens.Jwt;

using CMS;
using CMS.Ecommerce;
using CMS.Helpers;

using Kentico.Xperience.StoreApi.Routing;
using Kentico.Xperience.StoreApi.ShoppingCart;

using Microsoft.AspNetCore.Http;

[assembly: RegisterImplementation(typeof(IShoppingCartCache), typeof(StoreApiShoppingCartCache))]
namespace Kentico.Xperience.StoreApi.ShoppingCart;

public class StoreApiShoppingCartCache : IShoppingCartCache
{
    private const string ShoppingCartCacheKey = "CurrentShoppingCart";
    private readonly IShoppingCartCache defaultCache;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CMS.Ecommerce.ShoppingCartCache" /> class.
    /// </summary>
    /// <param name="defaultCache"></param>
    /// <param name="httpContextAccessor">The HTTP context retriever.</param>
    public StoreApiShoppingCartCache(IShoppingCartCache defaultCache, IHttpContextAccessor httpContextAccessor)
    {
        this.defaultCache = defaultCache;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>Returns cached shopping cart of the current visitor.</summary>
    /// <remarks>Returns null if session does not exist.</remarks>
    public ShoppingCartInfo GetCart()
    {
        if (!IsStoreApiRequest)
        {
            return defaultCache.GetCart();
        }

        if (httpContextAccessor.HttpContext == null)
        {
            return null;
        }

        string keyName = GetKeyName();
        if (keyName is null)
        {
            return null;
        }

        return CacheHelper.TryGetItem<ShoppingCartInfo>(GetKeyName(), out var cart) ? cart : null;
    }

    /// <summary>
    /// Stores current visitor's <see cref="T:CMS.Ecommerce.ShoppingCartInfo" /> to the cache.
    /// </summary>
    /// <param name="cart">Shopping cart to be stored.</param>
    /// <remarks>Does not store the shopping cart if session does not exist.</remarks>
    public void StoreCart(ShoppingCartInfo cart)
    {
        if (!IsStoreApiRequest)
        {
            defaultCache.StoreCart(cart);
            return;
        }

        if (httpContextAccessor.HttpContext == null)
        {
            return;
        }

        string keyName = GetKeyName();
        if (keyName is null)
        {
            return;
        }
        var cacheDependency = CacheHelper.GetCacheDependency(cart != null
            ? string.Format("{0}|byid|{1}", "ecommerce.shoppingcart", cart.ShoppingCartID)
            : null);
        var slidingExpiration = TimeSpan.FromMinutes(SessionHelper.SessionTimeout);
        CacheHelper.Add(keyName, cart, cacheDependency, CacheConstants.NoAbsoluteExpiration, slidingExpiration);
    }

    /// <summary>
    /// Customized: get cache key name from Jti claim (token identifier). Cache can be accessed until current token is valid.
    /// </summary>
    /// <returns></returns>
    private string GetKeyName()
    {
        string cacheKey =
            httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)
                ?.Value;
        if (string.IsNullOrEmpty(cacheKey))
        {
            return null;
        }

        return CacheHelper.GetCacheItemName(null, ShoppingCartCacheKey, cacheKey);
    }

    private bool IsStoreApiRequest =>
        httpContextAccessor.HttpContext?.Request.Path.StartsWithSegments("/" + ApiRoute.ApiPrefix) ?? false;
}
