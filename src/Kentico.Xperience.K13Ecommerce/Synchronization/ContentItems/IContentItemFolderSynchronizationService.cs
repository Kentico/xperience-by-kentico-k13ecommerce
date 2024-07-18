namespace Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;

/// <summary>
/// Product folder synchronization service.
/// </summary>
public interface IContentItemFolderSynchronizationService
{
    /// <summary>
    /// Synchronize XbK content items to selected destination folder.
    /// </summary>
    /// <returns></returns>
    Task SynchronizeContentItemFolders(CancellationToken cancellationToken = default);
}
