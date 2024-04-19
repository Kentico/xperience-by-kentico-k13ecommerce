using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using AutoMapper;

using CMS.Base;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Membership;

using Kentico.Xperience.StoreApi.Authentication;
using Kentico.Xperience.StoreApi.Currencies;
using Kentico.Xperience.StoreApi.Customers;
using Kentico.Xperience.StoreApi.Orders;
using Kentico.Xperience.StoreApi.Routing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.ShoppingCart;

#pragma warning disable IDE0060 // Remove unused parameter
[ApiController]
[AuthorizeStore]
[Route($"{ApiRoute.ApiPrefix}/cart")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingService shoppingService;
    private readonly IMapper mapper;
    private readonly ISiteService siteService;
    private readonly IShippingOptionInfoProvider shippingOptionInfoProvider;
    private readonly IPaymentOptionInfoProvider paymentOptionInfoProvider;
    private readonly ICurrencyInfoProvider currencyInfoProvider;


    public ShoppingCartController(IShoppingService shoppingService, IMapper mapper, ISiteService siteService,
        IShippingOptionInfoProvider shippingOptionInfoProvider, IPaymentOptionInfoProvider paymentOptionInfoProvider,
        ICurrencyInfoProvider currencyInfoProvider)
    {
        this.shoppingService = shoppingService;
        this.mapper = mapper;
        this.siteService = siteService;
        this.shippingOptionInfoProvider = shippingOptionInfoProvider;
        this.paymentOptionInfoProvider = paymentOptionInfoProvider;
        this.currencyInfoProvider = currencyInfoProvider;
    }


    /// <summary>
    /// Returns current cart content for cart preview or cart content step
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <returns></returns>
    [HttpGet("content", Name = nameof(GetCurrentCartContent))]
    public ActionResult<KShoppingCartContent> GetCurrentCartContent(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid)
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        return mapper.Map<KShoppingCartContent>(cart);
    }


    /// <summary>
    /// Returns current cart details for cart delivery details step
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <returns></returns>
    [HttpGet("details", Name = nameof(GetCurrentCartDetails))]
    public ActionResult<KShoppingCartDetails> GetCurrentCartDetails(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid)
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        var cartDetails = GetCartDetails(cart);
        return Ok(cartDetails);
    }


    /// <summary>
    /// Returns current customer assigned to current cart
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <returns></returns>
    [HttpGet("customer", Name = nameof(GetCurrentCustomer))]
    public ActionResult<ShoppingCartResponse<KCustomer>> GetCurrentCustomer(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid)
    {
        var customerInfo = shoppingService.GetCurrentCustomer();
        var cart = shoppingService.GetCurrentShoppingCart();
        return Ok(new ShoppingCartResponse<KCustomer>
        {
            ShoppingCartGuid = cart.ShoppingCartGUID,
            Value = mapper.Map<KCustomer>(customerInfo)
        });
    }


    /// <summary>
    /// Adds item to cart
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="skuId"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    [HttpPost("add-item", Name = nameof(AddItemToCart))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartResponse<KShoppingCartItem>> AddItemToCart(
        [FromHeader(Name = "ShoppingCartGUID")] Guid shoppingCartGuid,
        [FromQuery] int skuId, [FromQuery][Range(1, short.MaxValue)] int quantity) =>
        TryCatch<ShoppingCartResponse<KShoppingCartItem>>(() =>
        {
            var result = shoppingService.AddItemToCart(skuId, quantity);
            if (result is null)
            {
                return UnprocessableEntity();
            }

            var cart = shoppingService.GetCurrentShoppingCart();
            return Ok(new ShoppingCartResponse<KShoppingCartItem>
            {
                ShoppingCartGuid = cart.ShoppingCartGUID,
                Value = mapper.Map<KShoppingCartItem>(result)
            });
        });


    /// <summary>
    /// Updates cart item quantity
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="itemId"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    [HttpPut("update-item", Name = nameof(UpdateItemQuantity))]
    public ActionResult<ShoppingCartBaseResponse> UpdateItemQuantity(
        [Required] [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid,
        [FromQuery][Required] int itemId, [FromQuery][Required][Range(0, short.MaxValue)] int quantity)
    {
        shoppingService.UpdateItemQuantity(itemId, quantity);
        return Ok(GetBaseResponse());
    }


    /// <summary>
    /// Removes item from cart
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    [HttpDelete("remove-item", Name = nameof(RemoveItemFromCart))]
    public ActionResult<ShoppingCartBaseResponse> RemoveItemFromCart(
        [Required] [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid,
        [FromQuery][Required] int itemId)
    {
        shoppingService.RemoveItemFromCart(itemId);
        return Ok(GetBaseResponse());
    }


    /// <summary>
    /// Adds coupon code to cart
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="couponCode"></param>
    /// <returns></returns>
    [HttpPost("add-coupon-code")]
    public ActionResult<ShoppingCartResponse<bool>> AddCouponCode(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid, [FromQuery][Required] string couponCode)
    {
        bool success = shoppingService.AddCouponCode(couponCode);
        var cart = shoppingService.GetCurrentShoppingCart();
        return new ShoppingCartResponse<bool> { ShoppingCartGuid = cart.ShoppingCartGUID, Value = success };
    }


    /// <summary>
    /// Removes coupon code from cart
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="couponCode"></param>
    /// <returns></returns>
    [HttpDelete("remove-coupon-code")]
    public ActionResult<ShoppingCartBaseResponse> RemoveCouponCode(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid, [FromQuery][Required] string couponCode)
    {
        shoppingService.RemoveCouponCode(couponCode);
        return Ok(GetBaseResponse());
    }


    /// <summary>
    /// Set shipping option
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="shippingOptionId"></param>
    /// <returns></returns>
    [HttpPut("set-shipping-option")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetShippingOption(
        [FromHeader(Name = "ShoppingCartGUID")][Required] Guid shoppingCartGuid,
        [FromQuery][Required] int shippingOptionId) =>
        TryCatch<ShoppingCartBaseResponse>(() =>
        {
            shoppingService.SetShippingOption(shippingOptionId);
            return Ok(GetBaseResponse());
        });


    /// <summary>
    /// Set payment option
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="paymentOptionId"></param>
    /// <returns></returns>
    [HttpPut("set-payment-option")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetPaymentOption(
        [FromHeader(Name = "ShoppingCartGUID")][Required] Guid shoppingCartGuid,
        [FromQuery][Required] int paymentOptionId) =>
        TryCatch<ShoppingCartBaseResponse>(() =>
        {
            shoppingService.SetPaymentOption(paymentOptionId);
            return Ok(GetBaseResponse());
        });


    /// <summary>
    /// Set shipping and payment
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="shippingOptionId"></param>
    /// <param name="paymentOptionId"></param>
    /// <returns></returns>
    [HttpPut("set-shipping-and-payment")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetShippingAndPayment(
        [FromHeader(Name = "ShoppingCartGUID")][Required] Guid shoppingCartGuid,
        [FromQuery][Required] int shippingOptionId, [FromQuery][Required] int paymentOptionId) =>
        TryCatch<ShoppingCartBaseResponse>(() =>
        {
            shoppingService.SetShippingOption(shippingOptionId);
            shoppingService.SetPaymentOption(paymentOptionId);
            return Ok(GetBaseResponse());
        });


    /// <summary>
    /// Set customer to cart
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="customer"></param>
    /// <returns></returns>
    [HttpPut("set-customer")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetCustomer(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid,
        [FromBody] KCustomer customer)
    {
        var customerInfo = mapper.Map(customer, shoppingService.GetCurrentCustomer());
        if (customerInfo.CustomerUserID == 0 && !MembershipContext.AuthenticatedUser.IsPublic())
        {
            customerInfo.CustomerUserID = MembershipContext.AuthenticatedUser.UserID;
        }

        return TryCatch<ShoppingCartBaseResponse>(() =>
        {
            shoppingService.SetCustomer(customerInfo);
            return Ok(GetBaseResponse());
        });
    }


    /// <summary>
    /// Set billing address
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="billingAddress"></param>
    /// <returns></returns>
    [HttpPut("set-billing-address")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetBillingAddress(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid,
        [FromBody] KAddress billingAddress)
    {
        var addressInfo = mapper.Map(billingAddress, shoppingService.GetBillingAddress());
        return TryCatch<ShoppingCartBaseResponse>(() =>
        {
            shoppingService.SetBillingAddress(addressInfo);
            return Ok(GetBaseResponse());
        });
    }


    /// <summary>
    /// Set shipping address
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="shippingAddress"></param>
    /// <returns></returns>
    [HttpPut("set-shipping-address")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetShippingAddress(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid,
        [FromBody] KAddress shippingAddress)
    {
        var addressInfo = mapper.Map(shippingAddress, shoppingService.GetShippingAddress());
        return TryCatch<ShoppingCartBaseResponse>(() =>
        {
            shoppingService.SetShippingAddress(addressInfo);
            return Ok(GetBaseResponse());
        });
    }


    /// <summary>
    /// Set all delivery details (customer, addreses, shipping and payment)
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="deliveryDetails"></param>
    /// <returns></returns>
    [HttpPut("set-delivery-details")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetDeliveryDetails(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid,
        [FromBody] KShoppingCartDeliveryDetails deliveryDetails)
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        var customerInfo = mapper.Map(deliveryDetails.Customer, cart.Customer);
        if (customerInfo.CustomerUserID == 0 && !MembershipContext.AuthenticatedUser.IsPublic())
        {
            customerInfo.CustomerUserID = MembershipContext.AuthenticatedUser.UserID;
        }
        var billingAddressInfo = mapper.Map(deliveryDetails.BillingAddress, cart.ShoppingCartBillingAddress);
        var shippingAddressInfo = deliveryDetails.ShippingAddress != null ?
            mapper.Map(deliveryDetails.ShippingAddress, cart.ShoppingCartShippingAddress) :
            null;

        return TryCatch<ShoppingCartBaseResponse>(() =>
        {
            using var transactionScope = new CMSTransactionScope();
            shoppingService.SetCustomer(customerInfo);
            shoppingService.SetBillingAddress(billingAddressInfo);
            if (shippingAddressInfo != null)
            {
                shoppingService.SetShippingAddress(shippingAddressInfo);
            }

            if (deliveryDetails.ShippingOptionId > 0)
            {
                shoppingService.SetShippingOption(deliveryDetails.ShippingOptionId);
            }

            if (deliveryDetails.PaymentOptionId > 0)
            {
                shoppingService.SetPaymentOption(deliveryDetails.PaymentOptionId);
            }

            transactionScope.Commit();
            return Ok(GetBaseResponse());
        });
    }


    /// <summary>
    /// Returns all shopping cart data (content and details)
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <returns></returns>
    [HttpGet("summary", Name = nameof(GetCurrentCartSummary))]
    public ActionResult<KShoppingCartSummary> GetCurrentCartSummary(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid)
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        return Ok(new KShoppingCartSummary
        {
            CartContent = mapper.Map<KShoppingCartContent>(cart),
            CartDetails = GetCartDetails(cart)
        });
    }


    /// <summary>
    /// Creates order from shopping cart
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <param name="note"></param>
    /// <returns></returns>
    [HttpPost("create-order")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<KOrder> CreateOrder([FromHeader(Name = "ShoppingCartGUID")][Required] Guid shoppingCartGuid,
        [FromForm] string note) =>
        TryCatch<KOrder>(() =>
        {
            var cart = shoppingService.GetCurrentShoppingCart();
            cart.ShoppingCartNote = note;
            var orderInfo = shoppingService.CreateOrder();
            return Ok(mapper.Map<KOrder>(orderInfo));
        });


    /// <summary>
    /// Validates shopping cart items
    /// </summary>
    /// <param name="shoppingCartGuid"></param>
    /// <returns></returns>
    [HttpPut("validate-cart-items")]
    public ActionResult<ShoppingCartResponse<IEnumerable<KShoppingCartItemValidationError>>> ValidateCartItems(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid)
    {
        var cart = shoppingService.GetCurrentShoppingCart();

        var validationErrors = ShoppingCartInfoProvider.ValidateShoppingCart(cart)
            .OfType<ShoppingCartItemValidationError>();

        return Ok(new ShoppingCartResponse<IEnumerable<KShoppingCartItemValidationError>>
        {
            ShoppingCartGuid = cart.ShoppingCartGUID,
            Value = mapper.Map<IEnumerable<KShoppingCartItemValidationError>>(validationErrors)
        });
    }


    /// <summary>
    /// Set currency to cart when currency is different than previous
    /// </summary>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    [HttpPut("set-currency")]
    public ActionResult<ShoppingCartBaseResponse> SetCurrency(
        [FromHeader(Name = "ShoppingCartGUID")][Required] Guid shoppingCartGuid,
        [CurrencyValidation] string currencyCode)
    {
        var currencyInfo = currencyInfoProvider.Get()
            .WhereEquals(nameof(CurrencyInfo.CurrencyCode), currencyCode)
            .OnSite(ECommerceHelper.GetSiteID(siteService.CurrentSite.SiteID, "CMSStoreUseGlobalCurrencies"))
            .FirstOrDefault();

        if (currencyInfo is null)
        {
            return ValidationProblem($"Currency '{currencyCode}' not found");
        }

        return TryCatch<ShoppingCartBaseResponse>(() =>
        {
            var cart = shoppingService.GetCurrentShoppingCart();
            if (cart.ShoppingCartCurrencyID != currencyInfo.CurrencyID)
            {
                cart.ShoppingCartCurrencyID = currencyInfo.CurrencyID;
                shoppingService.SaveCart();
            }

            return Ok(GetBaseResponse());
        });
    }


    private ShoppingCartBaseResponse GetBaseResponse()
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        return new ShoppingCartBaseResponse { ShoppingCartGuid = cart.ShoppingCartGUID };
    }


    private KShoppingCartDetails GetCartDetails(ShoppingCartInfo cart)
    {
        var cartDetails = mapper.Map<KShoppingCartDetails>(cart);
        var shippingOptionInfos = shippingOptionInfoProvider.GetBySite(siteService.CurrentSite.SiteID, true).ToList();
        cartDetails.AvailableShippingOptions = mapper.Map<IEnumerable<KShippingOption>>(shippingOptionInfos);

        foreach (var shippingOption in cartDetails.AvailableShippingOptions)
        {
            shippingOption.ShippingOptionPrice =
                shoppingService.CalculateShippingOptionPrice(
                    shippingOptionInfos.First(s => s.ShippingOptionID == shippingOption.ShippingOptionId));
        }

        cartDetails.AvailablePaymentOptions = mapper.Map<IEnumerable<KPaymentOption>>(
            paymentOptionInfoProvider.GetBySite(siteService.CurrentSite.SiteID, true)
                .Where(p => PaymentOptionInfoProvider.IsPaymentOptionApplicable(cart, p)));
        return cartDetails;
    }


    private ActionResult<T> TryCatch<T>(Func<ActionResult<T>> func)
    {
        try
        {
            return func();
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity, detail: e.Message);
        }
    }
}
#pragma warning restore IDE0060 // Remove unused parameter
