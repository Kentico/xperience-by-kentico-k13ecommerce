using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.SiteStore;
internal class SiteStoreService(IKenticoStoreApiClient storeApiClient) : ISiteStoreService
{
    /// <inheritdoc/>
    public async Task<ICollection<KCulture>> GetCultures() => await storeApiClient.GetCulturesAsync();

    /// <inheritdoc/>
    public async Task<ICollection<string>> GetCurrencies() => await storeApiClient.GetCurrenciesAsync();
}
