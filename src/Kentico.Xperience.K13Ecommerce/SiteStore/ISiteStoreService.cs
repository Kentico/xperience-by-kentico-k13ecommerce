using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.SiteStore;

/// <summary>
/// Interface for site store service.
/// </summary>
public interface ISiteStoreService
{
    /// <summary>
    /// Returns enabled cultures on the K13 store.
    /// </summary>    
    Task<ICollection<KCulture>> GetCultures();

    /// <summary>
    /// Returns enabled currencies on the K13 store.
    /// </summary>    
    Task<ICollection<string>> GetCurrencies();
}
