using CMS.Integration.K13Ecommerce;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;

/// <summary>
/// Product folder synchronization service.
/// </summary>
public interface IContentItemFolderSynchronizationService
{
    /// <summary>
    /// Synchronize XbK content items to selected destination folder.
    /// </summary>
    /// <param name="ecommerceSettings">Ecommerce settings from XbyK admin.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    Task SynchronizeContentItemFolders(K13EcommerceSettingsInfo ecommerceSettings, CancellationToken cancellationToken = default);
}
