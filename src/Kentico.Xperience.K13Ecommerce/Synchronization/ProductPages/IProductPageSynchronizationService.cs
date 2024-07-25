namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductPages;

/// <summary>
/// Product page synchronization service.
/// </summary>
public interface IProductPageSynchronizationService
{
    /// <summary>
    /// Synchronize product pages to the Xbk web channel from XbK content hub.
    /// </summary>
    /// <returns></returns>
    Task SynchronizeProductPages();
}
