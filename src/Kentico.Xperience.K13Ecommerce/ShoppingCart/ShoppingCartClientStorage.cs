using CMS.Core;

using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Http;

namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

internal class ShoppingCartClientStorage
    (ICookieAccessor cookieAccessor, IConversionService conversionService) : IShoppingCartClientStorage
{
    public const string ShoppingCartKey = "ShoppingCartGUID";


    /// <inheritdoc/>
    public Guid GetCartGuid() => conversionService.GetGuid(cookieAccessor.Get(ShoppingCartKey), Guid.Empty);


    /// <inheritdoc/>
    public void SetCartGuid(Guid cartGuid) =>
        cookieAccessor.Set(ShoppingCartKey, cartGuid.ToString(),
            new CookieOptions
            {
                IsEssential = true,
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddMonths(1),
                SameSite = SameSiteMode.Strict
            });


    /// <inheritdoc/>
    public void ClearCartGuid() => cookieAccessor.Remove(ShoppingCartKey);
}
