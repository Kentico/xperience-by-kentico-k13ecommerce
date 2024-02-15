namespace Kentico.Xperience.K13Ecommerce.KenticoStoreApi;

internal class KStoreApiService : IKStoreApiService
{
    private readonly KenticoStoreApiClient apiClient;

    public KStoreApiService(HttpClient httpClient) => apiClient = new KenticoStoreApiClient(httpClient);

    public async Task<ICollection<KProductNode>> GetProductPages(string path, string? culture = null, string? currency = null, string? orderBy = null, int? limit = null)
    => await apiClient.ListingAsync(path, culture, currency, orderBy, limit);

    public async Task<ICollection<KProductCategory>> GetProductCategories(string? culture = null)
    => await apiClient.CategoriesAsync(culture);


    public async Task<ICollection<KCulture>> GetCultures()
    => await apiClient.CulturesAsync();

    public async Task<ICollection<string>> GetCurrencies()
    => await apiClient.CurrenciesAsync();
}
