using System.Net.Mime;

using CMS.Base;
using CMS.Ecommerce;
using CMS.SiteProvider;

using Kentico.Xperience.StoreApi.Routing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.StoreSite;

/// <summary>
/// API endpoints for site on global level
/// </summary>
[Route($"{ApiRoute.ApiPrefix}/site")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
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
    [HttpGet("cultures", Name = nameof(GetCultures))]
    public IEnumerable<KCulture> GetCultures()
        => CultureSiteInfoProvider.GetSiteCultures(siteService.CurrentSite.SiteName)
            .Select(c => new KCulture { CultureName = c.CultureName, CultureCode = c.CultureCode });


    /// <summary>
    /// Returns all enabled site currencies
    /// </summary>
    /// <returns></returns>
    [HttpGet("currencies", Name = nameof(GetCurrencies))]
    public async Task<IEnumerable<string>> GetCurrencies()
        => (await CurrencyInfoProvider.GetCurrencies(siteService.CurrentSite.SiteID).GetEnumerableTypedResultAsync())
            .Select(c => c.CurrencyCode);
}
