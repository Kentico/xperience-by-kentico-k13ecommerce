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

    public CustomerController(IAddressInfoProvider addressInfoProvider, IMapper mapper)
    {
        this.addressInfoProvider = addressInfoProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Endpoint for customer addresses
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpGet("addresses", Name = nameof(CustomerAddresses))]
    public ActionResult<IEnumerable<KAddress>> CustomerAddresses([FromQuery][Required] int customerId)
    {
        var addresses = mapper.Map<IEnumerable<KAddress>>(addressInfoProvider.GetByCustomer(customerId));
        return Ok(addresses);
    }
}
