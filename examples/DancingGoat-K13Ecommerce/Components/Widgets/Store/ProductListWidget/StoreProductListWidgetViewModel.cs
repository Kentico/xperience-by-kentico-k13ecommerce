using Kentico.Xperience.K13Ecommerce.Products;

namespace DancingGoat.Components.Widgets.Store.ProductListWidget;

public class StoreProductListWidgetViewModel
{
    public string Title { get; set; }
    public IReadOnlyList<ProductListModel> Products { get; set; }
    public string CurrencyFormatString { get; set; }
}
