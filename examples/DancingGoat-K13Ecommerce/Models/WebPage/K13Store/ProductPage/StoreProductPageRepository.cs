using CMS.ContentEngine;
using CMS.Core;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using Kentico.Xperience.K13Ecommerce.ProductPages;

namespace DancingGoat.Models;

public class StoreProductPageRepository : StoreContentRepositoryBase
{
    public StoreProductPageRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor,
        IProgressiveCache cache, IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever,
        IConversionService conversionService, ISettingsService settingsService) : base(websiteChannelContext, executor,
        cache, webPageLinkedItemsDependencyRetriever, conversionService, settingsService)
    {
    }


    /// <summary>
    /// Returns model for product detail page
    /// </summary>
    /// <param name="webPageItemId"></param>
    /// <param name="languageName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductPage> GetProductDetailPage(int webPageItemId, string languageName,
        CancellationToken cancellationToken = default)
    {
        var queryBuilder = GetQueryBuilder(webPageItemId, languageName, ProductPage.CONTENT_TYPE_NAME, 2);
        var cacheSettings = new CacheSettings(CacheMinutes, WebsiteChannelContext.WebsiteChannelName,
            nameof(ProductPage), webPageItemId, languageName);

        var result = await GetCachedQueryResult<ProductPage>(queryBuilder, null, cacheSettings,
            (pages, token) => GetDependencyCacheKeys(pages, 2, token), cancellationToken);

        return result.FirstOrDefault();
    }
}
