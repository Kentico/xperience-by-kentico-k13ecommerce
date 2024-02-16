using Kentico.Xperience.K13Ecommerce.Products;

namespace Kentico.Xperience.K13Ecommerce.KenticoStoreApi;

public interface IKStoreApiService
{
    Task<ICollection<KProductNode>> GetProductPages(ProductPageRequest request);
    Task<ICollection<KProductCategory>> GetProductCategories(string? culture = null);
    Task<ICollection<KCulture>> GetCultures();
    Task<ICollection<string>> GetCurrencies();
}
