using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace K13Store;

/// <summary>
/// Product image external identifer
/// </summary>
public partial class ProductImage : IItemIdentifier<string>
{
    /// <summary>
    /// External Id for product image is original image path
    /// </summary>
    public string ExternalId => ProductImageOriginalPath;
}
