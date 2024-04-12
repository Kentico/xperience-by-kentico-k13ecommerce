using Kentico.Xperience.StoreApi.Products.Categories;
using Kentico.Xperience.StoreApi.Products.Pages;
using Kentico.Xperience.StoreApi.Products.Prices;

namespace Kentico.Xperience.StoreApi.Products;

/// <summary>
/// Service for products related data (products, categories, inventory, prices etc.)
/// </summary>
public interface IKProductService
{
    /// <summary>
    /// Get product pages based on request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IEnumerable<KProductNode>> GetProductPages(ProductPageRequest request);


    /// <summary>
    /// Get product categories for in given culture
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    Task<IEnumerable<KProductCategory>> GetProductCategories(string culture);


    /// <summary>
    /// Get product prices for product in given currency
    /// </summary>
    /// <param name="productSkuId"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<ProductPricesResponse> GetProductPrices(int productSkuId, string currencyCode);



    /// <summary>
    /// Get product invetory and price info for product/variant
    /// </summary>
    /// <param name="skuId"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<ProductInventoryPriceInfo> GetProductInventoryAndPrices(int skuId, string currencyCode);


    /// <summary>
    /// Get product prices for list of products in given currency
    /// </summary>
    /// <param name="productsSkuIds"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>

    IAsyncEnumerable<ProductPricesResponse> GetProductPrices(IEnumerable<int> productsSkuIds, string currencyCode);
}
