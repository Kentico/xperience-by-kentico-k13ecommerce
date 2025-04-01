using CMS.Workspaces.Internal;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Base.FormAnnotations.Internal;
using Kentico.Xperience.Ecommerce.Common.Admin;

namespace Kentico.Xperience.K13Ecommerce.Admin;

internal class SettingsConfigurationModel : SynchronizationSettingsModel
{
    [TextInputComponent(Disabled = true, Order = 0)]
    [HiddenVisibility]
    public new string WorkspaceName { get; set; } = WorkspaceConstants.WORKSPACE_DEFAULT_CODE_NAME;
}

