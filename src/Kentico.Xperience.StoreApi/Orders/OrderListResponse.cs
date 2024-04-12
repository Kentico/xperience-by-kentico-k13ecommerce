namespace Kentico.Xperience.StoreApi.Orders;

public class OrderListResponse
{
    public IEnumerable<KOrder> Orders { get; set; }

    public int Page { get; set; }

    public int MaxPage { get; set; }
}
