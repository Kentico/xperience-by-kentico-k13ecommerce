using CMS.Base;
using CMS.Ecommerce;
using Kentico.Xperience.StoreApi.Products.SKU;

namespace DancingGoat.ShopApi;

/// <summary>
/// Example of custom converter from SKU when you want to have complete control over mapping to custom model
/// </summary>
public class CustomSKUConverter : ProductSKUConverter<CustomSKU>
{
    public override CustomSKU Convert(SKUInfo skuInfo, string currencyCode)
    {
        var model = base.Convert(skuInfo, currencyCode);
        return model;
    }

    public CustomSKUConverter(ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory, ISiteService siteService) :
        base(catalogPriceCalculatorFactory, siteService)
    {
    }
}
