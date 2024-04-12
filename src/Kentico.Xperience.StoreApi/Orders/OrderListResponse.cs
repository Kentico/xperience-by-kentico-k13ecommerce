namespace Kentico.Xperience.StoreApi.Orders;

/// <summary>
/// Request for order response
/// </summary>
public class OrderListResponse
{
    public IEnumerable<KOrder> Orders { get; set; }

    public int Page { get; set; }

    public int MaxPage { get; set; }
}
