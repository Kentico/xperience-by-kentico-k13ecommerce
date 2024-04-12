namespace Kentico.Xperience.StoreApi.ShoppingCart;

/// <summary>
/// Base shopping cart response for Store API
/// </summary>
public class ShoppingCartBaseResponse
{
    public Guid ShoppingCartGuid { get; set; }
}

/// <summary>
/// Shopping cart API response with model
/// </summary>
/// <typeparam name="TModel">Cart's model</typeparam>
public class ShoppingCartResponse<TModel> : ShoppingCartBaseResponse
{
    public TModel Value { get; set; }
}
