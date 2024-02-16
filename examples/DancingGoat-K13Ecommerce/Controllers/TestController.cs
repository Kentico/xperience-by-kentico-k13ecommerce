using Kentico.Xperience.K13Ecommerce.KenticoStoreApi;
using Kentico.Xperience.K13Ecommerce.Products;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers;

[Route("[controller]/[action]")]
public class TestController : Controller
{
    private readonly IKStoreApiService storeApiService;

    public TestController(IKStoreApiService storeApiService)
    {
        this.storeApiService = storeApiService;
    }

    public async Task<IActionResult> TestProducts()
    {
        var products = await storeApiService.GetProductPages(new ProductPageRequest { Path = "/", Limit = 12});
        return Ok(products);
    }
}
