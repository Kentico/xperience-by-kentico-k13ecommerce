using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using AutoMapper;

using CMS.Ecommerce;

using Kentico.Xperience.StoreApi.Authentication;
using Kentico.Xperience.StoreApi.Routing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.Customers;

[ApiController]
[AuthorizeStore]
[Route($"{ApiRoute.ApiPrefix}/customer")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class CustomerController : ControllerBase
{
    private readonly IAddressInfoProvider addressInfoProvider;
    private readonly IMapper mapper;
    private readonly IShoppingService shoppingService;

    public CustomerController(IAddressInfoProvider addressInfoProvider, IMapper mapper, IShoppingService shoppingService)
    {
        this.addressInfoProvider = addressInfoProvider;
        this.mapper = mapper;
        this.shoppingService = shoppingService;
    }

    /// <summary>
    /// Endpoint for current customer addresses.
    /// </summary>
    /// <returns></returns>
    [HttpGet("addresses", Name = nameof(CurrentCustomerAddresses))]
    public ActionResult<IEnumerable<KAddress>> CurrentCustomerAddresses()
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        if (cart.Customer is null)
        {
            return Ok(Enumerable.Empty<KAddress>());
        }
        var addresses = mapper.Map<IEnumerable<KAddress>>(addressInfoProvider.GetByCustomer(cart.Customer.CustomerID));
        return Ok(addresses);
    }


    /// <summary>
    /// Endpoint for given customer addresses to display in XbyK administration.
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns></returns>
    [HttpGet("admin/addresses", Name = nameof(AdminCustomerAddresses))]
    public ActionResult<IEnumerable<KAddress>> AdminCustomerAddresses([FromQuery][Required] int customerId)
    {
        var addresses = mapper.Map<IEnumerable<KAddress>>(addressInfoProvider.GetByCustomer(customerId));
        return Ok(addresses);
    }
}
