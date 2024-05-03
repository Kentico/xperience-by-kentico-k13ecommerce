namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Interface for shopping cart identifier.
/// </summary>
public interface IShoppingCartIdentifier
{
    public Guid? ShoppingCartGuid { get; }
}

public partial class KShoppingCartContent : IShoppingCartIdentifier
{
    Guid? IShoppingCartIdentifier.ShoppingCartGuid => ShoppingCartGuid;
}

public partial class KShoppingCartDetails : IShoppingCartIdentifier
{
    Guid? IShoppingCartIdentifier.ShoppingCartGuid => ShoppingCartGuid;
}

public partial class KCustomerShoppingCartResponse : IShoppingCartIdentifier
{
    Guid? IShoppingCartIdentifier.ShoppingCartGuid => ShoppingCartGuid;
}

public partial class KShoppingCartItemShoppingCartResponse : IShoppingCartIdentifier
{
    Guid? IShoppingCartIdentifier.ShoppingCartGuid => ShoppingCartGuid;
}

public partial class ShoppingCartBaseResponse : IShoppingCartIdentifier
{
    Guid? IShoppingCartIdentifier.ShoppingCartGuid => ShoppingCartGuid;
}

public partial class BooleanShoppingCartResponse : IShoppingCartIdentifier
{
    Guid? IShoppingCartIdentifier.ShoppingCartGuid => ShoppingCartGuid;
}

public partial class KShoppingCartItemValidationErrorIEnumerableShoppingCartResponse : IShoppingCartIdentifier
{
    Guid? IShoppingCartIdentifier.ShoppingCartGuid => ShoppingCartGuid;
}
