using CMS.Globalization;
using CMS.Websites;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.K13Ecommerce.Countries;
using Kentico.Xperience.K13Ecommerce.Customers;
using Kentico.Xperience.K13Ecommerce.ShoppingCart;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat;

public class CheckoutService : ICheckoutService
{
    private readonly IShoppingService shoppingService;
    private readonly ICountryService countryService;
    private readonly ICustomerService customerService;
    private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
    private readonly IWebPageUrlRetriever webPageUrlRetriever;
    private readonly CheckoutPageRepository checkoutPageRepository;
    private readonly IPreferredLanguageRetriever preferredLanguageRetriever;


    public CheckoutService(IShoppingService shoppingService,
        ICountryService countryService,
        ICustomerService customerService,
        IWebPageDataContextRetriever webPageDataContextRetriever,
        IWebPageUrlRetriever webPageUrlRetriever,
        CheckoutPageRepository checkoutPageRepository,
        IPreferredLanguageRetriever preferredLanguageRetriever
    )
    {
        this.shoppingService = shoppingService;
        this.countryService = countryService;
        this.customerService = customerService;
        this.webPageDataContextRetriever = webPageDataContextRetriever;
        this.webPageUrlRetriever = webPageUrlRetriever;
        this.checkoutPageRepository = checkoutPageRepository;
        this.preferredLanguageRetriever = preferredLanguageRetriever;
    }


    public async Task<CartContentViewModel> PrepareCartViewModel()
    {
        var cart = await shoppingService.GetCurrentShoppingCartContent();
        return new CartContentViewModel(cart, await checkoutPageRepository.GetProductImages(cart, preferredLanguageRetriever.Get()));
    }


    public async Task<DeliveryDetailsViewModel> PrepareDeliveryDetailsViewModel(CustomerViewModel customer = null,
        BillingAddressViewModel billingAddress = null, ShippingOptionViewModel shippingOption = null,
        KShoppingCartDetails cartDetails = null)
    {
        cartDetails ??= await shoppingService.GetCurrentShoppingCartDetails();
        var currency = (await shoppingService.GetCurrentShoppingCartContent()).Currency;

        var countries = await CreateCountryList();
        var shippingOptions =
            CreateShippingOptionList(cartDetails.AvailableShippingOptions, currency.CurrencyFormatString);

        customer ??= new CustomerViewModel(await shoppingService.GetCustomerOrCreateFromAuthenticatedUser(cartDetails.Customer));

        var addresses = (cartDetails.Customer != null)
            ? await customerService.GetCustomerAddresses(cartDetails.Customer.CustomerId)
            : Enumerable.Empty<KAddress>();

        var billingAddresses = new SelectList(addresses, nameof(KAddress.AddressId), nameof(KAddress.AddressName));

        billingAddress ??= await BillingAddressViewModel.GetModel(cartDetails.BillingAddress, countries,
            countryService, billingAddresses);
        var selectedShippingOption =
            cartDetails.AvailableShippingOptions!.FirstOrDefault(s =>
                s.ShippingOptionId == cartDetails.ShippingOptionId);

        shippingOption ??=
            new ShippingOptionViewModel(selectedShippingOption, shippingOptions, cartDetails.IsShippingNeeded);

        billingAddress.BillingAddressCountryStateSelector.Countries ??= countries;
        billingAddress.BillingAddressSelector ??= new AddressSelectorViewModel { Addresses = billingAddresses };
        shippingOption.ShippingOptions = shippingOptions;

        var pageContext = webPageDataContextRetriever.Retrieve().WebPage;
        var currentStepPage = await checkoutPageRepository.GetCartStepPage<CartDeliveryDetails>(
            pageContext.WebPageItemID,
            pageContext.LanguageName, pageContext.ContentTypeName);

        var viewModel = new DeliveryDetailsViewModel
        {
            Customer = customer,
            BillingAddress = billingAddress,
            ShippingOption = shippingOption,
            CartPreviousStepUrl =
                await GetNextOrPreviousStepUrl<CartDeliveryDetails>(s => s.CartPreviousStep.First().WebPageGuid)
        };

        return viewModel;
    }


