using Kentico.Xperience.StoreApi.Products.Categories;
using Kentico.Xperience.StoreApi.Products.Pages;
using Kentico.Xperience.StoreApi.Products.Prices;

namespace Kentico.Xperience.StoreApi.Products;

public interface IKProductService
{
    Task<IEnumerable<KProductNode>> GetProductPages(ProductPageRequest request);

    Task<IEnumerable<KProductCategory>> GetProductCategories(string culture);

    Task<ProductPricesResponse> GetProductPrices(int productSkuId, string currencyCode);
    Task<ProductInventoryPriceInfo> GetProductInventoryAndPrices(int skuId, string currencyCode);

    IAsyncEnumerable<ProductPricesResponse> GetProductPrices(IEnumerable<int> productsSkuIds, string currencyCode);
}
