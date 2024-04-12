using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Kentico.Xperience.StoreApi.Currencies;

namespace Kentico.Xperience.StoreApi.Products;

/// <summary>
/// Model for product pages request used in API
/// </summary>
public class ProductPageRequest
{
    /// <summary>
    /// Node alias path prefix
    /// </summary>
    [Required]
    public string Path { get; set; }

    /// <summary>
    /// Document culture
    /// </summary>
    [RegularExpression("[a-zA-Z]{2}-[a-zA-Z]{2}")]
    public string Culture { get; set; }

    /// <summary>
    /// Product currency
    /// </summary>
    [CurrencyValidation]
    public string Currency { get; set; }

    /// <summary>
    /// Order by SQL expression
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Limit how many products to return
    /// </summary>
    [DefaultValue(12)]
    [Range(1, 1000)]
    public int Limit { get; set; }

    /// <summary>
    /// If true variants are loaded too for products with variants (default false)
    /// </summary>
    public bool WithVariants { get; set; }

    /// <summary>
    /// If true, DocumentSKUDescription is filled too (default false)
    /// </summary>
    public bool WithLongDescription { get; set; }

    /// <summary>
    /// If true, only not-linked product pages are returned (default false)
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
