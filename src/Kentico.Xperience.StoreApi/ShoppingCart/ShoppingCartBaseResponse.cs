namespace Kentico.Xperience.StoreApi.ShoppingCart;

public class ShoppingCartBaseResponse
{
    public Guid ShoppingCartGuid { get; set; }
}

public class ShoppingCartResponse<TModel> : ShoppingCartBaseResponse
{
    public TModel Value { get; set; }
}
