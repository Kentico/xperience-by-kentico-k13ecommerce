namespace Kentico.Xperience.K13Ecommerce.Products;

/// <summary>
/// Model for request to filter product pages
/// </summary>
public class ProductPageRequest : ProductRequest
{
    /// <summary>
    /// Node alias path prefix
    /// </summary>
    public required string Path { get; set; }


    /// <summary>
    /// If true, DocumentSKUDescription is filled too (default false).
    /// </summary>
    public bool WithLongDescription { get; set; }


    /// <summary>
    /// If true, only not-linked product pages are returned (default false).
    /// </summary>
    public bool NoLinks { get; set; }
}

