using AutoMapper;

using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.FormEngine;

using Kentico.Content.Web.Mvc;
using Kentico.Xperience.StoreApi.Products.Categories;
using Kentico.Xperience.StoreApi.Products.Pages;
using Kentico.Xperience.StoreApi.Products.Prices;
using Kentico.Xperience.StoreApi.Products.SKU;

namespace Kentico.Xperience.StoreApi.Products;

/// <inheritdoc/>
internal class KProductService : IKProductService
{
    private readonly IPageRetriever pageRetriever;
    private readonly IProductPageConverter<KProductNode> productPageConverter;
    private readonly IProductSKUConverter<KProductSKU> productSKUConverter;
    private readonly ISKUInfoProvider skuInfoProvider;
    private readonly ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory;
    private readonly ISiteService siteService;
    private readonly IMapper mapper;
    private readonly IShoppingService shoppingService;


    public KProductService(
        IPageRetriever pageRetriever, IProductPageConverter<KProductNode> productPageConverter, IProductSKUConverter<KProductSKU> productSKUConverter,
        ISKUInfoProvider skuInfoProvider, ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory,
        ISiteService siteService, IMapper mapper, IShoppingService shoppingService)
    {
        this.pageRetriever = pageRetriever;
        this.productPageConverter = productPageConverter;
        this.productSKUConverter = productSKUConverter;
        this.skuInfoProvider = skuInfoProvider;
        this.catalogPriceCalculatorFactory = catalogPriceCalculatorFactory;
        this.siteService = siteService;
        this.mapper = mapper;
        this.shoppingService = shoppingService;
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<KProductNode>> GetProductPages(ProductPageRequest request)
    {
        var (path, culture, currencyCode, orderBy, limit, withVariants, withLongDescription, noLinks) = request;
        if (string.IsNullOrWhiteSpace(orderBy))
        {
            orderBy = "DocumentSKUName ASC";
        }

        var productTypes = (await GetProductDataClasses())
            .Select(p => new
            {
                p.ClassName,
                CustomFields = new FormInfo(p.ClassFormDefinition).ItemsList
                    .OfType<FormFieldInfo>()
                    .Where(i => !i.PrimaryKey)
                    .Select(i => i.Name)
                    .ToList()
            })
            .ToList();

        var productPages = (await pageRetriever.RetrieveMultipleAsync(q =>
            {
                q.Types(productTypes.Select(p => p.ClassName).ToArray())
                    .Path(path, PathTypeEnum.Children)
                    .FilterDuplicates(noLinks)
                    .TopN(limit)
                    .OrderBy(orderBy);
                if (culture is not null)
                {
                    q.Culture(culture);
                }
            }
        )).OfType<SKUTreeNode>();

        return productPages.Where(p => p.SKU is not null)
            .Select(p => productPageConverter.Convert(p,
                productTypes.First(t => t.ClassName == p.ClassName)
                    .CustomFields, currencyCode, withVariants, withLongDescription))
            .ToList();
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<KProductCategory>> GetProductCategories(string culture)
    {
        string[] categoriesTypes = (await DataClassInfoProvider.ProviderObject.Get()
                .WhereTrue(nameof(DataClassInfo.ClassIsProductSection))
                .Column(nameof(DataClassInfo.ClassName))
                .GetEnumerableTypedResultAsync())
            .Select(p => p.ClassName)
            .ToArray();

        return (await pageRetriever.RetrieveMultipleAsync(q =>
            {
                q.Types(categoriesTypes);
                if (culture is not null)
                {
                    q.Culture(culture);
                }
            }))
            .Select(c => new KProductCategory { Name = c.DocumentName, Path = c.NodeAliasPath });
    }


    /// <inheritdoc/>
    public async Task<ProductPricesResponse> GetProductPrices(int productSkuId, string currencyCode)
    {
        var sku = await skuInfoProvider.GetAsync(productSkuId) ??
            throw new InvalidOperationException("No such product with SKUID: " + productSkuId);

        return GetProductPrices(sku, currencyCode);
    }


    /// <inheritdoc/>
    public async IAsyncEnumerable<ProductPricesResponse> GetProductPrices(IEnumerable<int> productsSkuIds, string currencyCode)
    {
        var skus = await skuInfoProvider.Get()
            .WhereIn(nameof(SKUInfo.SKUID), productsSkuIds.ToArray())
            .GetEnumerableTypedResultAsync();

        foreach (var sku in skus)
        {
            yield return GetProductPrices(sku, currencyCode);
        }
    }


    /// <inheritdoc/>
    public async Task<ProductInventoryPriceInfo> GetProductInventoryAndPrices(int skuId, string currencyCode)
    {
        var sku = await skuInfoProvider.GetAsync(skuId) ??
            throw new InvalidOperationException("No such product or variant with SKUID: " + skuId);

        var calculator = catalogPriceCalculatorFactory.GetCalculator(siteService.CurrentSite.SiteID);

        var currentCart = shoppingService.GetCurrentShoppingCart();

        if (currencyCode is not null && CurrencyInfoProvider.GetCurrenciesByCode(siteService.CurrentSite.SiteID)
                .TryGetValue(currencyCode, out var currency) &&
            currentCart.ShoppingCartCurrencyID != currency.CurrencyID)
        {
            currentCart.ShoppingCartCurrencyID = currency.CurrencyID;
        }

        var response = new ProductInventoryPriceInfo
        {
            SkuId = sku.SKUID,
            Prices = mapper.Map<KProductCatalogPrices>(calculator.GetPrices(sku, null, currentCart)),
            SKUAvailableItems = sku.SKUAvailableItems,
            SKUSellOnlyAvailable = sku.SKUSellOnlyAvailable,
            SKUTrackInventory = sku.SKUTrackInventory
        };

        return response;
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<KProductSKU>> GetStandaloneProducts(ProductRequest request)
    {
        var (culture, currencyCode, orderBy, limit, withVariants) = request;

        string[] productTypes = (await GetProductDataClasses())
            .Select(p => p.ClassName)
            .ToArray();

        var skuInfos = skuInfoProvider.Get()
            .WhereEqualsOrNull(nameof(SKUInfo.SKUOptionCategoryID), 0)
            .WhereEqualsOrNull(nameof(SKUInfo.SKUParentSKUID), 0)
            .TopN(limit)
            .OrderBy(orderBy);

        if (productTypes.Length > 0)
        {
            var query = new MultiDocumentQuery()
                .Types(productTypes)
                .Column(nameof(SKUTreeNode.NodeSKUID));

            if (!string.IsNullOrEmpty(culture))
            {
                query = query.Culture(culture);
            }

            skuInfos = skuInfos.WhereNotIn(nameof(SKUInfo.SKUID), query);
        }

        return (await skuInfos.GetEnumerableTypedResultAsync())
            .Select(x => productSKUConverter.Convert(x, currencyCode, withVariants));
    }


    private ProductPricesResponse GetProductPrices(SKUInfo sku, string currencyCode)
    {
        var calculator = catalogPriceCalculatorFactory.GetCalculator(siteService.CurrentSite.SiteID);
        var response = new ProductPricesResponse { ProductSkuId = sku.SKUID };
        var currentCart = shoppingService.GetCurrentShoppingCart();

        if (currencyCode is not null && CurrencyInfoProvider.GetCurrenciesByCode(siteService.CurrentSite.SiteID)
                .TryGetValue(currencyCode, out var currency) &&
            currentCart.ShoppingCartCurrencyID != currency.CurrencyID)
        {
            currentCart.ShoppingCartCurrencyID = currency.CurrencyID;
        }

        response.Prices = mapper.Map<KProductCatalogPrices>(calculator.GetPrices(sku, null, currentCart));

        if (sku.IsProduct && sku.HasVariants)
        {
            var skuVariants = sku.Children[SKUInfo.OBJECT_TYPE_VARIANT].OfType<SKUInfo>();
            response.VariantPrices = skuVariants.AsEnumerable()
                .ToDictionary(sku => sku.SKUID, sku => mapper.Map<KProductCatalogPrices>(calculator.GetPrices(sku, null, currentCart)));
        }

        return response;
    }


    private async Task<IEnumerable<DataClassInfo>> GetProductDataClasses()
        => await DataClassInfoProvider.ProviderObject.Get()
                .WhereTrue(nameof(DataClassInfo.ClassIsProduct))
                .Columns(nameof(DataClassInfo.ClassName), nameof(DataClassInfo.ClassFormDefinition))
                .GetEnumerableTypedResultAsync();
}
