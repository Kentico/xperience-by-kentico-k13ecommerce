using CMS.ContentEngine;
using CMS.Core;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using K13Store;

using Kentico.Xperience.K13Ecommerce.ProductPages;
using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace DancingGoat.Models;

public class CheckoutPageRepository : StoreContentRepositoryBase
{
    private readonly IWebsiteChannelContext websiteChannelContext;
    private readonly IContentQueryExecutor executor;
    private readonly IWebPageQueryResultMapper pageMapper;
    private readonly IContentQueryResultMapper contentMapper;


    public CheckoutPageRepository(
        IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor, IWebPageQueryResultMapper pageMapper, IContentQueryResultMapper contentMapper,
        IProgressiveCache cache, IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever,
        IConversionService conversionService, ISettingsService settingsService) : base(websiteChannelContext, executor,
        cache, webPageLinkedItemsDependencyRetriever, conversionService, settingsService)
    {
        this.websiteChannelContext = websiteChannelContext;
        this.executor = executor;
        this.pageMapper = pageMapper;
        this.contentMapper = contentMapper;
    }


    public async Task<TPage> GetCartStepPage<TPage>(int webPageItemId, string languageName, string contentTypeName,
        CancellationToken cancellationToken = default)
        where TPage : IWebPageFieldsSource, new()
    {
        var queryBuilder = GetQueryBuilder(webPageItemId, languageName, contentTypeName, 1);
        var cacheSettings = new CacheSettings(CacheMinutes, WebsiteChannelContext.WebsiteChannelName,
            nameof(CategoryPage), languageName, webPageItemId);

        var result = await GetCachedQueryResult<TPage>(queryBuilder, null, cacheSettings,
            (pages, token) => GetDependencyCacheKeys(pages.OfType<IWebPageFieldsSource>(), 0, token), cancellationToken);

        return result.FirstOrDefault();
    }


    public async Task<CartContent> GetCartContentPage(string languageName, CancellationToken cancellationToken = default)
    {
        var pageQueryBuilder = new ContentItemQueryBuilder()
            .ForContentType(CartContent.CONTENT_TYPE_NAME, q =>
                q.TopN(1).ForWebsite(websiteChannelContext.WebsiteChannelName));

        var cacheSettings = new CacheSettings(CacheMinutes, WebsiteChannelContext.WebsiteChannelName,
            nameof(GetCartContentPage), languageName);

        var result = await GetCachedQueryResult<CartContent>(pageQueryBuilder, null, cacheSettings,
            (pages, token) => GetDependencyCacheKeys(pages, 0, token), cancellationToken);

        return result.FirstOrDefault();
    }


    public async Task<ProductPage> GetProductPageBySKUID(int skuId, string languageName)
    {
        var skuItem = (await GetProductsByQuery(config => config
                .TopN(1)
                .WithLinkedItems(1)
                .Where(w => w.WhereEquals(nameof(ProductSKU.SKUID), skuId)), languageName))
            .FirstOrDefault();

        if (skuItem is null)
        {
            return null;
        }

        var pageQueryBuilder = new ContentItemQueryBuilder()
            .ForContentType(ProductPage.CONTENT_TYPE_NAME, q =>
                q.TopN(1)
                    .Linking(nameof(ProductPage.Product), [skuItem.SystemFields.ContentItemID])
                    .ForWebsite(websiteChannelContext.WebsiteChannelName))
            .InLanguage(languageName);

        var productDetailPage =
            (await executor.GetWebPageResult(pageQueryBuilder, c => pageMapper.Map<ProductPage>(c)))
            .FirstOrDefault();

        return productDetailPage;
    }


    private async Task<IEnumerable<ProductSKU>> GetProductsByQuery(Action<ContentTypeQueryParameters> query, string languageName)
    {
        var builder = new ContentItemQueryBuilder()
            .ForContentType(ProductSKU.CONTENT_TYPE_NAME, query)
            .InLanguage(languageName);

        return await executor.GetResult(builder, c => contentMapper.Map<ProductSKU>(c));
    }


    public async Task<ILookup<int, ContentItemAsset>> GetProductImages(KShoppingCartContent cart, string languageName) =>
        (await GetProductsByQuery(q =>
            q.WithLinkedItems(1)
                .Where(w =>
                    w.WhereIn(nameof(ProductSKU.SKUID),
                        cart.CartProducts.Select(p => p.ProductSKU.Skuid).ToList())), languageName))
        .Where(p => p.ProductImages.Any())
        .ToLookup(p => p.SKUID, p => p.ProductImages.First().ProductImageAsset);
}
