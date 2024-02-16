using Kentico.Xperience.StoreApi.Products.Pages;

namespace DancingGoat.ShopApi;

/// <summary>
/// Example for custom SKUTreeNode model inherited from default
/// </summary>
public class CustomProductPage : KProductNode
{
    public string CoffeeFarm { get; set; }
}
