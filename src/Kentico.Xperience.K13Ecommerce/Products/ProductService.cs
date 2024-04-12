using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Products;

internal class ProductService(IKenticoStoreApiClient storeApiClient) : IProductService
{
    public async Task<ICollection<KProductNode>> GetProductPages(ProductPageRequest request)
        => await storeApiClient.GetProductPagesAsync(request.Path, request.Culture, request.Currency, request.OrderBy,
            request.Limit, request.WithVariants, request.WithLongDescription, request.NoLinks);


    public async Task<ICollection<ProductPricesResponse>> GetProductsPrices(IEnumerable<int> productSkuIds,
        string? currencyCode = null) =>
        //@TODO caching individually for each product to prevent next loading on product detail 
        await storeApiClient.GetProductPricesListAsync(productSkuIds, currencyCode);


    public async Task<ProductPricesResponse> GetProductPrices(int skuId, string? currencyCode = null) =>
        await storeApiClient.GetProductPricesAsync(skuId, currencyCode);


    public async Task<ProductInventoryPriceInfo> GetVariantInventoryPriceInfo(int skuId, string? currencyCode = null)
        => await storeApiClient.GetInventoryPricesAsync(skuId, currencyCode);
}
