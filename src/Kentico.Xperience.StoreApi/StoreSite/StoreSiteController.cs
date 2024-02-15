using CMS.Base;
using CMS.Ecommerce;
using CMS.SiteProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Kentico.Xperience.StoreApi.StoreSite;

/// <summary>
/// API endpoints for site on global level
/// </summary>
[Route("api/store/site")]
[ApiController]
public class StoreSiteController : ControllerBase
{
    private readonly ISiteService siteService;

    public StoreSiteController(ISiteService siteService)
    {
        this.siteService = siteService;
    }

    /// <summary>
    /// Returns all enabled site cultures
    /// </summary>
    /// <returns></returns>
    [HttpGet("cultures")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<KCulture> GetCultures()
        => CultureSiteInfoProvider.GetSiteCultures(siteService.CurrentSite.SiteName)
            .Select(c => new KCulture { CultureName = c.CultureName, CultureCode = c.CultureCode });


    /// <summary>
    /// Returns all enabled site currencies
    /// </summary>
    /// <returns></returns>
    [HttpGet("currencies")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<string>> GetCurrencies()
        => (await CurrencyInfoProvider.GetCurrencies(siteService.CurrentSite.SiteID).GetEnumerableTypedResultAsync())
            .Select(c => c.CurrencyCode);
}