    public async Task<bool> IsCountryValid(int countryId) => await countryService.GetCountry(countryId) != null;


    public async Task<bool> IsStateValid(int countryId, int? stateId)
    {
        var states = (await countryService.GetCountryStates(countryId)).ToList();

        return (states.Count < 1) || states.Exists(s => s.StateID == stateId);
    }


    public async Task<KAddress> GetAddress(int customerId, int addressId) =>
        customerId > 0 && addressId > 0
            ? (await customerService.GetCustomerAddresses(customerId)).FirstOrDefault(a => a.AddressId == addressId)
            : null;


    public async Task<string> GetNextOrPreviousStepUrl<TCurrentStep>(Func<TCurrentStep, Guid> nextOrPreviousStepFunc)
        where TCurrentStep : IWebPageFieldsSource, new()
    {
        var pageContext = webPageDataContextRetriever.Retrieve().WebPage;
        var currentStepPage = await checkoutPageRepository.GetCartStepPage<TCurrentStep>(pageContext.WebPageItemID,
            pageContext.LanguageName, pageContext.ContentTypeName);
        string nextStepUrl =
            (await webPageUrlRetriever.Retrieve(nextOrPreviousStepFunc(currentStepPage), pageContext.LanguageName))
            .RelativePath;

        return nextStepUrl;
    }

    /// <summary>
    /// Creates view model for checkout preview step.
    /// </summary>
    /// <param name="paymentMethod">Payment method selected on preview step</param>
    public async Task<SummaryViewModel> PreparePreviewViewModel(PaymentMethodViewModel paymentMethod = null)
    {
        var cart = await shoppingService.GetCurrentShoppingCartSummaryAsync();
        var billingAddress = cart.CartDetails.BillingAddress;

        var shippingOption = cart.CartDetails.ShippingOption;

        var paymentMethods = new SelectList(cart.CartDetails.AvailablePaymentOptions,
            nameof(KPaymentOption.PaymentOptionId), nameof(KPaymentOption.PaymentOptionDisplayName));

        var paymentOption = cart.CartDetails.PaymentOption;

        paymentMethod ??= new PaymentMethodViewModel(paymentOption, paymentMethods);

        // PaymentMethods are excluded from automatic binding and must be recreated manually after post action
        paymentMethod.PaymentMethods ??= paymentMethods;

        var deliveryDetailsModel = new DeliveryDetailsViewModel
        {
            Customer = new CustomerViewModel(cart.CartDetails.Customer),
            BillingAddress = await BillingAddressViewModel.GetModel(billingAddress, null, countryService),
            ShippingOption = new ShippingOptionViewModel(shippingOption, null, cart.CartDetails.IsShippingNeeded)
        };

        var cartModel = new CartContentViewModel(cart.CartContent, await checkoutPageRepository.GetProductImages(cart.CartContent, preferredLanguageRetriever.Get()));

        var viewModel = new SummaryViewModel
        {
            CartModel = cartModel,
            DeliveryDetails = deliveryDetailsModel,
            ShippingName = shippingOption?.ShippingOptionDisplayName ?? "",
            PaymentMethod = paymentMethod,
            CartPreviousStepUrl = await GetNextOrPreviousStepUrl<CartSummary>(s => s.CartPreviousStep.First().WebPageGuid)
        };

        return viewModel;
    }

    private async Task<SelectList> CreateCountryList()
    {
        var allCountries = await countryService.GetAllCountries();
        return new SelectList(allCountries, nameof(CountryInfo.CountryID), nameof(CountryInfo.CountryDisplayName));
    }

    private SelectList CreateShippingOptionList(IEnumerable<KShippingOption> shippingOptions,
        string currencyFormatString)
    {
        var selectList = shippingOptions.Select(s =>
        {
            return new SelectListItem
            {
                Value = s.ShippingOptionId.ToString(),
                Text = $"{s.ShippingOptionDisplayName} ({string.Format(currencyFormatString, s.ShippingOptionPrice)})"
            };
        });

        return new SelectList(selectList, "Value", "Text");
    }
}
