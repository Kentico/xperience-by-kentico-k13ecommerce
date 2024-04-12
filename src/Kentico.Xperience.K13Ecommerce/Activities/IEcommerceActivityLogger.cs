using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Activities;

public interface IEcommerceActivityLogger
{
    /// <summary>Logs activity product added to shopping cart.</summary>
    /// <param name="sku">SKU info object.</param>
    /// <param name="quantity">SKU quantity.</param>
    /// <param name="variant"></param>
    void LogProductAddedToShoppingCartActivity(KProductSKU sku, int quantity, KProductVariant? variant = null);


    /// <summary>Logs activity product removed from shopping cart.</summary>
    /// <param name="sku">SKU info object.</param>
    /// <param name="quantity">SKU quantity.</param>
    /// <param name="variant"></param>
    void LogProductRemovedFromShoppingCartActivity(KProductSKU sku, int quantity, KProductVariant? variant = null);


    /// <summary>Logs purchase activity.</summary>
    /// <param name="orderId">Order ID.</param>
    /// <param name="totalPrice">Order total price in main currency.</param>
    /// <param name="totalPriceInCorrectCurrency">String representation of order total price in main currency.</param>
    void LogPurchaseActivity(int orderId, decimal totalPrice, string totalPriceInCorrectCurrency);


    /// <summary>Logs product purchased activity.</summary>
    /// <param name="sku">SKUInfo object.</param>
    /// <param name="quantity"></param>
    /// <param name="variant"></param>
    void LogPurchasedProductActivity(KProductSKU sku, int quantity, KProductVariant? variant = null);
}
