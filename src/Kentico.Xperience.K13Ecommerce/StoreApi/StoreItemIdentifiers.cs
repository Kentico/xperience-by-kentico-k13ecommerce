﻿using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Partial class for product node extension to set external identifer.
/// </summary>
public partial class KProductNode : IItemIdentifier<int>
{
    /// <inheritdoc/>
    public int ExternalId => Sku.Skuid;

    /// <summary>
    /// Create product node from product SKU.
    /// </summary>
    /// <param name="sku"></param>
    public KProductNode(KProductSKU sku)
    {
        Sku = sku;
        DocumentSKUName = sku.SkuName;
        DocumentSKUDescription = sku.SkuLongDescription;
        DocumentSKUShortDescription = sku.SkuShortDescription;
        ClassName = string.Empty;
        NodeAliasPath = string.Empty;
    }
}

/// <summary>
/// Partial class for product SKU to set external identifer.
/// </summary>
public partial class KProductSKU : IItemIdentifier<int>
{
    /// <inheritdoc/>
    public int ExternalId => Skuid;
}

/// <summary>
/// Partial class for product variant to set external identifer.
/// </summary>
public partial class KProductVariant : IItemIdentifier<int>
{
    /// <inheritdoc/>
    public int ExternalId => Skuid;
}
