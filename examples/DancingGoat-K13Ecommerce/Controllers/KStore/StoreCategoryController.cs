using CMS.Websites;

using DancingGoat;
using DancingGoat.Controllers.Store;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.K13Ecommerce.Products;

using Microsoft.AspNetCore.Mvc;

[assembly:
    RegisterWebPageRoute(CategoryPage.CONTENT_TYPE_NAME, typeof(StoreCategoryController),
        WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers.Store;

public class StoreCategoryController : Controller
{
    private readonly CategoryPageRepository categoryPageRepository;
    private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
    private readonly IWebPageUrlRetriever urlRetriever;
    private readonly IProductService productService;


    public StoreCategoryController(CategoryPageRepository categoryPageRepository,
        IWebPageDataContextRetriever webPageDataContextRetriever,
        IWebPageUrlRetriever urlRetriever,
        IProductService productService)
    {
        this.categoryPageRepository = categoryPageRepository;
        this.webPageDataContextRetriever = webPageDataContextRetriever;
        this.urlRetriever = urlRetriever;
        this.productService = productService;
    }


    // GET
    public async Task<IActionResult> Index()
    {
        var webPage = webPageDataContextRetriever.Retrieve().WebPage;
        var categoryPage = await categoryPageRepository.GetCategoryPage(webPage.WebPageItemID, webPage.LanguageName, HttpContext.RequestAborted);

        var products = await categoryPageRepository.GetCategoryProducts(categoryPage, webPage.LanguageName,
            HttpContext.RequestAborted);

        var prices = (await productService.GetProductsPrices(products.Select(p => p.Product.First().SKUID)))
            .ToLookup(p => p.ProductSkuId);

        var productModels = new List<ProductListItemViewModel>();
        foreach (var productPage in products)
        {
            var productSku = productPage.Product.First();
            if (!prices[productSku.SKUID].Any())
            {
                continue;
            }
            var model = ProductListItemViewModel.GetViewModel(productSku,
                (await urlRetriever.Retrieve(productPage)).RelativePath, prices[productSku.SKUID].First().Prices);
            productModels.Add(model);
        }

        var viewModel = CategoryPageViewModel.GetViewModel(categoryPage, productModels);
        return View(viewModel);
    }
}
