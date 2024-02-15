using DancingGoat.Components.Widgets.Store.ProductListWidget;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.K13Ecommerce.KenticoStoreApi;
using Kentico.Xperience.K13Ecommerce.Products;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(
    identifier: StoreProductListWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(StoreProductListWidgetViewComponent), name: "K13 Store product list",
    propertiesType: typeof(StoreProductListWidgetProperties),
    Description = "Display products from Kentico 13 E-commerce", IconClass = "icon-chain")]

namespace DancingGoat.Components.Widgets.Store.ProductListWidget;

public class StoreProductListWidgetViewComponent(IKStoreApiService storeApiService) : ViewComponent
{
    public const string IDENTIFIER = "DancingGoat.LandingPage.KenticoStoreProductList";

    public async Task<IViewComponentResult> InvokeAsync(StoreProductListWidgetProperties properties)
    {
        //@TODO cache results
        var products = await storeApiService.GetProductPages(properties.Path, properties.Culture, properties.CurrencyCode,
            orderBy: properties.OrderBy, limit: properties.Limit);

        string currencyFormatString = products.FirstOrDefault()?.Sku?.Prices?.Currency?.CurrencyFormatString;
        var model = new StoreProductListWidgetViewModel
        {
            Title = properties.Title,
            CurrencyFormatString = currencyFormatString,
            Products = products.Select(p => new ProductListModel
            {
                Name = p.DocumentSKUName,
                ShortDescription = p.DocumentSKUShortDescription,
                ProductUrl = p.AbsoluteUrl,
                Image = p.Sku?.MainImageUrl,
                ImageAlt = p.DocumentSKUName,
                Price = p.Sku?.Prices?.Price,
                ListPrice = p.Sku?.Prices?.ListPrice
            }).ToList()
        };
        return View("~/Components/Widgets/Store/ProductListWidget/_StoreProductListWidget.cshtml", model);
    }
}