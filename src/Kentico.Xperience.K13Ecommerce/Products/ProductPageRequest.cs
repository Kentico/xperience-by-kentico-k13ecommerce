namespace Kentico.Xperience.K13Ecommerce.Products;

public class ProductPageRequest
{
    public required string Path { get; set; }
    public string? Culture { get; set; }
    public string? Currency { get; set; }
    public string? OrderBy { get; set; }
    public int? Limit { get; set; }
}

