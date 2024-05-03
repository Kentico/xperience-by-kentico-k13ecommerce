namespace Kentico.Xperience.K13Ecommerce.Products;

/// <summary>
/// Model for request to filter product pages
/// </summary>
public class ProductPageRequest
{
    /// <summary>
    /// Node alias path prefix
    /// </summary>
    public required string Path { get; set; }


    /// <summary>
    /// Document culture
    /// </summary>
    public string? Culture { get; set; }


    /// <summary>
    /// Product currency
    /// </summary>
    public string? Currency { get; set; }


    /// <summary>
    /// Order by SQL expression
    /// </summary>
    public string? OrderBy { get; set; }


    /// <summary>
    /// Limit how many products to return.
    /// </summary>
    public int? Limit { get; set; }


    /// <summary>
    /// If true variants are loaded too for products with variants (default false).
    /// </summary>
    public bool WithVariants { get; set; }


    /// <summary>
    /// If true, DocumentSKUDescription is filled too (default false).
    /// </summary>
    public bool WithLongDescription { get; set; }


    /// <summary>
    /// If true, only not-linked product pages are returned (default false).
    /// </summary>
    public bool NoLinks { get; set; }
}

