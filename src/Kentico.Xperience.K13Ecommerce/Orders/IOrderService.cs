using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Orders;

public interface IOrderService
{
    /// <summary>
    /// Get orders based on parameters
    /// </summary>
    /// <param name="request">Request parameters for order listing</param>
    /// <returns>Paged list of orders</returns>
    Task<OrderListResponse> GetOrderList(OrderListRequest request);
}
