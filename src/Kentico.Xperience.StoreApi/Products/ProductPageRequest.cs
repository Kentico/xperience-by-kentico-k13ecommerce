using System.ComponentModel.DataAnnotations;

namespace Kentico.Xperience.StoreApi.Products;

/// <summary>
/// Model for product pages request used in API.
/// </summary>
public class ProductPageRequest : ProductRequest
{
    /// <summary>
    /// Node alias path prefix.
    /// </summary>
    [Required]
    public string Path { get; set; }

    /// <summary>
    /// If true, DocumentSKUDescription is filled too (default false).
    /// </summary>
    public bool WithLongDescription { get; set; }

    /// <summary>
    /// If true, only not-linked product pages are returned (default false).
    /// </summary>
    public bool NoLinks { get; set; }


    public void Deconstruct(out string path, out string culture, out string currency, out string orderBy,
        out int limit, out bool withVariants, out bool withLongDescription, out bool noLinks)
    {
        path = Path;
        culture = Culture;
        currency = Currency;
        orderBy = OrderBy;
        limit = Limit;
        withVariants = WithVariants;
        withLongDescription = WithLongDescription;
        noLinks = NoLinks;
    }
}
