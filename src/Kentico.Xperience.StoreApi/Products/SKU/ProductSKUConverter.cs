using AutoMapper;

using CMS.Base;
using CMS.Ecommerce;

using Kentico.Xperience.StoreApi.Mapping;
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
                if (typeof(TModel) != typeof(KProductSKU))
                {
                    cfg.CreateMap<SKUInfo, TModel>().IncludeBase<SKUInfo, KProductSKU>();
                }
            })
            .CreateMapper();
    }

    public virtual TModel Convert(SKUInfo skuInfo, string currencyCode, bool withVariants)
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

        if (withVariants)
        {
            var skuVariants = skuInfo.Children[SKUInfo.OBJECT_TYPE_VARIANT].OfType<SKUInfo>();
            var variants = new List<KProductVariant>();
            foreach (var variantSku in skuVariants)
            {
                var pricesVariant = priceCalculator.GetPrices(variantSku, null, cart);
                var productVariant = mapper.Map<KProductVariant>(variantSku);
                productVariant.Prices = mapper.Map<KProductCatalogPrices>(pricesVariant);
                variants.Add(productVariant);
            }

            model.Variants = variants;
        }

        model.Prices = mapper.Map<KProductCatalogPrices>(prices);

        return model;
    }
}
