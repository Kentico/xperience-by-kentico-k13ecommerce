using CMS.DataEngine;
using CMS.Workspaces;

using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.K13Ecommerce.Admin.Providers
{
    internal class WorkspaceOptionsProvider(IInfoProvider<WorkspaceInfo> workspaceInfoProvider) : IDropDownOptionsProvider
    {
        public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems() =>
            (await workspaceInfoProvider.Get()
                .GetEnumerableTypedResultAsync())
                .Select(w => new DropDownOptionItem()
                {
                    Value = w.WorkspaceName,
                    Text = w.WorkspaceDisplayName
                });
    }
}
