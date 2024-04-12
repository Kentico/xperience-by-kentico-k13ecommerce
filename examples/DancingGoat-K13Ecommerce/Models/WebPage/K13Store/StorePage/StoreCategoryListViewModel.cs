using CMS.Websites;

namespace DancingGoat.Models;

public class StoreCategoryListViewModel
{
    public string CategoryName { get; set; }


    public string CategoryUrl { get; set; }


    public static async ValueTask<StoreCategoryListViewModel> GetViewModel(CategoryPage category, IWebPageUrlRetriever urlRetriever) =>
        new()
        {
            CategoryName = category.CategoryName,
            CategoryUrl = (await urlRetriever.Retrieve(category)).RelativePath
        };
}
