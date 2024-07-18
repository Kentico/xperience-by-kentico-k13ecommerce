using CMS.Helpers;
using CMS.Membership;
using CMS.Websites;

using Kentico.Xperience.K13Ecommerce.WebsiteChannel;

namespace Kentico.Xperience.K13Ecommerce.WebPageFolders
{
    internal class WebPageFolderService : IWebPageFolderService
    {
        private readonly IWebPageFolderRetriever webPageFolderRetriever;
        private readonly IProgressiveCache progressiveCache;
        private readonly IWebsiteChannelProvider websiteChannelProvider;
        private readonly IWebPageManagerFactory webPageManagerFactory;

        public WebPageFolderService(
            IWebPageFolderRetriever webPageFolderRetriever,
            IProgressiveCache progressiveCache,
            IWebsiteChannelProvider websiteChannelProvider,
            IWebPageManagerFactory webPageManagerFactory)
        {
            this.webPageFolderRetriever = webPageFolderRetriever;
            this.progressiveCache = progressiveCache;
            this.websiteChannelProvider = websiteChannelProvider;
            this.webPageManagerFactory = webPageManagerFactory;
        }

        public async Task<WebPageFolder?> CreateWebPageFolder(string folderPath, string websiteChannelName, string languageName, CancellationToken cancellationToken = default)
        {
            // Split the path into individual folder names
            string[] folders = folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            WebPageFolder? currentFolder = null;
            int lastExistingFolderIndex = -1;

            int maxFoldersIndex = folders.Length - 1;
            for (int i = maxFoldersIndex; i >= 0; i--)
            {
                // Try to find the deepest existing folder
                string currentPath = string.Join("/", folders, 0, i + 1);
                currentFolder = await GetFolderByPath(currentPath, websiteChannelName);
                if (currentFolder != null)
                {
                    lastExistingFolderIndex = i;
                    break;
                }
            }

            if (lastExistingFolderIndex == maxFoldersIndex || currentFolder is null)
            {
                // Folder is created or invalid path provided.
                return currentFolder;
            }

            int websiteChannelId = await websiteChannelProvider.GetWebsiteChannelId(websiteChannelName, cancellationToken);
            var webPageManager = webPageManagerFactory.Create(websiteChannelId, UserInfoProvider.AdministratorUser.UserID);
            int currentFolderId = currentFolder.WebPageItemID;
            // Create any necessary folders starting from the first non-existing folder
            for (int i = lastExistingFolderIndex + 1; i < folders.Length; i++)
            {
                string folderToCreate = folders[i];

                var createFolderParameters = new CreateFolderParameters(folderToCreate, languageName)
                {
                    ParentWebPageItemID = currentFolderId
                };
                currentFolderId = await webPageManager.CreateFolder(createFolderParameters, cancellationToken);
            }

            // Folder is already created
            return await GetFolderByPath(folderPath, websiteChannelName);
        }

        private async Task<WebPageFolder?> GetFolderByPath(string folderPath, string websiteChannelName)
        {
            string cacheKey = CacheHelper.BuildCacheItemName(new[]
            {
                "webpageitem",
                "bychannel",
                websiteChannelName,
                "bypath",
                folderPath
            });
            var folderResult = await progressiveCache.LoadAsync(
                async cacheSettings =>
                {
                    var result = await webPageFolderRetriever.Retrieve(websiteChannelName, PathMatch.Single(folderPath));

                    cacheSettings.CacheDependency = CacheHelper.GetCacheDependency(
                        cacheKey
                    );
                    return result;
                },
                new CacheSettings(TimeSpan.FromMinutes(30).TotalMinutes, useSlidingExpiration: true, $"{websiteChannelName}|WebPageFolder|{folderPath}")
            );
            var folder = folderResult.FirstOrDefault();
            if (folder is null)
            {
                CacheHelper.TouchKey(cacheKey);
            }
            return folder;
        }
    }
}
