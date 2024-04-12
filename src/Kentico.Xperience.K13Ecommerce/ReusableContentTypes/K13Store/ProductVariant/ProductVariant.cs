using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

namespace K13Store;

/// <summary>
/// Product variant external identifier
/// </summary>
public partial class ProductVariant : IItemIdentifier<int>
{
    public int ExternalId => SKUID;
}
