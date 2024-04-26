using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace K13Store;

/// <summary>
/// Product image external identifer
/// </summary>
public partial class ProductImage : IItemIdentifier<string>
{
    public string ExternalId => ProductImageOriginalPath;
}
