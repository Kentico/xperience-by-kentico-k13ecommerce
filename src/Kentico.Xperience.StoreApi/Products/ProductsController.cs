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
/// Controller for product related api endpoints.
/// </summary>
[ApiController]
[AuthorizeStore]
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
    /// Test request.
    /// </summary>
    [HttpGet("test")]
    public IActionResult Test() => Ok("test");


    /// <summary>
    /// Returns product pages based on parameters.
    /// </summary>
    /// <param name="request">Product pages request.</param>    
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
    /// Returns all product categories translated for given culture.
    /// </summary>
    /// <param name="culture">Culture.</param>
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

    /// <summary>
    /// Returns prices for product and it's variant when <paramref name="productSkuId"/> is main product or only variant prices when ID is for variant.
    /// </summary>
    /// <param name="productSkuId">Main product or variant id.</param>
    /// <param name="currencyCode">Currency code.</param>
    /// <returns></returns>
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


    /// <summary>
    /// Returns prices for list of products.
    /// </summary>
    /// <param name="productSkuId">Product or variant IDs.</param>
    /// <param name="currencyCode">Currency code.</param>
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


    /// <summary>
    /// Returns inventory and price for given product/variant,
    /// </summary>
    /// <param name="skuId">SKU ID.</param>
    /// <param name="currencyCode">Currency code.</param>
    /// <returns></returns>
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
