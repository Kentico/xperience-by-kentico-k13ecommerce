using CMS.Core;

using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Http;

namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

internal class ShoppingCartClientStorage
    (ICookieAccessor cookieAccessor, IConversionService conversionService) : IShoppingCartClientStorage
{
    private const string ShoppingCartKey = "ShoppingCartGUID";


    public Guid GetCartGuid() => conversionService.GetGuid(cookieAccessor.Get(ShoppingCartKey), Guid.Empty);


    public void SetCartGuid(Guid cartGuid) =>
        cookieAccessor.Set(ShoppingCartKey, cartGuid.ToString(),
            new CookieOptions { HttpOnly = true, Expires = DateTimeOffset.Now.AddMonths(1), SameSite = SameSiteMode.Strict });


    public void ClearCartGuid() => cookieAccessor.Remove(ShoppingCartKey);
}
