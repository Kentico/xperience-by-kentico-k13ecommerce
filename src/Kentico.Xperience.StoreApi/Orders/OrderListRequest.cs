using System.ComponentModel.DataAnnotations;

namespace Kentico.Xperience.StoreApi.Orders;

public class OrderListRequest
{
    public int Page { get; set; }

    [Range(1, 100)]
    public int PageSize { get; set; }

    public string OrderBy { get; set; }
}
