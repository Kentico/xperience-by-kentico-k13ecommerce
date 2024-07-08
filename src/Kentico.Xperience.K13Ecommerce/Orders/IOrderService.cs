using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Orders;

/// <summary>
/// Order service.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Get orders based on parameters for current customer.
    /// </summary>
    /// <param name="request">Request parameters for order listing.</param>
    /// <returns>Paged list of orders.</returns>
    Task<OrderListResponse> GetCurrentCustomerOrderList(OrderListRequest request);


    /// <summary>
    /// Get orders based on parameters to display in XbyK administration (for all customers).
    /// </summary>
    /// <param name="request">Request parameters for order listing.</param>
    /// <returns>Paged list of orders.</returns>
    Task<OrderListResponse> GetAdminOrderList(OrderListRequest request);


    /// <summary>
    /// Get order by id for current customer.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    Task<KOrder> GetCurrentCustomerOrder(int orderId);


    /// <summary>
    /// Get order by ID to display in XbyK administration.
    /// </summary>
    /// <param name="orderId"></param>
    Task<KOrder> GetAdminOrder(int orderId);


    /// <summary>
    /// Returns list of enabled order statuses.
    /// </summary>
    Task<ICollection<KOrderStatus>> GetOrderStatuses();


    /// <summary>
    /// Updates order.
    /// </summary>
    /// <param name="order">Order Dto, send full data for order - retrieve order data first.</param>
    Task UpdateOrder(KOrder order);
}
