using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Orders;

internal class OrderService(IKenticoStoreApiClient storeApiClient) : IOrderService
{
    /// <inheritdoc/>
    public async Task<OrderListResponse> GetCurrentCustomerOrderList(OrderListRequest request)
        => await storeApiClient.CurrentCustomerOrderListAsync(request.Page, request.PageSize, request.OrderBy);


    ///<inheritdoc/>
    public async Task<OrderListResponse> GetAdminOrderList(OrderListRequest request)
        => await storeApiClient.AdminOrderListAsync(request.Page, request.PageSize, request.OrderBy);


    /// <inheritdoc/>
    public async Task<KOrder> GetCurrentCustomerOrder(int orderId) => await storeApiClient.OrderDetailAsync(orderId);


    /// <inheritdoc/>
    public async Task<KOrder> GetAdminOrder(int orderId) => await storeApiClient.AdminOrderDetailAsync(orderId);


    /// <inheritdoc/>
    public async Task<ICollection<KOrderStatus>> GetOrderStatuses() => await storeApiClient.OrderStatusesListAsync();


    /// <inheritdoc/>
    public async Task UpdateOrder(KOrder order) => await storeApiClient.UpdateOrderAsync(order);
}
