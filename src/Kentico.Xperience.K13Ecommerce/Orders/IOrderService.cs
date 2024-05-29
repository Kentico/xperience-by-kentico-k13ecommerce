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
}
