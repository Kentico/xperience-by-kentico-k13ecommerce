using K13Store;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace DancingGoat.Models;

public class StoreProductListItemViewModel
{
    public string MainImageUrl { get; init; }
    public string ProductName { get; init; }
    public string ProductDetailUrl { get; init; }
    public KProductCatalogPrices Prices { get; init; }
    public bool HasMultipleVariants { get; init; }
    public string PublicStatusName { get; init; }
    public bool IsAvailable { get; set; }

    public static StoreProductListItemViewModel GetViewModel(ProductSKU product, string url, KProductCatalogPrices prices)
    {
        var mainImage = product.ProductImages.FirstOrDefault();
        return new StoreProductListItemViewModel()
        {
            MainImageUrl = mainImage?.ProductImageAsset.Url ?? string.Empty,
            ProductName = product.SKUName,
            ProductDetailUrl = url,
            Prices = prices,
            HasMultipleVariants = product.ProductVariants.Any(),
            PublicStatusName = product.PublicStatusDisplayName,
            IsAvailable = !product.SKUSellOnlyAvailable || product.SKUAvailableItems > 0 //@TODO inventory items could be better to show directly from StoreAPI as prices
        };
    }
}
