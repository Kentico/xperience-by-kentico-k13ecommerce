﻿using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization.Interfaces;

namespace K13Store;

/// <summary>
/// Product image external identifer
/// </summary>
public partial class ProductImage : IItemIdentifier<Guid>
{
    public Guid ExternalId => ProductImageOriginalGUID;
}