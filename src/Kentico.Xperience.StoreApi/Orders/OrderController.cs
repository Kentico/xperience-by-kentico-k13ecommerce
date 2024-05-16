using System.Net.Mime;

using AutoMapper;

using CMS.Base;
using CMS.Ecommerce;

using Kentico.Xperience.StoreApi.Authentication;
using Kentico.Xperience.StoreApi.Routing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.Orders;

[ApiController]
[AuthorizeStore]
[Route($"{ApiRoute.ApiPrefix}/order")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class OrderController : ControllerBase
{
    private readonly IOrderInfoProvider orderInfoProvider;
    private readonly IMapper mapper;
    private readonly ISiteService siteService;
    private readonly IOrderStatusInfoProvider orderStatusInfoProvider;

    public OrderController(
        IOrderInfoProvider orderInfoProvider,
        IMapper mapper,
        ISiteService siteService,
        IOrderStatusInfoProvider orderStatusInfoProvider)
    {
        this.orderInfoProvider = orderInfoProvider;
        this.mapper = mapper;
        this.siteService = siteService;
        this.orderStatusInfoProvider = orderStatusInfoProvider;
    }

    /// <summary>
    /// Endpoint for getting list of orders based on request.
    /// </summary>
    /// <param name="request">Order list request.</param>
    /// <returns></returns>
    [HttpGet("list", Name = nameof(OrderList))]
    public async Task<ActionResult<OrderListResponse>> OrderList([FromQuery] OrderListRequest request)
    {
        int page = request.Page > 0 ? request.Page - 1 : 0;
        if (string.IsNullOrWhiteSpace(request.OrderBy))
        {
            request.OrderBy = $"{nameof(OrderInfo.OrderDate)} DESC";
        }

        var orderQuery = orderInfoProvider.Get()
            .OnSite(siteService.CurrentSite.SiteID)
            .Page(page, request.PageSize)
            .OrderBy(request.OrderBy);

        if (request.CustomerId is > 0)
        {
            orderQuery = orderQuery.WhereEquals(nameof(OrderInfo.OrderCustomerID), request.CustomerId);
        }

        var orders = mapper.Map<IEnumerable<KOrder>>(await orderQuery.GetEnumerableTypedResultAsync());

        return Ok(new OrderListResponse
        {
            Orders = orders,
            Page = page + 1,
            MaxPage = (orderQuery.TotalRecords / request.PageSize) + 1
        });
    }

    /// <summary>
    /// Returns order by ID.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    [HttpGet("detail/{orderId:int}", Name = nameof(OrderDetail))]
    public async Task<ActionResult<KOrder>> OrderDetail([FromRoute] int orderId)
    {
        var order = await orderInfoProvider.GetAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<KOrder>(order));
    }


    /// <summary>
    /// Endpoint for listing order statuses.
    /// </summary>
    /// <returns>List of order statuses.</returns>
    [HttpGet("statuses/list", Name = nameof(OrderStatusesList))]
    public async Task<ActionResult<IEnumerable<KOrderStatus>>> OrderStatusesList()
    {
        int siteId = ECommerceHelper.GetSiteID(siteService.CurrentSite.SiteID, "CMSStoreUseGlobalOrderStatus");

        var orderStatuses = await orderStatusInfoProvider.Get()
            .OnSite(siteId, includeGlobal: siteId == 0)
            .OrderBy(nameof(OrderStatusInfo.StatusOrder))
            .GetEnumerableTypedResultAsync();

        return Ok(mapper.Map<IEnumerable<KOrderStatus>>(orderStatuses));
    }

    /// <summary>
    /// Updates order. Updates all fields on order level and addresses on sub-level. Customer data and order items cannot be updated.
    /// </summary>
    /// <param name="order">Order dto</param>
    [HttpPut("update", Name = nameof(UpdateOrder))]
    public async Task<ActionResult> UpdateOrder([FromBody] KOrder order)
    {
        var orderInfo = await orderInfoProvider.GetAsync(order.OrderId);
        if (orderInfo == null)
        {
            return NotFound();
        }

        orderInfo = mapper.Map(order, orderInfo);

        if (orderInfo.OrderBillingAddress.HasChanged)
        {
            orderInfo.OrderBillingAddress.Update();
        }

        if (orderInfo.OrderShippingAddress.HasChanged)
        {
            orderInfo.OrderShippingAddress.Update();
        }

        orderInfoProvider.Set(orderInfo);

        return Ok();
    }
}
