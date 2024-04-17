using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.SiteStore;

public interface ISiteStoreService
{
    Task<ICollection<KCulture>> GetCultures();

    Task<ICollection<string>> GetCurrencies();
}
