using K13Store;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductVariants;

public interface IProductVariantSynchronizationService
{
    /// <summary>
    /// Synchronize product variants from K13 shop to XByK content hub.
    /// </summary>
    /// <param name="variants">Product variants from K13 Store.</param>
    /// <param name="existingVariants">Existing variants content items.</param>
    /// <param name="language">Current language.</param>
    /// <param name="userId">Admin User ID.</param>
    /// <returns></returns>
    Task<IReadOnlySet<Guid>> ProcessVariants(IEnumerable<KProductVariant> variants, IEnumerable<ProductVariant> existingVariants,
        string language, int userId);
}
