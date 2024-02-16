using Kentico.Xperience.K13Ecommerce.Products;

namespace Kentico.Xperience.K13Ecommerce.KenticoStoreApi;

internal class KStoreApiService(HttpClient httpClient) : IKStoreApiService
{
    private readonly KenticoStoreApiClient apiClient = new(httpClient);

    public async Task<ICollection<KProductNode>> GetProductPages(ProductPageRequest request)
    => await apiClient.ListingAsync(request.Path, request.Culture, request.Currency, request.OrderBy, request.Limit);

    public async Task<ICollection<KProductCategory>> GetProductCategories(string? culture = null)
    => await apiClient.CategoriesAsync(culture);
    
    public async Task<ICollection<KCulture>> GetCultures()
    => await apiClient.CulturesAsync();

    public async Task<ICollection<string>> GetCurrencies()
    => await apiClient.CurrenciesAsync();
}
