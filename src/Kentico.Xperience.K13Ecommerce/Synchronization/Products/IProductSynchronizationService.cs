namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products;

public interface IProductSynchronizationService
{
    /// <summary>
    /// Synchronize product from K13 shop to XByK content hub.
    /// </summary>
    /// <returns></returns>
    Task SynchronizeProducts();
}
