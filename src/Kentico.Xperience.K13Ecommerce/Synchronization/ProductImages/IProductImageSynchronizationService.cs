using K13Store;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

/// <summary>
/// Product image synchronization service.
/// </summary>
public interface IProductImageSynchronizationService
{
    /// <summary>
    /// Process images from source K13 eshop to XbyK and save them to content hub.
    /// </summary>
    /// <param name="images">Images DTOs.</param>
    /// <param name="existingImages">Existing images.</param>
    /// <param name="language">Current language.</param>
    /// <param name="userId">Admin User ID.</param>
    /// <returns></returns>
    Task<IReadOnlySet<Guid>> ProcessImages(IEnumerable<ProductImageDto> images, IEnumerable<ProductImage> existingImages,
        string language, int userId);
}
