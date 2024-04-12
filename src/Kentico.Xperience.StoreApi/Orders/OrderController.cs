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

    public OrderController(IOrderInfoProvider orderInfoProvider, IMapper mapper, ISiteService siteService)
    {
        this.orderInfoProvider = orderInfoProvider;
        this.mapper = mapper;
        this.siteService = siteService;
    }

    /// <summary>
    /// Endpoint for getting list of orders based on request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("list", Name = nameof(OrderList))]
    public async Task<ActionResult<IEnumerable<KOrder>>> OrderList([FromQuery] OrderListRequest request)
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

        var orders = mapper.Map<IEnumerable<KOrder>>(await orderQuery.GetEnumerableTypedResultAsync());

        return Ok(new OrderListResponse
        {
            Orders = orders,
            Page = page + 1,
            MaxPage = (orderQuery.TotalRecords / request.PageSize) + 1
        });
    }
}
