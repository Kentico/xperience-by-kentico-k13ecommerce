using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using AutoMapper;

using CMS.Base;
using CMS.DataEngine;
using CMS.Ecommerce;

using Kentico.Xperience.StoreApi.Authentication;
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
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingService shoppingService;
    private readonly IMapper mapper;
    private readonly ISiteService siteService;
    private readonly IShippingOptionInfoProvider shippingOptionInfoProvider;
    private readonly IPaymentOptionInfoProvider paymentOptionInfoProvider;

    public ShoppingCartController(IShoppingService shoppingService, IMapper mapper, ISiteService siteService,
        IShippingOptionInfoProvider shippingOptionInfoProvider, IPaymentOptionInfoProvider paymentOptionInfoProvider)
    {
        this.shoppingService = shoppingService;
        this.mapper = mapper;
        this.siteService = siteService;
        this.shippingOptionInfoProvider = shippingOptionInfoProvider;
        this.paymentOptionInfoProvider = paymentOptionInfoProvider;
    }

    [HttpGet("content", Name = nameof(GetCurrentCartContent))]
    public ActionResult<KShoppingCartContent> GetCurrentCartContent(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid)
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        return mapper.Map<KShoppingCartContent>(cart);
    }

    [HttpGet("details", Name = nameof(GetCurrentCartDetails))]
    public ActionResult<KShoppingCartDetails> GetCurrentCartDetails(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid)
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        var cartDetails = GetCartDetails(cart);
        return Ok(cartDetails);
    }

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

    [HttpPut("update-item", Name = nameof(UpdateItemQuantity))]
    public ActionResult<ShoppingCartBaseResponse> UpdateItemQuantity(
        [Required] [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid,
        [FromQuery][Required] int itemId, [FromQuery][Required][Range(0, short.MaxValue)] int quantity)
    {
        shoppingService.UpdateItemQuantity(itemId, quantity);
        return Ok(GetBaseResponse());
    }

    [HttpDelete("remove-item", Name = nameof(RemoveItemFromCart))]
    public ActionResult<ShoppingCartBaseResponse> RemoveItemFromCart(
        [Required] [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid,
        [FromQuery][Required] int itemId)
    {
        shoppingService.RemoveItemFromCart(itemId);
        return Ok(GetBaseResponse());
    }

    [HttpPost("add-coupon-code")]
    public ActionResult<ShoppingCartResponse<bool>> AddCouponCode(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid, [FromQuery][Required] string couponCode)
    {
        bool success = shoppingService.AddCouponCode(couponCode);
        var cart = shoppingService.GetCurrentShoppingCart();
        return new ShoppingCartResponse<bool> { ShoppingCartGuid = cart.ShoppingCartGUID, Value = success };
    }

    [HttpDelete("remove-coupon-code")]
    public ActionResult<ShoppingCartBaseResponse> RemoveCouponCode(
        [FromHeader(Name = "ShoppingCartGUID")]
        Guid shoppingCartGuid, [FromQuery][Required] string couponCode)
    {
        shoppingService.RemoveCouponCode(couponCode);
        return Ok(GetBaseResponse());
    }

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

    [HttpPut("set-customer")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetCustomer(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid,
        [FromBody] KCustomer customer)
    {
        var customerInfo = mapper.Map(customer, shoppingService.GetCurrentCustomer());
        return TryCatch<ShoppingCartBaseResponse>(() =>
        {
            shoppingService.SetCustomer(customerInfo);
            return Ok(GetBaseResponse());
        });
    }

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

    [HttpPut("set-delivery-details")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ShoppingCartBaseResponse> SetDeliveryDetails(
        [FromHeader(Name = "ShoppingCartGUID")] [Required]
        Guid shoppingCartGuid,
        [FromBody] KShoppingCartDeliveryDetails deliveryDetails)
    {
        var cart = shoppingService.GetCurrentShoppingCart();
        var customerInfo = mapper.Map(deliveryDetails.Customer, cart.Customer);
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
            return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
        }
    }
}
#pragma warning restore IDE0060 // Remove unused parameter
