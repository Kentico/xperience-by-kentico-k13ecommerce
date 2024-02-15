namespace Kentico.Xperience.K13Ecommerce.KenticoStoreApi;

public interface IKStoreApiService
{
    Task<ICollection<KProductNode>> GetProductPages(string path, string? culture = null, string? currency = null, string? orderBy = null, int? limit = null);
    Task<ICollection<KProductCategory>> GetProductCategories(string? culture = null);
    Task<ICollection<KCulture>> GetCultures();
    Task<ICollection<string>> GetCurrencies();
}
