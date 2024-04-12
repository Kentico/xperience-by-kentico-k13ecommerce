using System.ComponentModel.DataAnnotations;

namespace Kentico.Xperience.StoreApi.Orders;

/// <summary>
/// Request for order listing
/// </summary>
public class OrderListRequest
{
    public int Page { get; set; }

    [Range(1, 100)]
    public int PageSize { get; set; }

    public string OrderBy { get; set; }
}
