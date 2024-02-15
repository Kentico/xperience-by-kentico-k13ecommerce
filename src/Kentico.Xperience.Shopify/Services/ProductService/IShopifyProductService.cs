using Kentico.Xperience.Shopify.Models;

namespace Kentico.Xperience.Shopify.Services.ProductService
{
    public interface IShopifyProductService
    {
        /// <summary>
        /// Get products in collection
        /// </summary>
        /// <param name="initialFilter"></param>
        /// <returns></returns>
        Task<ListResultWrapper<ProductListModel>> GetProductsAsync(ProductFilter initialFilter);

        /// <summary>
        /// Get filtered products. This method is used for paging options.
        /// </summary>
        /// <param name="filterParams">Filter with Limit parameter and PageInfo returned in header of last API response</param>
        /// <returns></returns>
        Task<ListResultWrapper<ProductListModel>> GetProductsAsync(PagingFilterParams filterParams);
    }
}
