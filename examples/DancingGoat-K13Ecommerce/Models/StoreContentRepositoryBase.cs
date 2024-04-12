using CMS.ContentEngine;
using CMS.Core;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using DancingGoat.Extensions;

namespace DancingGoat.Models;

public class StoreContentRepositoryBase : ContentRepositoryBase
{
    protected readonly IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever;
    private readonly IConversionService conversionService;
    private readonly ISettingsService settingsService;

    protected int CacheMinutes => conversionService.GetInteger(settingsService["CMSCacheMinutes"], 0);

    public StoreContentRepositoryBase(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor,
        IWebPageQueryResultMapper mapper, IProgressiveCache cache,
        IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever,
        IConversionService conversionService,
        ISettingsService settingsService)
        : base(websiteChannelContext, executor, mapper, cache)
    {
        this.webPageLinkedItemsDependencyRetriever = webPageLinkedItemsDependencyRetriever;
        this.conversionService = conversionService;
        this.settingsService = settingsService;
    }

    protected async Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<IWebPageFieldsSource> pages, int maxLevel,
        CancellationToken cancellationToken)
    {
        if (pages == null)
        {
            return new HashSet<string>();
        }

        return (await pages.Select(async p => (await webPageLinkedItemsDependencyRetriever.Get(p.SystemFields.WebPageItemID, maxLevel,
                    cancellationToken))
                    .Append(CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", p.SystemFields.WebPageItemID.ToString() }, false)))
                .AsAsyncEnumerable()
                .ToListAsync(cancellationToken))
            .SelectMany(p => p)
            .Append(CacheHelper.GetCacheItemName(null, WebsiteChannelInfo.OBJECT_TYPE, "byid",
                WebsiteChannelContext.WebsiteChannelID))
            .Append(CacheHelper.GetCacheItemName(null, ContentLanguageInfo.OBJECT_TYPE, "all"))
            .ToHashSet();
    }

    protected ContentItemQueryBuilder GetQueryBuilder(int webPageItemId, string languageName, string contentTypeName,
        int linkedItemsLevel)
    {
        return new ContentItemQueryBuilder()
            .ForContentType(contentTypeName,
                config => config
                    .WithLinkedItems(linkedItemsLevel)
                    .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                    .Where(where =>
                        where.WhereEquals(nameof(IWebPageContentQueryDataContainer.WebPageItemID), webPageItemId))
                    .TopN(1))
            .InLanguage(languageName);
    }
}
