using CMS.ContentEngine;
using CMS.ContentEngine.Internal;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Integration.K13Ecommerce;
using CMS.Membership;
using CMS.Websites;

using K13Store;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;
using Kentico.Xperience.K13Ecommerce.ProductPages;
using Kentico.Xperience.K13Ecommerce.SiteStore;
using Kentico.Xperience.K13Ecommerce.WebPageFolders;
using Kentico.Xperience.K13Ecommerce.WebsiteChannel;

using Microsoft.Extensions.Logging;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductPages;

internal class ProductPageSynchronizationService : IProductPageSynchronizationService
{
    private readonly IContentItemService contentItemService;
    private readonly IWebPageManagerFactory webPageManagerFactory;
    private readonly ISiteStoreService siteStoreService;
    private readonly ILogger<ProductPageSynchronizationService> logger;
    private readonly IContentQueryExecutor contentQueryExecutor;
    private readonly IInfoProvider<PagePathMappingRuleInfo> pagePathMappingRuleInfoProvider;
    private readonly IWebsiteChannelProvider websiteChannelProvider;
    private readonly IProgressiveCache progressiveCache;
    private readonly IWebPageFolderService webPageFolderService;
    private readonly IContentItemCodeNameProvider contentItemCodeNameProvider;

    private readonly Dictionary<string, IWebPageManager> webPageManagers = [];

    public ProductPageSynchronizationService(
        IContentItemService contentItemService,
        ILogger<ProductPageSynchronizationService> logger,
        IWebPageManagerFactory webPageManagerFactory,
        ISiteStoreService siteStoreService,
        IContentQueryExecutor contentQueryExecutor,
        IInfoProvider<PagePathMappingRuleInfo> pagePathMappingRuleInfoProvider,
        IWebsiteChannelProvider websiteChannelProvider,
        IProgressiveCache progressiveCache,
        IWebPageFolderService webPageFolderService,
        IContentItemCodeNameProvider contentItemCodeNameProvider)
    {
        this.contentItemService = contentItemService;
        this.logger = logger;
        this.siteStoreService = siteStoreService;
        this.contentQueryExecutor = contentQueryExecutor;
        this.pagePathMappingRuleInfoProvider = pagePathMappingRuleInfoProvider;
        this.websiteChannelProvider = websiteChannelProvider;
        this.webPageManagerFactory = webPageManagerFactory;
        this.progressiveCache = progressiveCache;
        this.webPageFolderService = webPageFolderService;
        this.contentItemCodeNameProvider = contentItemCodeNameProvider;
    }

    public async Task SynchronizeProductPages()
    {
        var contentItemProducts =
            await contentItemService.GetContentItems<ProductSKU>(ProductSKU.CONTENT_TYPE_NAME, linkedItemsLevel: 0);

        //Current limitations: only synchronization from default culture is supported. XByK must have same language enabled as default culture in K13
        string defaultCultureCode = ((await siteStoreService.GetCultures()).FirstOrDefault(c => c.CultureIsDefault)?.CultureCode)
            ?? throw new InvalidOperationException("No default culture found on K13 Store");

        string language = defaultCultureCode[..2];

        var rules = await progressiveCache.LoadAsync(
            async cacheSettings =>
            {
                var result = await pagePathMappingRuleInfoProvider.Get()
                    .GetEnumerableTypedResultAsync();

                cacheSettings.CacheDependency = CacheHelper.GetCacheDependency($"{PagePathMappingRuleInfo.OBJECT_TYPE}|all");

                return result.ToList();
            },
            new CacheSettings(TimeSpan.FromHours(1).TotalMinutes, $"{nameof(SynchronizeProductPages)}|PagePathMappingRules")
        );

        foreach (var product in contentItemProducts)
        {
            await SynchronizeProductPageByRules(product, rules, language);
        }
    }

