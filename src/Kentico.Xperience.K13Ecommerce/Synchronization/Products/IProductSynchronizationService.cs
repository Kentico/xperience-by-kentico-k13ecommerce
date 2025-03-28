using CMS.Integration.K13Ecommerce;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products;

/// <summary>
/// Product synchronization service.
/// </summary>
public interface IProductSynchronizationService
{
    /// <summary>
    /// Synchronize product from K13 shop to XByK content hub.
    /// </summary>
    /// <param name="ecommerceSettings">Ecommerce settings from XbyK admin.</param>
    /// <returns></returns>
    Task SynchronizeProducts(K13EcommerceSettingsInfo ecommerceSettings);
}
