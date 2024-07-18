using CMS.Websites;

namespace Kentico.Xperience.K13Ecommerce.WebPageFolders;

public interface IWebPageFolderService
{
    /// <summary>
    /// Create web page folder if it does not exist. Create all required folders.
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="websiteChannelName"></param>
    /// <param name="languageName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WebPageFolder?> CreateWebPageFolder(string folderPath, string websiteChannelName, string languageName, CancellationToken cancellationToken = default);
}
