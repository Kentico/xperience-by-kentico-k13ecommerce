using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using CMS.Helpers;
using CMS.Websites;

using DancingGoat;
using DancingGoat.Controllers.KStore;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.K13Ecommerce.Countries;
using Kentico.Xperience.K13Ecommerce.ShoppingCart;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using ProblemDetails = Kentico.Xperience.K13Ecommerce.StoreApi.ProblemDetails;

[assembly:
    RegisterWebPageRoute(CartContent.CONTENT_TYPE_NAME, typeof(CheckoutController), ActionName = nameof(CheckoutController.CartContent),
        WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

[assembly:
    RegisterWebPageRoute(CartDeliveryDetails.CONTENT_TYPE_NAME, typeof(CheckoutController),
        ActionName = nameof(CheckoutController.CartDeliveryDetails),
        WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

[assembly:
    RegisterWebPageRoute(CartSummary.CONTENT_TYPE_NAME, typeof(CheckoutController),
        ActionName = nameof(CheckoutController.CartSummary),
        WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

[assembly:
    RegisterWebPageRoute(OrderComplete.CONTENT_TYPE_NAME, typeof(CheckoutController),
        ActionName = nameof(CheckoutController.ThankYou),
        WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers.KStore;

[Route("Checkout")]
public class CheckoutController : Controller
{
    private readonly CheckoutPageRepository checkoutPageRepository;
    private readonly ICountryService countryService;
    private readonly IPreferredLanguageRetriever preferredLanguageRetriever;
    private readonly ILogger<CheckoutController> logger;
    private readonly ICheckoutService checkoutService;
    private readonly IWebPageUrlRetriever webPageUrlRetriever;
    private readonly IShoppingService shoppingService;


    public CheckoutController(
        ICheckoutService checkoutService,
        IWebPageUrlRetriever webPageUrlRetriever,
        IShoppingService shoppingService,
        CheckoutPageRepository checkoutPageRepository,
        ICountryService countryService,
        IPreferredLanguageRetriever preferredLanguageRetriever,
        ILogger<CheckoutController> logger)
    {
        this.checkoutPageRepository = checkoutPageRepository;
        this.countryService = countryService;
        this.preferredLanguageRetriever = preferredLanguageRetriever;
        this.logger = logger;
        this.checkoutService = checkoutService;
        this.webPageUrlRetriever = webPageUrlRetriever;
        this.shoppingService = shoppingService;
    }


    [HttpGet]
    public async Task<IActionResult> CartContent()
    {
        var model = await checkoutService.PrepareCartViewModel();

        return View(model);
    }


    [HttpPost("cart-content-checkout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CartContentCheckout()
    {
        var validationErrors = await shoppingService.ValidateShoppingCartItems();
        
        if (validationErrors.Any())
        {
            ProcessCheckResult(validationErrors);
            var model = await checkoutService.PrepareCartViewModel();
            return View("CartContent", model);
        }
        
        string deliveryDetailsPageUrl = await checkoutService.GetNextOrPreviousStepUrl<CartContent>(s => s.CartNextStep.First().WebPageGuid);
        return Redirect(deliveryDetailsPageUrl);
    }
    
    
    private void ProcessCheckResult(IEnumerable<KShoppingCartItemValidationError> validationErrors)
    {
        var itemErrors = validationErrors
            .GroupBy(g => g.SkuId);

        foreach (var errorGroup in itemErrors)
        {
            string errors = errorGroup.Select(e => e.Message).Join(", ");

            ModelState.AddModelError(errorGroup.Key.ToString(), errors);
        }
    }


    [HttpPost("add-item")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem([FromForm] int skuId, [FromForm][Range(0, short.MaxValue)] int units)
    {
        if (ModelState.IsValid)
        {
            await shoppingService.AddItemToCart(skuId, units);
        }
        return await RedirectToCartContentPage();
    }


    [HttpPost("update-item")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateItem(
        [FromForm] int cartItemId,
        [FromForm][Range(0, short.MaxValue)] int units)
    {
        if (ModelState.IsValid)
        {
            await shoppingService.UpdateItemQuantity(cartItemId, units);
        }
        return await RedirectToCartContentPage();
    }


    [HttpPost("remove-item")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItem([FromForm] int cartItemId)
    {
        await shoppingService.RemoveItemFromCart(cartItemId);
        return await RedirectToCartContentPage();
    }


    [HttpGet("item-detail")]
    public async Task<IActionResult> ItemDetail(int skuId)
    {
        var productPage = await checkoutPageRepository.GetProductPageBySKUID(skuId, preferredLanguageRetriever.Get());
        if (productPage is null)
        {
            return NotFound();
        }

        return Redirect((await webPageUrlRetriever.Retrieve(productPage)).RelativePath);
    }


    [HttpPost("add-coupon-code")]
    public async Task<IActionResult> AddCouponCode([FromForm][Required] string couponCode)
    {
        await shoppingService.AddCouponCode(couponCode);
        return await RedirectToCartContentPage();
    }


    [HttpPost("remove-coupon-code")]
    public async Task<IActionResult> RemoveCouponCode([FromForm][Required] string couponCode)
    {
        await shoppingService.RemoveCouponCode(couponCode);
        return await RedirectToCartContentPage();
    }


    [HttpGet]
    public async Task<IActionResult> CartDeliveryDetails()
    {
        var cart = await shoppingService.GetCurrentShoppingCartContent();
        if (cart.IsEmpty)
        {
            return await RedirectToCartContentPage();
        }

        var model = await checkoutService.PrepareDeliveryDetailsViewModel();
        return View(model);
    }


    [HttpPost("cart-delivery-details")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CartDeliveryDetails(DeliveryDetailsViewModel model,
        [FromServices] IStringLocalizer<SharedResources> localizer)
    {
        var cartDetails = await shoppingService.GetCurrentShoppingCartDetails();

        // Check the selected shipping option
        if (cartDetails.IsShippingNeeded &&
            !cartDetails.AvailableShippingOptions.Any(s => s.ShippingOptionId == model.ShippingOption.ShippingOptionID))
        {
            ModelState.AddModelError("ShippingOption.ShippingOptionID", localizer["Please select a delivery method"]);
        }

        // Check if the billing address's country and state are valid
        var countryStateViewModel = model.BillingAddress.BillingAddressCountryStateSelector;
        if (!await checkoutService.IsCountryValid(countryStateViewModel.CountryID))
        {
            countryStateViewModel.CountryID = 0;
            ModelState.AddModelError("BillingAddress.BillingAddressCountryStateSelector.CountryID",
                localizer["The Country field is required"]);
        }
        else if (!await checkoutService.IsStateValid(countryStateViewModel.CountryID, countryStateViewModel.StateID))
        {
            countryStateViewModel.StateID = 0;
            ModelState.AddModelError("BillingAddress.BillingAddressCountryStateSelector.StateID",
                localizer["The State field is required"]);
        }

        if (!ModelState.IsValid)
        {
            var viewModel =
                await checkoutService.PrepareDeliveryDetailsViewModel(model.Customer, model.BillingAddress,
                    model.ShippingOption);

            return View(viewModel);
        }

        var customer = await shoppingService.GetCustomerOrCreateFromAuthenticatedUser() ?? new KCustomer();
        bool emailCanBeChanged =
            !User.Identity?.IsAuthenticated == true || string.IsNullOrWhiteSpace(customer.CustomerEmail);
        model.Customer.ApplyToCustomer(customer, emailCanBeChanged);

        int modelAddressID = model.BillingAddress.BillingAddressSelector?.AddressID ?? 0;
        var billingAddress = await checkoutService.GetAddress(customer.CustomerId, modelAddressID) ?? new KAddress();

        model.BillingAddress.ApplyTo(billingAddress);
        billingAddress.AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}";

        try
        {
            await shoppingService.SetDeliveryDetails(new KShoppingCartDeliveryDetails
            {
                Customer = customer,
                BillingAddress = billingAddress,
                ShippingOptionId = model.ShippingOption?.ShippingOptionID ?? 0
            });
        }
        catch (ApiException<ProblemDetails> ex)
        {
            logger.LogError(ex, "Error during updating delivery details");
            var viewModel = await checkoutService.PrepareDeliveryDetailsViewModel(model.Customer, model.BillingAddress,
                    model.ShippingOption);
            ModelState.AddModelError(string.Empty, ex.Result.Detail ?? ex.Result.Title!);
            return View(viewModel);
        }

        string summaryStepUrl = await checkoutService.GetNextOrPreviousStepUrl<CartDeliveryDetails>(s => s.CartNextStep.First().WebPageGuid);
        return Redirect(summaryStepUrl);
    }


    [HttpPost("customer-address")]
    public async Task<JsonResult> CustomerAddress(int addressID)
    {
        var customer = (await shoppingService.GetCurrentShoppingCartDetails()).Customer;
        var address = customer != null ? await checkoutService.GetAddress(customer.CustomerId, addressID) : null;

        if (address == null)
        {
            return null;
        }

        var responseModel = new
        {
            Line1 = address.AddressLine1,
            Line2 = address.AddressLine2,
            City = address.AddressCity,
            PostalCode = address.AddressZip,
            CountryID = address.AddressCountryId,
            StateID = address.AddressStateId,
            PersonalName = address.AddressPersonalName
        };

        return Json(responseModel);
    }


    [HttpPost("country-states")]
    public async Task<JsonResult> CountryStates([FromForm] int countryId)
    {
        var responseModel = (await countryService.GetCountryStates(countryId))
            .Select(s => new { id = s.StateID, name = HTMLHelper.HTMLEncode(s.StateDisplayName) });

        return Json(responseModel);
    }


    [HttpGet]
    public async Task<IActionResult> CartSummary()
    {
        var cart = await shoppingService.GetCurrentShoppingCartContent();
        if (cart.IsEmpty)
        {
            return await RedirectToCartContentPage();
        }

        var viewModel = await checkoutService.PreparePreviewViewModel();
        return View(viewModel);
    }


    [HttpPost("finish-order")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> FinishOrder(SummaryViewModel model, [FromServices] IStringLocalizer<SharedResources> localizer)
    {
        var cart = await shoppingService.GetCurrentShoppingCartSummaryAsync();

        if (cart.CartContent.IsEmpty)
        {
            return await RedirectToCartContentPage();
        }

        if (!cart.CartDetails.AvailablePaymentOptions.Any(p => p.PaymentOptionId == model.PaymentMethod.PaymentMethodID))
        {
            ModelState.AddModelError("PaymentMethod.PaymentMethodID", localizer["Select payment method"]);
        }
        else
        {
            await shoppingService.SetPaymentOption(model.PaymentMethod.PaymentMethodID);
        }

        if (!ModelState.IsValid)
        {
            var viewModel = await checkoutService.PreparePreviewViewModel(model.PaymentMethod);
            return View("CartSummary", viewModel);
        }

        try
        {
            await shoppingService.CreateOrder();
        }
        catch (ApiException<ProblemDetails> ex)
        {
            // Example of formatted message in ex.Result.Detail: 'Order creation failed with following validation error: SKUDisabledOrExpiredValidationError.'
            // for more handsome validation messages standalone validation endpoint should be considered
            logger.LogError(ex, "Error during creating order. Detail: {JSON}: ", JsonSerializer.Serialize(ex.Result));
            ModelState.AddModelError(string.Empty, ex.Result.Detail ?? ex.Result.Title!);
            var viewModel = await checkoutService.PreparePreviewViewModel(model.PaymentMethod);
            return View("CartSummary", viewModel);
        }

        string orderCompletedUrl = await checkoutService.GetNextOrPreviousStepUrl<CartSummary>(s => s.CartNextStep.First().WebPageGuid);
        return Redirect(orderCompletedUrl);
    }


    public async Task<ActionResult> ThankYou([FromServices] ContactRepository contactRepository, [FromServices] IPreferredLanguageRetriever currentLanguageRetriever)
    {
        string language = currentLanguageRetriever.Get();
        var companyContact = await contactRepository.GetContact(language);

        var viewModel = new ThankYouViewModel
        {
            Phone = companyContact.ContactPhone
        };

        return View(viewModel);
    }


    private async Task<RedirectResult> RedirectToCartContentPage()
    {
        var cartContentPage = await checkoutPageRepository.GetCartContentPage(preferredLanguageRetriever.Get());
        return cartContentPage is null
            ? throw new InvalidOperationException("No cart content page found")
            : Redirect((await webPageUrlRetriever.Retrieve(cartContentPage)).RelativePath);
    }
}
