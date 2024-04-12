namespace Kentico.Xperience.K13Ecommerce.Products;


/// <summary>
/// Model for product list item
/// </summary>
public class ProductListModel
{
    /// <summary>
    /// Product name
    /// </summary>
    public string? Name { get; set; }


    /// <summary>
    /// Short description
    /// </summary>
    public string? ShortDescription { get; set; }


    /// <summary>
    /// Product url
    /// </summary>
    public string? ProductUrl { get; set; }


    /// <summary>
    /// Product image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Product image alt text
    /// </summary>
    public string? ImageAlt { get; set; }


    /// <summary>
    /// Product price
    /// </summary>
    public decimal? Price { get; set; }


    /// <summary>
    /// Product retail price
    /// </summary>
    public decimal? ListPrice { get; set; }
}
