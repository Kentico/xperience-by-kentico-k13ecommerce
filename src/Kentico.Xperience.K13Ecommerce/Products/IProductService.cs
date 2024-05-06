using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Products;

/// <summary>
/// Product service.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Returns product pages filtered by request.
    /// </summary>
    /// <param name="request">Product page <paramref name="request"/>.</param>    
    Task<ICollection<KProductNode>> GetProductPages(ProductPageRequest request);


    /// <summary>
    /// Returns product prices for given products.
    /// </summary>
    /// <param name="productSkuIds">List of product IDs (SKUIDs).</param>
    /// <param name="currencyCode">Currency code</param>    
    Task<ICollection<ProductPricesResponse>> GetProductsPrices(IEnumerable<int> productSkuIds, string? currencyCode = null);


    /// <summary>
    /// Returns product price for product.
    /// </summary>
    /// <param name="skuId">SKUID for product or variant.</param>
    /// <param name="currencyCode">Currency code</param>    
    Task<ProductPricesResponse> GetProductPrices(int skuId, string? currencyCode = null);


    /// <summary>
    /// Returns inventory and price for product or variant.
    /// </summary>
    /// <param name="skuId">SKUID for product or variant.</param>
    /// <param name="currencyCode">Currency code.</param>    
    Task<ProductInventoryPriceInfo> GetVariantInventoryPriceInfo(int skuId, string? currencyCode = null);
}
