namespace DancingGoat.Models;

public class CategoryPageViewModel
{
    public string CategoryName { get; init; }


    public IEnumerable<StoreProductListItemViewModel> Products { get; init; }



    public static CategoryPageViewModel GetViewModel(
        CategoryPage category,
        IEnumerable<StoreProductListItemViewModel> products) =>
        new()
        {
            CategoryName = category.CategoryName,
            Products = products
        };
}
