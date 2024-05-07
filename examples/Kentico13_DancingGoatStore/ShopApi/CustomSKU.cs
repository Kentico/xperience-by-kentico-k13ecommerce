using Kentico.Xperience.StoreApi.Products.SKU;

namespace DancingGoat.ShopApi;

/// <summary>
/// Example for custom SKU model inherited from default
/// </summary>
public class CustomSKU : KProductSKU
{
    public string TestCustom { get; set; } = "test";
    //public override string MainImageUrl => ""; //You can override image url, when you don't use default SKUImagePath
}
