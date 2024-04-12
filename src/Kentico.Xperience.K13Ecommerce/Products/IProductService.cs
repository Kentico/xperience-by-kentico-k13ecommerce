using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Products;

public interface IProductService
{
    /// <summary>
    /// Returns product pages filtered by request 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ICollection<KProductNode>> GetProductPages(ProductPageRequest request);


    /// <summary>
    /// Returns product prices for given products
    /// </summary>
    /// <param name="productSkuIds"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<ICollection<ProductPricesResponse>> GetProductsPrices(IEnumerable<int> productSkuIds, string? currencyCode = null);


    /// <summary>
    /// Returns product product for product
    /// </summary>
    /// <param name="skuId"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<ProductPricesResponse> GetProductPrices(int skuId, string? currencyCode = null);


    /// <summary>
    /// Returns inventory and price for product or variant
    /// </summary>
    /// <param name="skuId"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<ProductInventoryPriceInfo> GetVariantInventoryPriceInfo(int skuId, string? currencyCode = null);
}
