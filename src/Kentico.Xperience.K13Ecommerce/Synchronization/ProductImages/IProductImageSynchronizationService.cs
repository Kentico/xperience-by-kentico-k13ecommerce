using K13Store;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ProductImages;

public interface IProductImageSynchronizationService
{
    /// <summary>
    /// Process images from source K13 eshop to XbyK and save them to content hub
    /// </summary>
    /// <param name="images"></param>
    /// <param name="existingImages"></param>
    /// <param name="language"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IReadOnlySet<Guid>> ProcessImages(IEnumerable<ProductImageDto> images, IEnumerable<ProductImage> existingImages,
        string language, int userId);
}
