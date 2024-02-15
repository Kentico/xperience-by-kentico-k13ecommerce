using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Ecommerce;
using Kentico.Xperience.StoreApi.Products.Pages;
using Kentico.Xperience.StoreApi.Products.SKU;

namespace DancingGoat.ShopApi;

public class CustomProductPageConverter : ProductPageConverter<CustomProductPage>
{
    public CustomProductPageConverter(IProductSKUConverter<KProductSKU> skuConverter) : base(skuConverter)
    {
    }
    
    public override CustomProductPage Convert(SKUTreeNode skuTreeNode, IEnumerable<string> customFields, string currencyCode = null)
    {
        var model =  base.Convert(skuTreeNode, customFields, currencyCode);
        if (skuTreeNode is Coffee coffee)
        {
            model.CoffeeFarm = coffee.CoffeeFarm;
        }

        return model;
    }
}