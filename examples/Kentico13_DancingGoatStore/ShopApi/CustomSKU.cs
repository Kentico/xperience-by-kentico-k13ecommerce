using Kentico.Xperience.StoreApi.Products.SKU;

namespace DancingGoat.ShopApi;

public class CustomSKU : KProductSKU
{
    public string TestCustom { get; set; } = "test";
    //public override string MainImageUrl => "Some test";
}