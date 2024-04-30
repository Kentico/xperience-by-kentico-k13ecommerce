using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.KStore.Tests;

[TestFixture]
[Category("IntegrationTests")]
public class StoreApiShoppingCartTests : StoreApiTestBase
{
    [Test]
    [TestCase(16, 1)]//product
    [TestCase(19, 1)]//variant
    public async Task AddItemToCart_Valid_Product_And_Remove_It(int skuId, int quantity)
    {
        var response = await StoreApiClient.AddItemToCartAsync(skuId: skuId, quantity: quantity);

        AddItemToCartAsserts(response);

        await RemoveItem(response);
    }


    [Test]
    [TestCase(16, 1)]
    public async Task UpdateItemQuantity_ValidProduct_CheckContent(int skuId, int quantity)
    {
        var response = await StoreApiClient.AddItemToCartAsync(skuId: skuId, quantity: quantity);
        AddItemToCartAsserts(response);

        var updateResponse = await StoreApiClient.UpdateItemQuantityAsync(response!.ShoppingCartGuid,
            response!.Value!.CartItemId, 2);

        AssertBaseResponse(updateResponse);

        var content = await StoreApiClient.GetCurrentCartContentAsync(response!.ShoppingCartGuid);

        Assert.That(content.ShoppingCartGuid, Is.EqualTo(response.ShoppingCartGuid));
        Assert.That(content.CartProducts!.Count, Is.EqualTo(1));
        Assert.That(content.CartProducts.First().CartItemId, Is.EqualTo(response.Value!.CartItemId));
        Assert.That(content.CartProducts.First().CartItemUnits, Is.EqualTo(2));

        await RemoveItem(response);
    }


    [Test]
    [TestCase("COUPONNOTEXIST")]
    public async Task AddCouponCode_InvalidCode_FalseResult(string couponCode)
    {
        var response = await StoreApiClient.AddCouponCodeAsync(couponCode);
        Assert.That(response.Value, Is.False);
    }


    [Test]
    [TestCase("BDAY_CWPZ9")]
    public async Task AddCouponCode_ValidCoupon_CheckContent_RemoveCoupon(string couponCode)
    {
        var response = await StoreApiClient.AddCouponCodeAsync(couponCode);
        Assert.That(response.Value, Is.True);

        var content = await StoreApiClient.GetCurrentCartContentAsync();

        await StoreApiClient.RemoveCouponCodeAsync(couponCode);

        Assert.That(content.CouponCodes, Has.Count.EqualTo(1));
        Assert.That(content.OrderDiscountSummary, Has.Count.EqualTo(1));

        var contentNoCoupon = await StoreApiClient.GetCurrentCartContentAsync();

        Assert.That(contentNoCoupon.CouponCodes, Is.Empty);
        Assert.That(contentNoCoupon.OrderDiscountSummary, Is.Empty);
    }


    [Test]
    [TestCase(2)]
    public async Task SetShippingOption_Valid_CheckCartDetails(int shippingOptionId)
    {
        var res = await StoreApiClient.SetShippingOptionAsync(Guid.Empty, shippingOptionId);
        AssertBaseResponse(res);

        var cartDetails = await StoreApiClient.GetCurrentCartDetailsAsync(res.ShoppingCartGuid);

        Assert.That(cartDetails.ShippingOptionId, Is.EqualTo(shippingOptionId));
    }


    [Test]
    [TestCase(2)]
    public async Task SetPaymentOption_Valid_CheckCartDetails(int paymentOptionId)
    {
        var res = await StoreApiClient.SetPaymentOptionAsync(Guid.Empty, paymentOptionId);
        AssertBaseResponse(res);

        var cartDetails = await StoreApiClient.GetCurrentCartDetailsAsync(res.ShoppingCartGuid);

        Assert.That(cartDetails.PaymentOptionId, Is.EqualTo(paymentOptionId));
    }


