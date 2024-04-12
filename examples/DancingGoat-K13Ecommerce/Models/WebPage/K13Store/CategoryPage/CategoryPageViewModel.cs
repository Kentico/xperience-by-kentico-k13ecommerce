namespace DancingGoat.Models;

public class CategoryPageViewModel
{
    public string CategoryName { get; init; }


    public IEnumerable<ProductListItemViewModel> Products { get; init; }



    public static CategoryPageViewModel GetViewModel(
        CategoryPage category,
        IEnumerable<ProductListItemViewModel> products) =>
        new()
        {
            CategoryName = category.CategoryName,
            Products = products
        };
}
