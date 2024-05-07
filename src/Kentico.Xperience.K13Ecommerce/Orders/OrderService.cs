using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Orders;

internal class OrderService(IKenticoStoreApiClient storeApiClient) : IOrderService
{
    /// <inheritdoc/>
    public async Task<OrderListResponse> GetOrderList(OrderListRequest request)
        => await storeApiClient.OrderListAsync(request.Page, request.PageSize, request.OrderBy);
}
