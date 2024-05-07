using CMS.Ecommerce;

namespace Kentico.Xperience.StoreApi.Products.Pages;

/// <summary>
/// Converter from Kentico product page to product page model.
/// </summary>
/// <typeparam name="TProduct">Type for product node</typeparam>
public interface IProductPageConverter<out TProduct>
    where TProduct : KProductNode
{
    TProduct Convert(SKUTreeNode skuTreeNode, IEnumerable<string> customFields, string currencyCode, bool withVariants, bool withLongDescription);
}