    /// <summary>
    /// Create or update <see cref="ProductPage"/> for <paramref name="product"/> by first mached rule 
    /// (based on <see cref="PagePathMappingRuleInfo.PagePathMappingRuleK13NodeAliasPath"/>).
    /// </summary>
    /// <param name="product"></param>
    /// <param name="rules"></param>
    /// <param name="languageName"></param>
    /// <returns></returns>
    private async Task SynchronizeProductPageByRules(ProductSKU product, List<PagePathMappingRuleInfo> rules, string languageName)
    {
        var productPageModel = ProductPagePathMapping.MapPath(product.NodeAliasPath, rules);
        if (productPageModel is null)
        {
            logger.LogWarning("There is no rule for product with NodeAliasPath '{NodeAliasPath}'.", product.NodeAliasPath);
            return;
        }
        var (mappedPath, websiteChannelName) = productPageModel;


        var (folderPath, pageName) = ProcessPagePath(mappedPath);
        var folder = await webPageFolderService.CreateWebPageFolder(folderPath, websiteChannelName, languageName);

        if (folder is null)
        {
            logger.LogWarning("Folder with path '{FolderPath}' does not exist", folderPath);
            return;
        }

        var webPageManager = await GetChannelWebPageManager(websiteChannelName);
        if (webPageManager is null)
        {
            logger.LogWarning("Website channel '{WebsiteChannel}' does not exist", websiteChannelName);
            return;
        }

        var productPage = await GetProductPage(folderPath, product.SKUName, websiteChannelName);

        var itemData = CreateContentItemData(product);

        if (productPage is null)
        {
            // Create product page
            var webPageParameters = await CreateWebPageParameters(product, folder, pageName, languageName, itemData);

            try
            {
                int productPageId = await webPageManager.Create(webPageParameters);
                await webPageManager.TryPublish(productPageId, languageName);
            }
            catch (UrlPathCollisionException e)
            {
                logger.LogError(e, "Synchronization failed for product with NodeAliasPath: '{NodeAliasPath}'", product.NodeAliasPath);
            }
        }
        else
        {
            if (productPage.Product.FirstOrDefault()?.SystemFields.ContentItemGUID != product.SystemFields.ContentItemGUID)
            {
                // Need to update product page
                int productPageId = productPage.SystemFields.WebPageItemID;
                await webPageManager.TryCreateDraft(productPageId, languageName);
                await webPageManager.TryUpdateDraft(productPageId, languageName, new UpdateDraftData(itemData));
                await webPageManager.TryPublish(productPageId, languageName);
            }
        }
    }

    private async Task<CreateWebPageParameters> CreateWebPageParameters(ProductSKU product, WebPageFolder folder, string pageName, string language, ContentItemData itemData)
    {
        var contentItemParameters = new ContentItemParameters(ProductPage.CONTENT_TYPE_NAME, itemData);

        var createPageParameters = new CreateWebPageParameters(
            product.SKUName,
            language,
            contentItemParameters
        )
        {
            ParentWebPageItemID = folder.WebPageItemID,
            UrlSlug = await contentItemCodeNameProvider.Get(pageName),
        };
        return createPageParameters;
    }

    private static ContentItemData CreateContentItemData(ProductSKU product)
        => new(
            new Dictionary<string, object>
            {
                {
                    nameof(ProductPage.Product),
                    new List<ContentItemReference>()
                    {
                        new()
                        {
                            Identifier = product.SystemFields.ContentItemGUID
                        }
                    }
                }
            }
        );

    private async Task<IWebPageManager?> GetChannelWebPageManager(string channelName)
    {
        if (!webPageManagers.TryGetValue(channelName, out var webPageManager))
        {
            int websiteChannelId = await websiteChannelProvider.GetWebsiteChannelId(channelName);
            if (websiteChannelId == 0)
            {
                return null;
            }
            webPageManager = webPageManagerFactory.Create(websiteChannelId, UserInfoProvider.AdministratorUser.UserID);
            webPageManagers[channelName] = webPageManager;
        }

        return webPageManager;
    }

    private async Task<ProductPage?> GetProductPage(string folderPath, string pageDisplayName, string channelName)
    {
        string treePathSlug = TreePathUtils.NormalizePathSegment(pageDisplayName);
        string pagePath = $"{folderPath}/{treePathSlug}";
        var builder = new ContentItemQueryBuilder()
            .ForContentType(ProductPage.CONTENT_TYPE_NAME, q => q
                .Where(p => p.WhereEquals(nameof(IWebPageContentQueryDataContainer.WebPageItemTreePath), pagePath))
                .ForWebsite(channelName)
                .Columns(nameof(IWebPageContentQueryDataContainer.WebPageItemID))
                .TopN(1)
                .WithLinkedItems(1)
            );

        var pages = await contentQueryExecutor.GetMappedWebPageResult<ProductPage>(
            builder,
            new ContentQueryExecutionOptions()
            {
                ForPreview = true,
            }
        );
        return pages.FirstOrDefault();
    }

    private static (string FolderPath, string PageName) ProcessPagePath(string? path)
    {
        if (string.IsNullOrEmpty(path) || !path.Contains('/'))
        {
            throw new ArgumentException($"Invalid path: '{path}'");
        }
        if (path.EndsWith('/'))
        {
            throw new ArgumentException($"Path '{path}' cannot end with slash");
        }
        int lastSlashIndex = path.LastIndexOf('/');
        string folderPath = path[..lastSlashIndex];
        if (string.IsNullOrEmpty(folderPath))
        {
            folderPath = "/";
        }
        string pageName = path[(lastSlashIndex + 1)..];
        return (folderPath, pageName);
    }

}
