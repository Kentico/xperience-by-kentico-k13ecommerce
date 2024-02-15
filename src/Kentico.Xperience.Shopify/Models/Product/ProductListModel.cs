namespace Kentico.Xperience.Shopify.Models
{
    public class ProductListModel
    {
        public string? Image {  get; set; }
        public string? ImageAlt { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ShopifyUrl { get; set; }
        public decimal? Price { get; set; }
        public decimal? ListPrice { get; set; }
    }
}