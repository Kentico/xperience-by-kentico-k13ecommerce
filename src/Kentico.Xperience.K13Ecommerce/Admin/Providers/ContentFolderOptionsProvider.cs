using CMS.ContentEngine;
using CMS.ContentEngine.Internal;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.K13Ecommerce.Admin.Providers;

internal class ContentFolderOptionsProvider : IDropDownOptionsProvider
{
    private readonly IInfoProvider<ContentFolderInfo> contentFolderInfoProvider;

    public ContentFolderOptionsProvider(IInfoProvider<ContentFolderInfo> contentFolderInfoProvider)
        => this.contentFolderInfoProvider = contentFolderInfoProvider;

    public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems()
    {
        var result = new List<DropDownOptionItem>();
        var contentFolders = await contentFolderInfoProvider.Get()
            .GetEnumerableTypedResultAsync();

        foreach (var folder in contentFolders)
        {
            string displayName = await GetFolderDisplayNameLocation(folder);
            result.Add(new DropDownOptionItem()
            {
                Value = folder.ContentFolderGUID.ToString(),
                Text = displayName
            });
        }
        return result;
    }

    /// <summary>
    /// Generate display name location same as in <see cref="ContentFolderPropertiesModel.FolderDisplayNamesPath"/> (generated in <see cref="ContentFolderCommandManager.GetFormItemsForFolderProperties"/>).
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    private async Task<string> GetFolderDisplayNameLocation(ContentFolderInfo folder)
    {
        var folderDisplayNamePaths = await GetFolderDisplayNamePath(folder.ContentFolderTreePath);
        return "/" + string.Join('/', folderDisplayNamePaths);
    }

    private async Task<IList<string>> GetFolderDisplayNamePath(string folderPath)
    {
        var treePathsOnPath = TreePathUtils.GetTreePathsOnPath(folderPath, false, true);
        string[] columns = [nameof(ContentFolderInfo.ContentFolderTreePath)];
        return (await contentFolderInfoProvider.Get()
            .WhereIn(nameof(ContentFolderInfo.ContentFolderTreePath), treePathsOnPath)
            .OrderBy(columns)
            .Column(nameof(ContentFolderInfo.ContentFolderDisplayName))
            .GetEnumerableTypedResultAsync())
            .Select(folder => folder.ContentFolderDisplayName.Trim())
            .ToList();
    }
}
