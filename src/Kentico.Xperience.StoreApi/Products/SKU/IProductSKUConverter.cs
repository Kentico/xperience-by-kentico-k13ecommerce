using CMS.Ecommerce;

namespace Kentico.Xperience.StoreApi.Products.SKU;

/// <summary>
/// Converter for Kentico SKU to sku model.
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IProductSKUConverter<out TModel>
{
    /// <summary>
    /// Converts Kentico SKU to model.
    /// </summary>
    /// <param name="skuInfo">SKU info.</param>
    /// <param name="currencyCode">Currency code in which evaluates prices.</param>
    /// <param name="withVariants">If true, variants are included.</param>
    /// <param name="withLongDescription">If true, <see cref="SKUInfo.SKUDescription"/> is included.</param>
    /// <returns></returns>
    TModel Convert(SKUInfo skuInfo, string currencyCode, bool withVariants, bool withLongDescription);
}
