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

namespace Kentico.Xperience.StoreApi.Products;

internal class KProductService : IKProductService
{
    private readonly IPageRetriever pageRetriever;
    private readonly IProductPageConverter<KProductNode> productPageConverter;
    private readonly ISKUInfoProvider skuInfoProvider;
    private readonly ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory;
    private readonly ISiteService siteService;
    private readonly IMapper mapper;
    private readonly IShoppingService shoppingService;

    public KProductService(IPageRetriever pageRetriever, IProductPageConverter<KProductNode> productPageConverter,
        ISKUInfoProvider skuInfoProvider, ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory,
        ISiteService siteService, IMapper mapper, IShoppingService shoppingService)
    {
        this.pageRetriever = pageRetriever;
        this.productPageConverter = productPageConverter;
        this.skuInfoProvider = skuInfoProvider;
        this.catalogPriceCalculatorFactory = catalogPriceCalculatorFactory;
        this.siteService = siteService;
        this.mapper = mapper;
        this.shoppingService = shoppingService;
    }

    public async Task<IEnumerable<KProductNode>> GetProductPages(ProductPageRequest request)
    {
        var (path, culture, currencyCode, orderBy, limit, withVariants, withLongDescription, noLinks) = request;
        if (string.IsNullOrWhiteSpace(orderBy))
        {
            orderBy = "DocumentSKUName ASC";
        }

        var productTypes = (await DataClassInfoProvider.ProviderObject.Get()
                .WhereTrue(nameof(DataClassInfo.ClassIsProduct))
                .Columns(nameof(DataClassInfo.ClassName), nameof(DataClassInfo.ClassFormDefinition))
                .GetEnumerableTypedResultAsync())
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

    public async Task<IEnumerable<KProductCategory>> GetProductCategories(string culture)
    {
        var categoriesTypes = (await DataClassInfoProvider.ProviderObject.Get()
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

    public async Task<ProductPricesResponse> GetProductPrices(int productSkuId, string currencyCode)
    {
        var sku = await skuInfoProvider.GetAsync(productSkuId) ??
            throw new InvalidOperationException("No such product with SKUID: " + productSkuId);

        return GetProductPrices(sku, currencyCode);
    }

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
}
