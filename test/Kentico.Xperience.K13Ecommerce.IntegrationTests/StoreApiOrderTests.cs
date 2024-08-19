using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.KStore.Tests;

/// <summary>
/// Store API order integration tests.
/// </summary>
[TestFixture]
[Category("IntegrationTests")]
public class StoreApiOrderTests : StoreApiTestBase
{
    /// <summary>
    /// Test for existing order.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <param name="customerEmail">Customer email.</param>
    [Test]
    [TestCase(27, "automation.test@test.com")]
    public async Task OrderDetail_ExistingOrder(int orderId, string customerEmail)
    {
        var order = await StoreApiClient.OrderDetailAsync(orderId);

        Assert.That(order.OrderId, Is.EqualTo(orderId));
        Assert.That(order.OrderItems, Has.Count.EqualTo(2));

        Assert.That(order.OrderCustomer.CustomerEmail, Is.EqualTo(customerEmail));
        Assert.That(order.OrderBillingAddress.AddressLine1, Is.EqualTo("Automation Street 1"));
        Assert.That(order.OrderBillingAddress.AddressCity, Is.EqualTo("Automation City A"));
        Assert.That(order.OrderBillingAddress.AddressZip, Is.EqualTo("00001"));
        Assert.That(order.OrderBillingAddress.AddressCountryId, Is.EqualTo(326));

        Assert.That(order.OrderShippingAddress.AddressLine1, Is.EqualTo("Automation Street 2"));
        Assert.That(order.OrderShippingAddress.AddressCity, Is.EqualTo("Automation City B"));
        Assert.That(order.OrderShippingAddress.AddressZip, Is.EqualTo("00002"));
        Assert.That(order.OrderShippingAddress.AddressCountryId, Is.EqualTo(326));

        Assert.That(order.OrderStatus.StatusName, Is.EqualTo("New"));
        Assert.That(order.OrderShippingOption.ShippingOptionName, Is.EqualTo("StandardDelivery"));
        Assert.That(order.OrderPaymentOption.PaymentOptionName, Is.EqualTo("DancingGoatCore.MoneyTransfer"));
    }

    /// <summary>
    /// Test order detail when order does not exist.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    [Test]
    [TestCase(99999)]
    public Task OrderDetail_NonExistingOrder(int orderId)
    {
        // test that exception has status code 404
        var exception = Assert.ThrowsAsync<ApiException>(async () => await StoreApiClient.OrderDetailAsync(orderId));
        Assert.That(exception!.StatusCode, Is.EqualTo(404));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Test for order update.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    [Test]
    [TestCase(27)]
    public async Task OrderUpdate_ChangeStatus_And_IsPaid(int orderId)
    {
        var order = await StoreApiClient.OrderDetailAsync(orderId);

        order.OrderStatus = new KOrderStatus { StatusId = 2 };
        order.OrderIsPaid = true;

        await StoreApiClient.UpdateOrderAsync(order);

        var updatedOrder = await StoreApiClient.OrderDetailAsync(orderId);

        Assert.That(updatedOrder.OrderStatus.StatusName, Is.EqualTo("PaymentReceived"));
        Assert.That(updatedOrder.OrderIsPaid, Is.True);

        order.OrderIsPaid = false;
        order.OrderStatus = new KOrderStatus { StatusId = 3 };

        await StoreApiClient.UpdateOrderAsync(order);

        var rollbackedOrder = await StoreApiClient.OrderDetailAsync(orderId);

        Assert.That(rollbackedOrder.OrderStatus.StatusName, Is.EqualTo("New"));
        Assert.That(rollbackedOrder.OrderIsPaid, Is.False);
    }
}
