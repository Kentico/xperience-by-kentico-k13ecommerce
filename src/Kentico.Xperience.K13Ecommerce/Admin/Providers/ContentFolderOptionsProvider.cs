using CMS.ContentEngine;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.K13Ecommerce.Admin.Providers;

internal class ContentFolderOptionsProvider : IDropDownOptionsProvider
{
    private readonly IInfoProvider<ContentFolderInfo> contentFolderInfoProvider;

    public ContentFolderOptionsProvider(IInfoProvider<ContentFolderInfo> contentFolderInfoProvider)
        => this.contentFolderInfoProvider = contentFolderInfoProvider;

    public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems() =>
        (await contentFolderInfoProvider.Get()
            .GetEnumerableTypedResultAsync())
            .Select(folder => new DropDownOptionItem()
            {
                Value = folder.ContentFolderID.ToString(),
                Text = folder.ContentFolderTreePath
            });
}
