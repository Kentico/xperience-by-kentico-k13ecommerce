using CMS.Integration.K13Ecommerce;

using K13Store;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductVariants;

/// <summary>
/// Product variant synchronization service.
/// </summary>
public interface IProductVariantSynchronizationService
{
    /// <summary>
    /// Synchronize product variants from K13 shop to XByK content hub.
    /// </summary>
    /// <param name="variants">Product variants from K13 Store.</param>
    /// <param name="existingVariants">Existing variants content items.</param>
    /// <param name="ecommerceSettings">Ecommerce settings from XbyK admin.</param>
    /// <param name="language">Current language.</param>
    /// <param name="userId">Admin User ID.</param>
    /// <returns></returns>
    Task<IReadOnlySet<Guid>> ProcessVariants(IEnumerable<KProductVariant> variants, IEnumerable<ProductVariant> existingVariants,
        K13EcommerceSettingsInfo ecommerceSettings, string language, int userId);
}
