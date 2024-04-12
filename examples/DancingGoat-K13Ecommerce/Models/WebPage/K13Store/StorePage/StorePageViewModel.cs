using CMS.Websites;

namespace DancingGoat.Models;

public class StorePageViewModel : IWebPageBasedViewModel
{
    public string Title { get; set; }
    public IEnumerable<StoreCategoryListViewModel> Categories { get; set; }
    public IWebPageFieldsSource WebPage { get; init; }

    public IEnumerable<ProductListItemViewModel> Bestsellers { get; set; }
    public IEnumerable<ProductListItemViewModel> HotTips { get; set; }

    public static StorePageViewModel GetViewModel(StorePage storePage,
        IEnumerable<StoreCategoryListViewModel> categories,
        IEnumerable<ProductListItemViewModel> bestsellers,
        IEnumerable<ProductListItemViewModel> hotTips)
    {
        ArgumentNullException.ThrowIfNull(storePage);

        return new StorePageViewModel
        {
            Title = "Store",
            Categories = categories,
            WebPage = storePage,
            Bestsellers = bestsellers,
            HotTips = hotTips
        };
    }
}
