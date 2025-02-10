using CMS.Workspaces.Internal;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.K13Ecommerce.Admin.FormComponentConfigurator;
using Kentico.Xperience.K13Ecommerce.Admin.Providers;

namespace Kentico.Xperience.K13Ecommerce.Admin;

internal class SettingsConfigurationModel
{
    [RequiredValidationRule]
    [DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsWorkspaceName, DataProviderType = typeof(WorkspaceOptionsProvider), Order = 0)]
    public string WorkspaceName { get; set; } = WorkspaceConstants.WORKSPACE_DEFAULT_CODE_NAME;

    [RequiredValidationRule]
    //[DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductSKUFolderGuid, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 10)]
    [ContentFolderSelectorComponent(Label = K13EcommerceSettingsConstants.SettingsProductSKUFolderGuid, Order = 10)]
    [FormComponentConfiguration(ContentFolderWorkspaceConfigurator.IDENTIFIER, nameof(WorkspaceName))]
    public int ProductSKUFolderId { get; set; }

    [RequiredValidationRule]
    //[DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductVariantFolderGuid, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 20)]
    [ContentFolderSelectorComponent(Label = K13EcommerceSettingsConstants.SettingsProductVariantFolderGuid, Order = 20)]
    [FormComponentConfiguration(ContentFolderWorkspaceConfigurator.IDENTIFIER, nameof(WorkspaceName))]
    public int ProductVariantFolderId { get; set; }

    [RequiredValidationRule]
    //[DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductImageFolderGuid, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 30)]
    [ContentFolderSelectorComponent(Label = K13EcommerceSettingsConstants.SettingsProductImageFolderGuid, Order = 30)]
    [FormComponentConfiguration(ContentFolderWorkspaceConfigurator.IDENTIFIER, nameof(WorkspaceName))]
    public int ProductImageFolderId { get; set; }

    /*[ContentFolderSelectorComponent(Label = "Location", Order = 69)]
    [FormComponentConfiguration(ContentFolderWorkspaceConfigurator.IDENTIFIER, nameof(WorkspaceName))]
    public int ContentItemFolderId { get; set; }*/


    /*internal void MapToSettingsInfo(K13EcommerceSettingsInfo infoObject)
    {
        //ContentFolderSelectorComponent
        //Kentico.Xperience.Admin.Base.ContentListingCommandManager
        //ContentHubList
        //DropDownComponent
        //ContentFolderSelectorComponent
        /*SmartFolderSelectorComponent
        Kentico.Xperience.Admin.Base.FormAnnotations.SmartFolderSelectorComponentAttribute
        infoObject.K13EcommerceSettingsProductSKUFolderGuid = Guid.Parse(ProductSKUFolderGuid);
        infoObject.K13EcommerceSettingsProductVariantFolderGuid = Guid.Parse(ProductVariantFolderGuid);
        infoObject.K13EcommerceSettingsProductImageFolderGuid = Guid.Parse(ProductImageFolderGuid.ToString());
        infoObject.K13EcommerceSettingsWorkspaceName = WorkspaceName;
    }*/
}
