using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Partial class for product node extension to set external identifer
/// </summary>
public partial class KProductNode : IItemIdentifier<int>
{
    public int ExternalId => Sku.Skuid;
}

/// <summary>
/// Partial class for product SKU to set external identifer
/// </summary>
public partial class KProductSKU : IItemIdentifier<int>
{
    public int ExternalId => Skuid;
}

/// <summary>
/// Partial class for product variant to set external identifer
/// </summary>
public partial class KProductVariant : IItemIdentifier<int>
{
    public int ExternalId => Skuid;
}
