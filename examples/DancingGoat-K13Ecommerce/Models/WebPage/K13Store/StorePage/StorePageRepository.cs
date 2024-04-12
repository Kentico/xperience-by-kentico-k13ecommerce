using System.Runtime.CompilerServices;

using CMS.ContentEngine;
using CMS.Core;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using K13Store;

namespace DancingGoat.Models;

public class StorePageRepository : StoreContentRepositoryBase
{
    private readonly IWebsiteChannelContext websiteChannelContext;
    private readonly IContentQueryExecutor executor;
    private readonly IWebPageQueryResultMapper mapper;
    private readonly IProgressiveCache cache;
    private readonly IContentQueryExecutor contentQueryExecutor;
    private readonly IContentQueryResultMapper contentMapper;
    private readonly IWebPageQueryResultMapper pageMapper;
    private readonly ISettingsService settingsService;
    private readonly IConversionService conversionService;


    public StorePageRepository(
        IWebsiteChannelContext websiteChannelContext,
        IContentQueryExecutor executor,
        IWebPageQueryResultMapper mapper,
        IProgressiveCache cache,
        IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever,
        ISettingsService settingsService,
        IConversionService conversionService,
        IContentQueryExecutor contentQueryExecutor,
        IContentQueryResultMapper contentMapper,
        IWebPageQueryResultMapper pageMapper)
        : base(websiteChannelContext, executor, mapper, cache, webPageLinkedItemsDependencyRetriever, conversionService,
            settingsService)
    {
        this.websiteChannelContext = websiteChannelContext;
        this.executor = executor;
        this.mapper = mapper;
        this.cache = cache;
        this.settingsService = settingsService;
        this.conversionService = conversionService;
        this.contentMapper = contentMapper;
        this.pageMapper = pageMapper;
        this.contentQueryExecutor = contentQueryExecutor;
    }


    /// <summary>
    /// Returns <see cref="StorePage"/> content item.
    /// </summary>
    public async Task<StorePage> GetStorePage(int webPageItemId, string languageName,
        CancellationToken cancellationToken = default)
    {
        var queryBuilder = GetQueryBuilder(webPageItemId, languageName, StorePage.CONTENT_TYPE_NAME, 1);

        var cacheSettings = new CacheSettings(CacheMinutes, WebsiteChannelContext.WebsiteChannelName, nameof(StorePage),
            webPageItemId, languageName);

        var result = await GetCachedQueryResult<StorePage>(queryBuilder, null, cacheSettings,
            (pages, token) => GetDependencyCacheKeys(pages, 0, token), cancellationToken);

        return result.FirstOrDefault();
    }


    public async IAsyncEnumerable<CategoryPage> GetCategories(StorePage store, string languageName, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string channelName = websiteChannelContext.WebsiteChannelName;

        var categories = await cache.LoadAsync(async (cacheSettings) =>
            {
                var builder = new ContentItemQueryBuilder()
                    .ForContentType(CategoryPage.CONTENT_TYPE_NAME, config =>
                        config.ForWebsite(channelName,
                                new[] { PathMatch.Children(store.SystemFields.WebPageItemTreePath) })
                            .WithLinkedItems(0))
                    .InLanguage(languageName);

                var categories =
                    await executor.GetWebPageResult(builder,
                        container => mapper.Map<CategoryPage>(container));

                if (cacheSettings.Cached)
                {
                    cacheSettings.CacheDependency = CacheHelper.GetCacheDependency(await GetDependencyCacheKeys(categories, maxLevel: 0, cancellationToken));
                }

                return categories;
            },
            new CacheSettings(CacheMinutes, websiteChannelContext.WebsiteChannelName, nameof(StorePage),
                nameof(GetCategories), languageName, store.SystemFields.WebPageItemGUID));

        foreach (var category in categories)
        {
            yield return category;
        }
    }


    public async Task<IEnumerable<ProductPage>> GetBestsellers(string languageName)
    {
        var builder = new ContentItemQueryBuilder()
            .ForContentType(
                ProductSKU.CONTENT_TYPE_NAME,
                config => config
                    .WithLinkedItems(2)
                    .Where(where => where.WhereEquals(nameof(ProductSKU.PublicStatusDisplayName), "Bestseller"))
            ).InLanguage(languageName);

        var skuItems = await contentQueryExecutor.GetResult(builder, c => contentMapper.Map<ProductSKU>(c));
        if (skuItems is null)
        {
            return null;
        }

        var skuItemsId = skuItems.Select(item => item.SystemFields.ContentItemID).ToList();

        var queryBuilder = new ContentItemQueryBuilder()
            .ForContentType(ProductPage.CONTENT_TYPE_NAME, q =>
                q.Linking(nameof(ProductPage.Product), skuItemsId)
                    .WithLinkedItems(2)
                .ForWebsite(websiteChannelContext.WebsiteChannelName))
            .InLanguage(languageName);

        var result = await contentQueryExecutor.GetWebPageResult(queryBuilder, c => pageMapper.Map<ProductPage>(c));

        return result;
    }


    public async Task<IEnumerable<ProductPage>> GetHottips(StorePage category, string languageName, CancellationToken cancellationToken = default)
    {
        var queryBuilder = GetProductsQueryBuilder(category.HotTipProducts.Select(x => x.WebPageGuid).ToArray(), languageName);

        var cacheSettings = new CacheSettings(CacheMinutes, WebsiteChannelContext.WebsiteChannelName, nameof(ProductPage), languageName, category.SystemFields.WebPageItemID);
        return await GetCachedQueryResult<ProductPage>(queryBuilder, null, cacheSettings, (pages, token) => GetDependencyCacheKeys(pages, 1, cancellationToken), cancellationToken);
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
