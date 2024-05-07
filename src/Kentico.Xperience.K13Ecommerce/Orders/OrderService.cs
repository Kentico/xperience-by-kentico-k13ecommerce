using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Orders;

internal class OrderService(IKenticoStoreApiClient storeApiClient) : IOrderService
{
    /// <inheritdoc/>
    public async Task<OrderListResponse> GetOrderList(OrderListRequest request)
        => await storeApiClient.OrderListAsync(request.Page, request.PageSize, request.OrderBy);

    /// <inheritdoc/>
    public async Task<KOrder> GetOrder(int orderId) => await storeApiClient.OrderDetailAsync(orderId);

    /// <inheritdoc/>
    public async Task<ICollection<KOrderStatus>> GetOrderStatuses() => await storeApiClient.OrderStatusesListAsync();

    /// <inheritdoc/>
    public async Task UpdateOrder(KOrder order) => await storeApiClient.UpdateOrderAsync(order);
}
