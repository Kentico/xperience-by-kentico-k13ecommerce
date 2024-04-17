using System.Globalization;

using CMS.Activities;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Activities;

internal class EcommerceActivityLogger(ICustomActivityLogger customActivityLogger) : IEcommerceActivityLogger
{
    public void LogProductAddedToShoppingCartActivity(KProductSKU sku, int quantity, KProductVariant? variant = null) =>
        customActivityLogger.Log(EcommerceActivityTypes.ProductAddedToCartActivity, new CustomActivityData
        {
            ActivityTitle = $"Product added to shopping cart '{sku.SkuName}'",
            ActivityValue = quantity.ToString(),
            ActivityItemID = sku.Skuid,
            ActivityItemDetailID = variant?.Skuid ?? 0
        });


    public void LogProductRemovedFromShoppingCartActivity(KProductSKU sku, int quantity, KProductVariant? variant = null) =>
        customActivityLogger.Log(EcommerceActivityTypes.ProductRemovedFromCartActivity, new CustomActivityData
        {
            ActivityTitle = $"Product removed from shopping cart '{sku.SkuName}'",
            ActivityValue = quantity.ToString(),
            ActivityItemID = sku.Skuid,
            ActivityItemDetailID = variant?.Skuid ?? 0
        });


    public void LogPurchaseActivity(int orderId, decimal totalPrice, string totalPriceInCorrectCurrency) =>
        customActivityLogger.Log(EcommerceActivityTypes.PurchaseActivity, new CustomActivityData
        {
            ActivityTitle = $"Purchase for '{totalPriceInCorrectCurrency}'",
            ActivityValue = totalPrice.ToString(CultureInfo.InvariantCulture),
            ActivityItemID = orderId,
        });


    public void LogPurchasedProductActivity(KProductSKU sku, int quantity, KProductVariant? variant = null) =>
        customActivityLogger.Log(EcommerceActivityTypes.PurchasedProductActivity, new CustomActivityData
        {
            ActivityTitle = $"Purchased product '{sku.SkuName}'",
            ActivityValue = quantity.ToString(),
            ActivityItemID = sku.Skuid,
            ActivityItemDetailID = variant?.Skuid ?? 0
        });
}
