using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CMS.SiteProvider;
using Kentico.Xperience.StoreApi.Currencies;
using Kentico.Xperience.StoreApi.Products.Categories;
using Kentico.Xperience.StoreApi.Products.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.Products;

/// <summary>
/// Controller for product related api endpoints
/// </summary>
[ApiController]
[Route("api/store/products")]
public class ProductsController : ControllerBase
{
    private readonly IKProductService productService;

    public ProductsController(IKProductService productService)
    {
        this.productService = productService;
    }

    /// <summary>
    /// Test request
    /// </summary>
    /// <returns></returns>
    [HttpGet("test")]
    public IActionResult Test() => Ok("test");

    /// <summary>
    /// Returns product pages based on parameters
    /// </summary>
    /// <param name="path"></param>
    /// <param name="culture"></param>
    /// <param name="currency"></param>
    /// <param name="orderBy"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [HttpGet("pages/listing")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<KProductNode>>> GetProductPages(
        [FromQuery][Required] string path,
        [FromQuery][RegularExpression("[a-zA-Z]{2}-[a-zA-Z]{2}")] string culture,
        [FromQuery][CurrencyValidation] string currency,
        [FromQuery] string orderBy,
        [FromQuery] int limit = 12
    )
    {
        if (culture is not null && !CultureSiteInfoProvider.IsCultureOnSite(culture, SiteContext.CurrentSiteName))
        {
            return BadRequest($"Culture '{culture}' is not assigned for site");
        }

        var productPages = await productService.GetProductPages(path, culture, currency, limit, orderBy);

        return Ok(productPages);
    }

    /// <summary>
    /// Returns all product categories translated for given culture
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<KProductCategory>>> GetProductCategories(
        [FromQuery][RegularExpression("[a-zA-Z]{2}-[a-zA-Z]{2}")] string culture)
    {
        if (culture is not null && !CultureSiteInfoProvider.IsCultureOnSite(culture, SiteContext.CurrentSiteName))
        {
            return BadRequest($"Culture '{culture}' is not assigned for site");
        }

        var categories = await productService.GetProductCategories(culture);
        return Ok(categories);
    }
}
