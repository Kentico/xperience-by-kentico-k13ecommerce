using CMS.Websites;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.K13Ecommerce.ShoppingCart;

using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.ViewComponents.ShoppingCartPreview;

public class ShoppingCartPreviewViewComponent(
    IShoppingService shoppingService,
    CheckoutPageRepository checkoutPageRepository,
    IWebPageUrlRetriever webPageUrlRetriever,
    IPreferredLanguageRetriever currentLanguageRetriever)
    : ViewComponent
{
    public IPreferredLanguageRetriever CurrentLanguageRetriever { get; } = currentLanguageRetriever;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cartContentPage = await checkoutPageRepository.GetCartContentPage(CurrentLanguageRetriever.Get());

        string checkoutUrl = cartContentPage != null ?
            (await webPageUrlRetriever.Retrieve(cartContentPage)).RelativePath :
            string.Empty;

        int totalUnits =
            (await shoppingService.GetCurrentShoppingCartContent()).CartProducts!.Sum(p => p.CartItemUnits);

        var model = new ShoppingCartPreviewViewModel { CartUrl = checkoutUrl, CartTotalUnits = totalUnits };
        return View("~/Components/ViewComponents/ShoppingCartPreview/Default.cshtml", model);
    }
}
