using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using CMS.SiteProvider;

using Kentico.Xperience.StoreApi.Authentication;
using Kentico.Xperience.StoreApi.Currencies;
using Kentico.Xperience.StoreApi.Products.Categories;
using Kentico.Xperience.StoreApi.Products.Pages;
using Kentico.Xperience.StoreApi.Products.Prices;
using Kentico.Xperience.StoreApi.Routing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.Products;

/// <summary>
/// Controller for product related api endpoints
/// </summary>
[ApiController]
[Route($"{ApiRoute.ApiPrefix}/products")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// <param name="request"></param>    
    /// <returns></returns>
    [HttpGet("pages/listing", Name = nameof(GetProductPages))]
    public async Task<ActionResult<IEnumerable<KProductNode>>> GetProductPages(
        [FromQuery] ProductPageRequest request)
    {
        if (request.Culture is not null &&
            !CultureSiteInfoProvider.IsCultureOnSite(request.Culture, SiteContext.CurrentSiteName))
        {
            return BadRequest($"Culture '{request.Culture}' is not assigned for site");
        }

        var productPages = await productService.GetProductPages(request);

        return Ok(productPages);
    }

    /// <summary>
    /// Returns all product categories translated for given culture
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    [HttpGet("categories", Name = nameof(GetProductCategories))]
    public async Task<ActionResult<IEnumerable<KProductCategory>>> GetProductCategories(
        [FromQuery] [RegularExpression("[a-zA-Z]{2}-[a-zA-Z]{2}")]
        string culture)
    {
        if (culture is not null && !CultureSiteInfoProvider.IsCultureOnSite(culture, SiteContext.CurrentSiteName))
        {
            return BadRequest($"Culture '{culture}' is not assigned for site");
        }

        var categories = await productService.GetProductCategories(culture);
        return Ok(categories);
    }

    [HttpGet("prices/{productSkuId:int}", Name = nameof(GetProductPrices))]
    [AuthorizeStore]
    public async Task<ActionResult<ProductPricesResponse>> GetProductPrices(int productSkuId,
        [FromQuery][CurrencyValidation] string currencyCode)
    {
        try
        {
            var prices = await productService.GetProductPrices(productSkuId, currencyCode);
            return Ok(prices);
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return ValidationProblem();
        }
    }

    [HttpGet("prices-list", Name = nameof(GetProductPricesList))]
    [AuthorizeStore]
    public ActionResult<IAsyncEnumerable<ProductPricesResponse>> GetProductPricesList(
        [FromQuery] ICollection<int> productSkuId, [FromQuery][CurrencyValidation] string currencyCode)
    {
        try
        {
            var prices = productService.GetProductPrices(productSkuId, currencyCode);
            return Ok(prices);
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return ValidationProblem();
        }
    }

    [HttpGet("inventory-prices/{skuId:int}", Name = nameof(GetInventoryPrices))]
    [AuthorizeStore]
    public async Task<ActionResult<ProductInventoryPriceInfo>> GetInventoryPrices(int skuId, [FromQuery][CurrencyValidation] string currencyCode)
    {
        try
        {
            var prices = await productService.GetProductInventoryAndPrices(skuId, currencyCode);
            return Ok(prices);
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return ValidationProblem();
        }
    }
}
