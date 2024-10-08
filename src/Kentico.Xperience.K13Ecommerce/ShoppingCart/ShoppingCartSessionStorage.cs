﻿using CMS.Core;

using Microsoft.AspNetCore.Http;

namespace Kentico.Xperience.K13Ecommerce.ShoppingCart;

internal class ShoppingCartSessionStorage(
        IHttpContextAccessor httpContextAccessor,
        IConversionService conversionService)
    : IShoppingCartSessionStorage
{
    private const string ShoppingCartKey = "ShoppingCartGUID";

    /// <inheritdoc/>
    public Guid GetCartGuid() =>
        conversionService.GetGuid(httpContextAccessor.HttpContext?.Session.GetString(ShoppingCartKey), Guid.Empty);


    /// <inheritdoc/>
    public void SetCartGuid(Guid cartGuid) =>
        httpContextAccessor.HttpContext?.Session.SetString(ShoppingCartKey, cartGuid.ToString());


    /// <inheritdoc/>
    public void ClearCartGuid() => httpContextAccessor.HttpContext?.Session.Remove(ShoppingCartKey);
}
