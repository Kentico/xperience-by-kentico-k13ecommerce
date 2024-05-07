namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products;

/// <summary>
/// Product synchronization service.
/// </summary>
public interface IProductSynchronizationService
{
    /// <summary>
    /// Synchronize product from K13 shop to XByK content hub.
    /// </summary>
    /// <returns></returns>
    Task SynchronizeProducts();
}
