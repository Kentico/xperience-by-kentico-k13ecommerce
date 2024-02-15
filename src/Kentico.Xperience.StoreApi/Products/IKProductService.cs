using Kentico.Xperience.StoreApi.Products.Categories;
using Kentico.Xperience.StoreApi.Products.Pages;

namespace Kentico.Xperience.StoreApi.Products;

public interface IKProductService
{
    Task<IEnumerable<KProductNode>> GetProductPages(string path, string culture, string currencyCode, int limit, string orderBy);

    Task<IEnumerable<KProductCategory>> GetProductCategories(string culture);
}
