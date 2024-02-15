using CMS.Base;
using CMS.Ecommerce;
using Kentico.Xperience.StoreApi.Products.SKU;

namespace DancingGoat.ShopApi;

public class CustomSKUConverter : ProductSKUConverter<CustomSKU>
{
    public override CustomSKU Convert(SKUInfo skuInfo, string currencyCode = null)
    {
        var model = base.Convert(skuInfo, currencyCode);
        return model;
    }

    public CustomSKUConverter(ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory, ISiteService siteService) :
        base(catalogPriceCalculatorFactory, siteService)
    {
    }
}