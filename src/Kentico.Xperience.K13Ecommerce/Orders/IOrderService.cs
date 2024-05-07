using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Orders;

/// <summary>
/// Order service.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Get orders based on parameters.
    /// </summary>
    /// <param name="request">Request parameters for order listing.</param>
    /// <returns>Paged list of orders.</returns>
    Task<OrderListResponse> GetOrderList(OrderListRequest request);
    
    
    /// <summary>
    /// Get order by id.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    Task<KOrder> GetOrder(int orderId);
    
    
    /// <summary>
    /// Returns list of enabled order statuses.
    /// </summary>
    Task<ICollection<KOrderStatus>> GetOrderStatuses();

    
    /// <summary>
    /// Updates order.
    /// </summary>
    /// <param name="order">Order Dto, send full data for order - retrive order data first.</param>
    Task UpdateOrder(KOrder order);
}