    [Test]
    [TestCase(2, 2)]
    public async Task SetShippingAndPayment_Valid_CheckCartDetails(int shippingOptionId, int paymentOptionId)
    {
        var res = await StoreApiClient.SetShippingAndPaymentAsync(Guid.Empty, shippingOptionId, paymentOptionId);
        AssertBaseResponse(res);

        var cartDetails = await StoreApiClient.GetCurrentCartDetailsAsync(res.ShoppingCartGuid);

        Assert.That(cartDetails.ShoppingCartGuid, Is.EqualTo(res.ShoppingCartGuid));
        Assert.That(cartDetails.ShippingOptionId, Is.EqualTo(shippingOptionId));
        Assert.That(cartDetails.PaymentOptionId, Is.EqualTo(paymentOptionId));
    }


    [Test]
    public async Task SetCustomer_CheckCartDetails()
    {
        var customer = GetCustomer();

        var res = await StoreApiClient.SetCustomerAsync(Guid.Empty, customer);
        AssertBaseResponse(res);

        var cartDetails = await StoreApiClient.GetCurrentCartDetailsAsync(res.ShoppingCartGuid);

        Assert.That(cartDetails.ShoppingCartGuid, Is.EqualTo(res.ShoppingCartGuid));
        Assert.That(cartDetails.Customer, Is.Not.Null);
        Assert.That(cartDetails.Customer.CustomerFirstName, Is.EqualTo(customer.CustomerFirstName));
        Assert.That(cartDetails.Customer.CustomerLastName, Is.EqualTo(customer.CustomerLastName));
        Assert.That(cartDetails.Customer.CustomerEmail, Is.EqualTo(customer.CustomerEmail));
    }


    [Test]
    public async Task SetBillingAddress_CheckCartDetails()
    {
        var customer = GetCustomer();
        var res = await StoreApiClient.SetCustomerAsync(Guid.Empty, customer);

        var address = new KAddress
        {
            AddressLine1 = "Automation Street",
            AddressCity = "Automation City",
            AddressZip = "00000",
            AddressCountryId = 326
        };

        res = await StoreApiClient.SetBillingAddressAsync(res.ShoppingCartGuid, address);
        AssertBaseResponse(res);

        var cartDetails = await StoreApiClient.GetCurrentCartDetailsAsync(res.ShoppingCartGuid);
        Assert.That(cartDetails.ShoppingCartGuid, Is.EqualTo(res.ShoppingCartGuid));
        Assert.That(cartDetails.BillingAddress, Is.Not.Null);
        Assert.That(cartDetails.BillingAddress.AddressLine1, Is.EqualTo(address.AddressLine1));
        Assert.That(cartDetails.BillingAddress.AddressCity, Is.EqualTo(address.AddressCity));
        Assert.That(cartDetails.BillingAddress.AddressZip, Is.EqualTo(address.AddressZip));
        Assert.That(cartDetails.BillingAddress.AddressCountryId, Is.EqualTo(address.AddressCountryId));
    }


    [Test]
    public async Task SetShippingAddress_CheckCartDetails()
    {
        var customer = GetCustomer();
        await StoreApiClient.SetCustomerAsync(Guid.Empty, customer);

        var address = new KAddress
        {
            AddressLine1 = "Automation Street",
            AddressCity = "Automation City",
            AddressZip = "00000",
            AddressCountryId = 326
        };

        var res = await StoreApiClient.SetShippingAddressAsync(Guid.Empty, address);
        AssertBaseResponse(res);

        var cartDetails = await StoreApiClient.GetCurrentCartDetailsAsync(res.ShoppingCartGuid);
        Assert.That(cartDetails.ShoppingCartGuid, Is.EqualTo(res.ShoppingCartGuid));
        Assert.That(cartDetails.ShippingAddress, Is.Not.Null);
        Assert.That(cartDetails.ShippingAddress?.AddressLine1, Is.EqualTo(address.AddressLine1));
        Assert.That(cartDetails.ShippingAddress?.AddressCity, Is.EqualTo(address.AddressCity));
        Assert.That(cartDetails.ShippingAddress?.AddressZip, Is.EqualTo(address.AddressZip));
        Assert.That(cartDetails.ShippingAddress?.AddressCountryId, Is.EqualTo(address.AddressCountryId));
    }


