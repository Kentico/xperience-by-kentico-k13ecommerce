using System.Net.Mime;

using CMS.Base;
using CMS.Ecommerce;
using CMS.SiteProvider;

using Kentico.Xperience.StoreApi.Authentication;
using Kentico.Xperience.StoreApi.Routing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.StoreSite;

/// <summary>
/// API endpoints for site on global level
/// </summary>
[Route($"{ApiRoute.ApiPrefix}/site")]
[AuthorizeStore]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
public class StoreSiteController : ControllerBase
{
    private readonly ISiteService siteService;
    private readonly ISettingServiceFactory settingServiceFactory;

    public StoreSiteController(ISiteService siteService, ISettingServiceFactory settingServiceFactory)
    {
        this.siteService = siteService;
        this.settingServiceFactory = settingServiceFactory;
    }

    /// <summary>
    /// Returns all enabled site cultures
    /// </summary>
    /// <returns></returns>
    [HttpGet("cultures", Name = nameof(GetCultures))]
    public IEnumerable<KCulture> GetCultures()
    {
        string defaultCultureCode = settingServiceFactory.GetSettingService(siteService.CurrentSite.SiteID).GetStringValue("CMSDefaultCultureCode");

        var siteCultures = CultureSiteInfoProvider.GetSiteCultures(siteService.CurrentSite.SiteName)
            .Select(c => new KCulture
            {
                CultureName = c.CultureName,
                CultureCode = c.CultureCode,
                CultureIsDefault = c.CultureCode.Equals(defaultCultureCode, StringComparison.InvariantCultureIgnoreCase)
            });

        return siteCultures;
    }


    /// <summary>
    /// Returns all enabled site currencies
    /// </summary>
    /// <returns></returns>
    [HttpGet("currencies", Name = nameof(GetCurrencies))]
    public async Task<IEnumerable<string>> GetCurrencies()
        => (await CurrencyInfoProvider.GetCurrencies(siteService.CurrentSite.SiteID).GetEnumerableTypedResultAsync())
            .Select(c => c.CurrencyCode);
}
