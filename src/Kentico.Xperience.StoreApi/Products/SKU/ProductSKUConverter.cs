using AutoMapper;
using CMS.Base;
using CMS.Ecommerce;
using Kentico.Xperience.StoreApi.Products.Prices;

namespace Kentico.Xperience.StoreApi.Products.SKU;

/// <summary>
/// Implemented converter from Kentico SKU to sku model
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class ProductSKUConverter<TModel> : IProductSKUConverter<TModel>
    where TModel : KProductSKU, new()
{
    private readonly ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory;
    private readonly ISiteService siteService;
    private readonly IMapper mapper;

    public ProductSKUConverter(ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory, ISiteService siteService)
    {
        this.catalogPriceCalculatorFactory = catalogPriceCalculatorFactory;
        this.siteService = siteService;
        mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StoreApiMappingProfile>();
                cfg.CreateMap<SKUInfo, TModel>();
            })
            .CreateMapper();
    }

    public virtual TModel Convert(SKUInfo skuInfo, string currencyCode)
    {
        var model = mapper.Map<TModel>(skuInfo);

        var cart = ShoppingCartFactory.CreateCart(siteService.CurrentSite.SiteID);
        if (currencyCode is not null && CurrencyInfoProvider.GetCurrenciesByCode(siteService.CurrentSite.SiteID)
                .TryGetValue(currencyCode, out var currency))
        {
            cart.ShoppingCartCurrencyID = currency.CurrencyID;
        }

        var priceCalculator = catalogPriceCalculatorFactory.GetCalculator(siteService.CurrentSite.SiteID);
        var prices = priceCalculator.GetPrices(skuInfo, null, cart);

        //@TODO move to standalone converter to enable easy customization of variant model
        model.Variants = skuInfo.Children[SKUInfo.OBJECT_TYPE_VARIANT].OfType<SKUInfo>()
            .Select(v => mapper.Map<KProductVariant>(v))
            .ToList();

        model.Prices = mapper.Map<KProductCatalogPrices>(prices);

        return model;
    }
}