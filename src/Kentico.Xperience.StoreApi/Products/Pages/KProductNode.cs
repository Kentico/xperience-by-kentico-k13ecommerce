using Kentico.Xperience.StoreApi.Products.SKU;

namespace Kentico.Xperience.StoreApi.Products.Pages;

/// <summary>
/// Represents product page
/// </summary>
public class KProductNode
{
    public string DocumentSKUName { get; set; }

    public string DocumentSKUShortDescription { get; set; }

    public string DocumentSKUDescription { get; set; }

    public string ClassName { get; set; }

    public string NodeAliasPath { get; set; }

    public Guid NodeGuid { get; set; }

    public int NodeOrder { get; set; }

    public string DocumentCulture { get; set; }

    public bool IsLink { get; set; }

    public string AbsoluteUrl { get; set; }

    /// <summary>
    /// Contains custom field values on given page type
    /// </summary>
    public Dictionary<string, object> CustomFields { get; set; }

    /// <summary>
    /// Product SKU
    /// </summary>
    public KProductSKU SKU { get; set; }
}
