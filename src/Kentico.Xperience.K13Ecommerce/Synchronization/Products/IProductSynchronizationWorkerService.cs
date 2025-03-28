namespace Kentico.Xperience.K13Ecommerce.Synchronization.Products
{
    /// <summary>
    /// Product synchronization service for <see cref="ProductSynchronizationWorker"/>
    /// </summary>
    public interface IProductSynchronizationWorkerService
    {
        /// <summary>
        /// Synchronize products with all dependencies.
        /// </summary>
        Task SynchronizeProducts();
    }
}
