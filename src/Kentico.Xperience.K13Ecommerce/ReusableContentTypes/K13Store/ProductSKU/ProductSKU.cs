using System.Text.Json;

using Kentico.Xperience.Ecommerce.Common.ContentItemSynchronization;

namespace K13Store;

/// <summary>
/// Product sku external identifier
/// </summary>
public partial class ProductSKU : IItemIdentifier<int>
{
    public int ExternalId => SKUID;


    private Dictionary<string, object>? customFieldsDict;

    /// <summary>
    /// Custom fields as dictionary
    /// </summary>
    public Dictionary<string, object> CustomFieldsDict => customFieldsDict ??=
        (JsonSerializer.Deserialize<Dictionary<string, object>>(CustomFields) ?? []);
}
