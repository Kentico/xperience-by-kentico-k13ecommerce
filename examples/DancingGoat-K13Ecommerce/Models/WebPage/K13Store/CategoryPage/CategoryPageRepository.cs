using CMS.ContentEngine;
using CMS.Core;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

namespace DancingGoat.Models;

public class CategoryPageRepository : StoreContentRepositoryBase
{
    public CategoryPageRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor,
        IProgressiveCache cache, IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever,
        IConversionService conversionService, ISettingsService settingsService) : base(websiteChannelContext, executor,
        cache, webPageLinkedItemsDependencyRetriever, conversionService, settingsService)
    {
    }


    /// <summary>
    /// Returns <see cref="CategoryPage"/> content item.
    /// </summary>
    public async Task<CategoryPage> GetCategoryPage(int webPageItemId, string languageName, CancellationToken cancellationToken = default)
    {
        var queryBuilder = GetQueryBuilder(webPageItemId, languageName, CategoryPage.CONTENT_TYPE_NAME, 4);
        var cacheSettings = new CacheSettings(CacheMinutes, WebsiteChannelContext.WebsiteChannelName, nameof(CategoryPage), languageName, webPageItemId);

        var result = await GetCachedQueryResult<CategoryPage>(queryBuilder, null, cacheSettings, (pages, token) => GetDependencyCacheKeys(pages, 1, token), cancellationToken);

        return result.FirstOrDefault();
    }


    public async Task<IEnumerable<ProductPage>> GetCategoryProducts(CategoryPage category, string languageName, CancellationToken cancellationToken = default)
    {
        var queryBuilder = GetProductsQueryBuilder(category.CategoryProducts.Select(x => x.WebPageGuid).ToArray(), languageName);

        var cacheSettings = new CacheSettings(CacheMinutes, WebsiteChannelContext.WebsiteChannelName, nameof(ProductPage), languageName, category.SystemFields.WebPageItemID);
        return (await GetCachedQueryResult<ProductPage>(queryBuilder, null, cacheSettings,
                (pages, token) => GetDependencyCacheKeys(pages, 1, cancellationToken), cancellationToken))
            .Where(p => p.Product.FirstOrDefault()?.SKUEnabled is true);
    }


    private ContentItemQueryBuilder GetProductsQueryBuilder(ICollection<Guid> webPageItemGUIDs, string languageName)
    {
        return new ContentItemQueryBuilder()
            .ForContentType(ProductPage.CONTENT_TYPE_NAME,
                config => config
                    .WithLinkedItems(2)
                    .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                    .Where(where => where.WhereIn(nameof(IWebPageContentQueryDataContainer.WebPageItemGUID), webPageItemGUIDs)))
            .InLanguage(languageName);
    }
}
