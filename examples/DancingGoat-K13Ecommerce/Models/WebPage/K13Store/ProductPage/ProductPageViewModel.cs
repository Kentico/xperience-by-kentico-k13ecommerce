using CMS.Base;

using K13Store;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace DancingGoat.Models;

public class ProductPageViewModel
{
    public KProductCatalogPrices Prices { get; init; }


    //public IEnumerable<ProductOptionCategoryViewModel> ProductOptionCategories { get; }


    public int? SelectedVariantID { get; init; }


    public bool IsInStock { get; init; }


    public bool AllowSale { get; init; }


    public string Name { get; init; }


    public string Description { get; init; }


    public string ShortDescription { get; init; }


    public int SKUID { get; init; }


    public string ImagePath { get; init; }


    public Dictionary<string, object> ParametersSection { get; init; }


    public IEnumerable<ProductVariant> Variants { get; init; }


    public static ProductPageViewModel GetViewModel(ProductSKU productSku, ProductPricesResponse pricesResponse)
    {
        var variant = productSku.ProductVariants.Any() ? GetCheapestVariant(productSku, pricesResponse) : null;

        bool isInStock = productSku.SKUTrackInventory == "Disabled" ||
                         (variant != null ? variant.SKUAvailableItems > 0 : productSku.SKUAvailableItems > 0);

        bool allowForSale = isInStock || !productSku.SKUSellOnlyAvailable;

        var model = new ProductPageViewModel
        {
            // Set page information
            Name = productSku.SKUName,
            Description = productSku.SKUDescription,
            ShortDescription = productSku.SKUShortDescription,

            // Set SKU information
            SKUID = productSku.SKUID,
            ImagePath = productSku.ProductImages.FirstOrDefault()?.ProductImageAsset?.Url ?? string.Empty,
            IsInStock = isInStock,
            AllowSale = allowForSale,

            // Set additional info
            Prices = variant != null ? pricesResponse.VariantPrices![variant.SKUID.ToString()] : pricesResponse.Prices,
            SelectedVariantID = variant?.SKUID,
            ParametersSection = productSku.CustomFieldsDict,
            Variants = productSku.ProductVariants.Where(v => v.SKUEnabled).OrderBy(v =>
                pricesResponse.VariantPrices?[v.SKUID.ToString()].Price ?? decimal.MaxValue).ToList()
        };

        return model;
    }


    private static ProductVariant GetCheapestVariant(ProductSKU productSku, ProductPricesResponse pricesResponse)
    {
        var cheapestVariant = pricesResponse.VariantPrices!.MinBy(p => p.Value.Price);

        return productSku.ProductVariants.FirstOrDefault(p => p.SKUID.ToString() == cheapestVariant.Key);
    }
}
