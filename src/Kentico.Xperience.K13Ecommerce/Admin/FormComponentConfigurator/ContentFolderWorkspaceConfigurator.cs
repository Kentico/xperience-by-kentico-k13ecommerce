using CMS.ContentEngine;
using CMS.Membership;

using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.K13Ecommerce.Admin.FormComponentConfigurator;

[assembly: RegisterFormComponentConfigurator(ContentFolderWorkspaceConfigurator.IDENTIFIER, typeof(ContentFolderWorkspaceConfigurator))]

namespace Kentico.Xperience.K13Ecommerce.Admin.FormComponentConfigurator;

internal class ContentFolderWorkspaceConfigurator : FormComponentConfigurator<ContentFolderSelectorComponent>
{
    public const string IDENTIFIER = "ContentFolderWorkspaceConfigurator";

    private readonly IContentFolderManager contentFolderManager;

    public ContentFolderWorkspaceConfigurator(IContentFolderManagerFactory contentFolderManagerFactory)
    {
        contentFolderManager = contentFolderManagerFactory.Create(UserInfoProvider.AdministratorUser.UserID);
    }


    public override Task Configure(ContentFolderSelectorComponent formComponent, IFormFieldValueProvider formFieldValueProvider, CancellationToken cancellationToken)
    {
        if (!formFieldValueProvider.TryGet(nameof(SettingsConfigurationModel.WorkspaceName), out string? workspaceName) || string.IsNullOrEmpty(workspaceName))
        {
            formComponent.AddVisibilityCondition(new Invisible());
            return Task.CompletedTask;
        }

        formComponent.Properties.WorkspaceName = workspaceName;
        return Task.CompletedTask;
    }

    public override async Task<object> ConfigureValue(IFormFieldValueProvider formFieldValueProvider, string changedFieldName, CancellationToken cancellationToken)
    {
        if (changedFieldName == nameof(SettingsConfigurationModel.WorkspaceName) && formFieldValueProvider.TryGet(nameof(SettingsConfigurationModel.WorkspaceName), out string? workspaceName) && !string.IsNullOrEmpty(workspaceName))
        {
            var newRoot = await contentFolderManager.GetRoot(workspaceName, cancellationToken);
            return newRoot.ContentFolderID;
        }
        return await base.ConfigureValue(formFieldValueProvider, changedFieldName, cancellationToken);
    }
}
internal class Invisible : VisibilityCondition
{
    public override bool Evaluate(IFormFieldValueProvider formFieldValueProvider)
    {
        return false;
    }
}
