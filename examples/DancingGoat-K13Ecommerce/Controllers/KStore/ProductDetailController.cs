using DancingGoat;
using DancingGoat.Controllers.Store;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.K13Ecommerce.Products;

using Microsoft.AspNetCore.Mvc;

[assembly:
    RegisterWebPageRoute(ProductPage.CONTENT_TYPE_NAME, typeof(ProductDetailController),
        WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers.Store;

public class ProductDetailController : Controller
{
    private readonly ProductPageRepository productPageRepository;
    private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
    private readonly IProductService productService;


    public ProductDetailController(ProductPageRepository productPageRepository,
        IWebPageDataContextRetriever webPageDataContextRetriever,
        IProductService productService)
    {
        this.productPageRepository = productPageRepository;
        this.webPageDataContextRetriever = webPageDataContextRetriever;
        this.productService = productService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var webPage = webPageDataContextRetriever.Retrieve().WebPage;
        var productPage = await productPageRepository.GetProductDetailPage(webPage.WebPageItemID, webPage.LanguageName);

        if (productPage == null || !productPage.Product.Any())
        {
            return NotFound();
        }

        var productSku = productPage.Product.First();
        var prices = await productService.GetProductPrices(productSku.SKUID);

        var model = ProductPageViewModel.GetViewModel(productSku, prices);
        return View(model);
    }
}
