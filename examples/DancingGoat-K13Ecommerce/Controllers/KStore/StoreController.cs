using CMS.Websites;

using DancingGoat;
using DancingGoat.Controllers.Store;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.K13Ecommerce.Products;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc;

[assembly:
    RegisterWebPageRoute(StorePage.CONTENT_TYPE_NAME, typeof(StoreController),
        WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers.Store;

public class StoreController : Controller
{
    private readonly StorePageRepository storePageRepository;
    private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
    private readonly IWebPageUrlRetriever urlRetriever;
    private readonly IProductService productService;


    public StoreController(StorePageRepository storePageRepository,
        IWebPageDataContextRetriever webPageDataContextRetriever,
        IWebPageUrlRetriever urlRetriever, IProductService productService)
    {
        this.storePageRepository = storePageRepository;
        this.webPageDataContextRetriever = webPageDataContextRetriever;
        this.urlRetriever = urlRetriever;
        this.productService = productService;
    }


    public async Task<IActionResult> Index()
    {
        var webPage = webPageDataContextRetriever.Retrieve().WebPage;

        var storePage = await storePageRepository.GetStorePage(webPage.WebPageItemID, webPage.LanguageName,
            HttpContext.RequestAborted);

        var categories = await storePageRepository.GetCategories(storePage, webPage.LanguageName)
            .SelectAwait(x => StoreCategoryListViewModel.GetViewModel(x, urlRetriever))
            .ToListAsync();

        var bestsellers = await GetBestsellers(webPage);

        var hotTips = await GetHottips(storePage, webPage);

        return View(StorePageViewModel.GetViewModel(storePage, categories, bestsellers, hotTips));
    }


    private async Task<IEnumerable<ProductListItemViewModel>> MapProductsToListViewModel(IEnumerable<ProductPage> products, ILookup<int, ProductPricesResponse> prices)
    {
        var productModels = new List<ProductListItemViewModel>();

        foreach (var product in products)
        {
            var productSku = product.Product.First();
            if (!prices[productSku.SKUID].Any())
            {
                continue;
            }
            var model = ProductListItemViewModel.GetViewModel(productSku,
                (await urlRetriever.Retrieve(product)).RelativePath, prices[productSku.SKUID].First().Prices);
            productModels.Add(model);
        }

        return productModels;
    }


    private async Task<IEnumerable<ProductListItemViewModel>> GetBestsellers(RoutedWebPage webPage)
    {

        var products = await storePageRepository.GetBestsellers(webPage.LanguageName);

        var prices = (await productService.GetProductsPrices(products.Select(p => p.Product.First().SKUID)))
            .ToLookup(p => p.ProductSkuId);

        return await MapProductsToListViewModel(products, prices);
    }


    private async Task<IEnumerable<ProductListItemViewModel>> GetHottips(StorePage storePage, RoutedWebPage webPage)
    {
        var products = await storePageRepository.GetHottips(storePage, webPage.LanguageName);

        var prices = (await productService.GetProductsPrices(products.Select(p => p.Product.First().SKUID)))
            .ToLookup(p => p.ProductSkuId);

        return await MapProductsToListViewModel(products, prices);
    }
}
