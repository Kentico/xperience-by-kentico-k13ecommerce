using K13Store;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductVariants;

public interface IProductVariantSynchronizationService
{
    /// <summary>
    /// Synchronize product variants from K13 shop to XByK content hub
    /// </summary>
    /// <param name="variants"></param>
    /// <param name="existingVariants"></param>
    /// <param name="language"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IReadOnlySet<Guid>> ProcessVariants(IEnumerable<KProductVariant> variants, IEnumerable<ProductVariant> existingVariants,
        string language, int userId);
}
