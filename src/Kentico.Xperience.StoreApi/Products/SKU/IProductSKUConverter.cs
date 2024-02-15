using CMS.Ecommerce;

namespace Kentico.Xperience.StoreApi.Products.SKU;

/// <summary>
/// Converter for Kentico SKU to sku model
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IProductSKUConverter<out TModel>
{
    TModel Convert(SKUInfo skuInfo, string currencyCode);
}