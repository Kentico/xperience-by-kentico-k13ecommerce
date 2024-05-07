namespace Kentico.Xperience.StoreApi.Orders;

/// <summary>
/// Dto for <see cref="CMS.Ecommerce.OrderStatusInfo"/>.
/// </summary>
public class KOrderStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; }

    public string StatusDisplayName { get; set; }
}