    [Test]
    [TestCase(2, 2)]
    public async Task SetDeliveryDetails_CheckCartDetails(int shippingId, int paymementId)
    {
        var (customer, billingAddress, shippingAddress) = (GetCustomer(), GetBillingAddress(), GetShippingAddress());

        var res = await StoreApiClient.SetDeliveryDetailsAsync(Guid.Empty,
            new KShoppingCartDeliveryDetails
            {
                Customer = customer,
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress,
                ShippingOptionId = shippingId,
                PaymentOptionId = paymementId,
            });


        AssertBaseResponse(res);
        var cartDetails = await StoreApiClient.GetCurrentCartDetailsAsync(res.ShoppingCartGuid);

        Assert.That(cartDetails.ShoppingCartGuid, Is.EqualTo(res.ShoppingCartGuid));
        Assert.That(cartDetails.Customer, Is.Not.Null);
        Assert.That(cartDetails.Customer.CustomerFirstName, Is.EqualTo(customer.CustomerFirstName));
        Assert.That(cartDetails.Customer.CustomerLastName, Is.EqualTo(customer.CustomerLastName));
        Assert.That(cartDetails.Customer.CustomerEmail, Is.EqualTo(customer.CustomerEmail));

        Assert.That(cartDetails.BillingAddress, Is.Not.Null);
        Assert.That(cartDetails.BillingAddress.AddressLine1, Is.EqualTo(billingAddress.AddressLine1));
        Assert.That(cartDetails.BillingAddress.AddressCity, Is.EqualTo(billingAddress.AddressCity));
        Assert.That(cartDetails.BillingAddress?.AddressZip, Is.EqualTo(billingAddress.AddressZip));
        Assert.That(cartDetails.BillingAddress?.AddressCountryId, Is.EqualTo(billingAddress.AddressCountryId));

        Assert.That(cartDetails.ShippingAddress, Is.Not.Null);
        Assert.That(cartDetails.ShippingAddress?.AddressLine1, Is.EqualTo(shippingAddress.AddressLine1));
        Assert.That(cartDetails.ShippingAddress?.AddressCity, Is.EqualTo(shippingAddress.AddressCity));
        Assert.That(cartDetails.ShippingAddress?.AddressZip, Is.EqualTo(shippingAddress.AddressZip));
        Assert.That(cartDetails.ShippingAddress?.AddressCountryId, Is.EqualTo(shippingAddress.AddressCountryId));

        Assert.That(cartDetails.ShippingOptionId, Is.EqualTo(shippingId));
        Assert.That(cartDetails.PaymentOptionId, Is.EqualTo(shippingId));
    }


