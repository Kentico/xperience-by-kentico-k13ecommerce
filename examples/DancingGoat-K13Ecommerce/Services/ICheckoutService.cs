using CMS.Websites;

using DancingGoat.Models;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace DancingGoat;

public interface ICheckoutService
{
    /// <summary>
    /// Creates view model for Shopping cart step.
    /// </summary>
    Task<CartContentViewModel> PrepareCartViewModel();


    /// <summary>
    /// Creates view model for checkout delivery step.
    /// </summary>
    /// <param name="customer">Filled customer details</param>
    /// <param name="billingAddress">Filled billing address</param>
    /// <param name="shippingOption">Selected shipping option</param>
    /// <param name="cartDetails"></param>
    Task<DeliveryDetailsViewModel> PrepareDeliveryDetailsViewModel(CustomerViewModel customer = null,
        BillingAddressViewModel billingAddress = null, ShippingOptionViewModel shippingOption = null,
        KShoppingCartDetails cartDetails = null);


    /// <summary>
    /// Checks if country exists.
    /// </summary>
    /// <param name="countryId">ID of country which should be checked</param>
    /// <returns>Return true if country exists.</returns>
    Task<bool> IsCountryValid(int countryId);


    /// <summary>
    /// Checks if state is valid for given country.
    /// </summary>
    /// <param name="countryId">ID of state`s country </param>
    /// <param name="stateId">ID of state</param>
    /// <returns>True if state is not required or state belongs to the country.</returns>
    Task<bool> IsStateValid(int countryId, int? stateId);


    /// <summary>
    /// Gets the address with the specified identifier.
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="addressId"></param>
    Task<KAddress> GetAddress(int customerId, int addressId);


    Task<string> GetNextOrPreviousStepUrl<TCurrentStep>(Func<TCurrentStep, Guid> nextOrPreviousStepFunc)
        where TCurrentStep : IWebPageFieldsSource, new();


    /// <summary>
    /// Creates view model for checkout preview step.
    /// </summary>
    /// <param name="paymentMethod">Payment method selected on preview step</param>
    Task<SummaryViewModel> PreparePreviewViewModel(PaymentMethodViewModel paymentMethod = null);
}
