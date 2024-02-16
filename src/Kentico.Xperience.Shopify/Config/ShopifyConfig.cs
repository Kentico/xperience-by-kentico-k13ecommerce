namespace Kentico.Xperience.Shopify.Config
{
    public class ShopifyConfig
    {
        public readonly static string SECTION_NAME = "CMSShopifyConfig";

        public required string ShopifyUrl { get; set; }
        public required string ApiToken { get; set; }
    }
}