    [Test]
    public async Task CreateOrder_TwoProducts()
    {
        var res1 = await StoreApiClient.AddItemToCartAsync(skuId: 19, quantity: 1);
        var res2 = await StoreApiClient.AddItemToCartAsync(res1.ShoppingCartGuid, skuId: 16, quantity: 1);

        Assert.That(res1.ShoppingCartGuid, Is.Not.Empty);
        Assert.That(res2.ShoppingCartGuid, Is.EqualTo(res1.ShoppingCartGuid));

        var (customer, billingAddress, shippingAddress) = (GetCustomer(), GetBillingAddress(), GetShippingAddress());

        (int shippingOptionId, int paymentOptionId) = (2, 2);

        var res3 = await StoreApiClient.SetDeliveryDetailsAsync(res2.ShoppingCartGuid,
            new KShoppingCartDeliveryDetails
            {
                Customer = customer,
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress,
                ShippingOptionId = shippingOptionId,
                PaymentOptionId = paymentOptionId,
            });

        Assert.That(res3.ShoppingCartGuid, Is.EqualTo(res2.ShoppingCartGuid));

        var order = await StoreApiClient.CreateOrderAsync(res3.ShoppingCartGuid, "Order test");

        Assert.That(order.OrderItems, Has.Count.EqualTo(2));
        Assert.That(order.OrderItems!.Select(oi => oi.OrderItemSkuId), Does.Contain(16).And.Contain(19));

        Assert.That(order.OrderCustomer, Is.Not.Null);
        Assert.That(order.OrderCustomer.CustomerEmail, Is.EqualTo(customer.CustomerEmail));
        Assert.That(order.OrderCustomer.CustomerFirstName, Is.EqualTo(customer.CustomerFirstName));
        Assert.That(order.OrderCustomer.CustomerLastName, Is.EqualTo(customer.CustomerLastName));

        Assert.That(order.OrderBillingAddress, Is.Not.Null);
        Assert.That(order.OrderBillingAddress.AddressLine1, Is.EqualTo(billingAddress.AddressLine1));
        Assert.That(order.OrderBillingAddress.AddressCity, Is.EqualTo(billingAddress.AddressCity));
        Assert.That(order.OrderBillingAddress.AddressZip, Is.EqualTo(billingAddress.AddressZip));
        Assert.That(order.OrderBillingAddress.AddressCountryId, Is.EqualTo(billingAddress.AddressCountryId));

        Assert.That(order.OrderShippingAddress, Is.Not.Null);
        Assert.That(order.OrderShippingAddress?.AddressLine1, Is.EqualTo(shippingAddress.AddressLine1));
        Assert.That(order.OrderShippingAddress?.AddressCity, Is.EqualTo(shippingAddress.AddressCity));
        Assert.That(order.OrderShippingAddress?.AddressZip, Is.EqualTo(shippingAddress.AddressZip));
        Assert.That(order.OrderShippingAddress?.AddressCountryId, Is.EqualTo(shippingAddress.AddressCountryId));

        Assert.That(order.OrderShippingOption.ShippingOptionId, Is.EqualTo(shippingOptionId));
        Assert.That(order.OrderPaymentOption.PaymentOptionId, Is.EqualTo(paymentOptionId));
    }


    private KCustomer GetCustomer() => new()
    {
        CustomerFirstName = "Automation",
        CustomerLastName = "Test",
        CustomerEmail = "automation.test@test.com"
    };


    private KAddress GetBillingAddress() => new()
    {
        AddressLine1 = "Automation Street 1",
        AddressCity = "Automation City A",
        AddressZip = "00001",
        AddressCountryId = 326
    };


    private KAddress GetShippingAddress() => new()
    {
        AddressLine1 = "Automation Street 2",
        AddressCity = "Automation City B",
        AddressZip = "00002",
        AddressCountryId = 326
    };


    private async Task RemoveItem(KShoppingCartItemShoppingCartResponse response)
    {
        var removeResponse = await StoreApiClient.RemoveItemFromCartAsync(response!.ShoppingCartGuid,
            response!.Value!.CartItemId);
        AssertBaseResponse(removeResponse);
    }


    private void AddItemToCartAsserts(KShoppingCartItemShoppingCartResponse response)
    {
        Assert.That(response.ShoppingCartGuid, Is.Not.Empty.And.Not.Null);
        Assert.That(response.Value, Is.Not.Null);
        Assert.That(response.Value!.TotalPrice, Is.GreaterThan(0m));
        Assert.That(response.Value.ProductSKU, Is.Not.Null);
        Assert.That(response.Value!.CartItemId, Is.GreaterThan(0));
    }


    private void AssertBaseResponse(ShoppingCartBaseResponse response)
    {
        Assert.That(response.ShoppingCartGuid, Is.Not.Empty.And.Not.Null);
    }


#pragma warning disable S1135 // Track uses of "TODO" tags
    [TearDown]
    public new void TearDown()
    {
        //@TODO DB objects cleanup - need to create endpoint for cart and other objects deletion (not high priority)
    }
#pragma warning restore S1135 // Track uses of "TODO" tags
}
