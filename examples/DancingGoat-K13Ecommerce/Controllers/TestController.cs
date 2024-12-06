using Kentico.Xperience.K13Ecommerce.Orders;
using Kentico.Xperience.K13Ecommerce.Products;
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
        => Json(await orderService.GetCurrentCustomerOrderList(new OrderListRequest { Page = 1, PageSize = 10, OrderBy = "OrderID DESC" }));

    public async Task<JsonResult> TestOrderStatuses([FromServices] IOrderService orderService)
    {
        var statuses = await orderService.GetOrderStatuses();
        return Json(statuses);
    }

    public async Task<IActionResult> TestUpdateOrder([FromServices] IOrderService orderService)
    {
        var orders =
            await orderService.GetAdminOrderList(new OrderListRequest { Page = 1, PageSize = 10, OrderBy = "OrderID DESC" });

        var order = orders.Orders.First(o => o.OrderId == 25);

        order.OrderGrandTotal = 999;
        order.OrderIsPaid = true;
        var newStatus = (await orderService.GetOrderStatuses()).First(o => o.StatusName == "Completed");
        order.OrderStatus = newStatus;

        order.OrderBillingAddress.AddressLine1 = "123 Main St";
        order.OrderBillingAddress.AddressCity = "New York";

        order.OrderShippingAddress.AddressZip = "10001";

        order.OrderShippingOption = new KShippingOption { ShippingOptionId = 2 };
        order.OrderPaymentOption = new KPaymentOption { PaymentOptionId = 1 };

        order.OrderPaymentResult = new KPaymentResult
        {
            PaymentIsCompleted = true,
            PaymentMethodName = "Test",
            PaymentStatusName = "Test status"
        };

        await orderService.UpdateOrder(order);

        return Ok();
    }

    public async Task<IActionResult> TestCartCacheIssue([FromServices] IShoppingService shoppingService, [FromServices] IProductService productService)
    {
        //first ensure that ShoppingCartGUID cookie is set to existing cart from previous session

        var prices = await productService.GetProductPrices(33);//set real SKUID
                                                               // now empty cart is cached in K13

        var cart = await shoppingService.GetCurrentShoppingCartContent();

        // now when cart is empty and cookie is removed -> invalid

        return Json(cart);
    }
}
#endif
