using Kentico.Xperience.K13Ecommerce.Orders;
using Kentico.Xperience.K13Ecommerce.ShoppingCart;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers;
#if DEBUG
[Route("[controller]/[action]")]
public class TestController(IKenticoStoreApiClient storeApiClient, IShoppingService shoppingService) : Controller
{
    public async Task<IActionResult> TestProducts()
    {
        var products = await storeApiClient.GetProductPagesAsync(path: "/", limit: 12);
        return Ok(products);
    }

    public async Task<IActionResult> TestCart()
    {
        var cart = await storeApiClient.GetCurrentCartContentAsync();
        return Ok(cart);
    }

    public async Task<IActionResult> TestSetCurrency(string currencyCode)
    {
        await shoppingService.SetCurrency(currencyCode);
        return await TestCart();
    }

    public async Task<JsonResult> TestOrders([FromServices] IOrderService orderService)
        => Json(await orderService.GetOrderList(new OrderListRequest { Page = 1, PageSize = 10, OrderBy = "OrderID DESC" }));
}
#endif
