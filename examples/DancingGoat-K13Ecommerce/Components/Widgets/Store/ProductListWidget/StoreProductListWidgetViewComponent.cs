using CMS.Helpers;

using DancingGoat.Components.Widgets.Store.ProductListWidget;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.K13Ecommerce.Products;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(
    identifier: StoreProductListWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(StoreProductListWidgetViewComponent), name: "K13 Store product list",
    propertiesType: typeof(StoreProductListWidgetProperties),
    Description = "Display products from Kentico 13 E-commerce", IconClass = "icon-chain")]

namespace DancingGoat.Components.Widgets.Store.ProductListWidget;

/// <summary>
/// Use this widget when you want to directly show product data from K13 Store
/// </summary>
/// <param name="storeApiClient"></param>
public class StoreProductListWidgetViewComponent(IKenticoStoreApiClient storeApiClient, IProgressiveCache cache)
    : ViewComponent
{
    public const string IDENTIFIER = "DancingGoat.LandingPage.KenticoStoreProductList";

    public async Task<IViewComponentResult> InvokeAsync(StoreProductListWidgetProperties properties)
    {
        var products = await cache.LoadAsync(async _ =>
                !string.IsNullOrWhiteSpace(properties.Path) ? await storeApiClient.GetProductPagesAsync(path: properties.Path,
                    culture: properties.Culture,
                    currency: properties.CurrencyCode,
                    orderBy: properties.OrderBy,
                    limit: properties.Limit
                ) : Array.Empty<KProductNode>(),
            new CacheSettings(5, "productpages", properties.Path, properties.Limit, properties.CurrencyCode,
                properties.Culture, properties.OrderBy));

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
