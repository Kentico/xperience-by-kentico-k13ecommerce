using CMS.Websites;

namespace DancingGoat.Models;

public class StorePageViewModel : IWebPageBasedViewModel
{
    public string Title { get; set; }

    public IEnumerable<StoreCategoryListViewModel> Categories { get; set; }

    public IWebPageFieldsSource WebPage { get; init; }

    public IEnumerable<StoreProductListItemViewModel> Bestsellers { get; set; }

    public IEnumerable<StoreProductListItemViewModel> HotTips { get; set; }

    public static StorePageViewModel GetViewModel(StorePage storePage,
        IEnumerable<StoreCategoryListViewModel> categories,
        IEnumerable<StoreProductListItemViewModel> bestsellers,
        IEnumerable<StoreProductListItemViewModel> hotTips)
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
