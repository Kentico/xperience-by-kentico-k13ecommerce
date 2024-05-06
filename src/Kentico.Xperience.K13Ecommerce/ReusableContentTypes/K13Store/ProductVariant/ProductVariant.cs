using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace K13Store;

/// <summary>
/// Product variant external identifier.
/// </summary>
public partial class ProductVariant : IItemIdentifier<int>
{
    /// <inheritdoc/>
    public int ExternalId => SKUID;
}
