using CMS.Integration.K13Ecommerce;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.K13Ecommerce.Admin.Providers;

namespace Kentico.Xperience.K13Ecommerce.Admin;

internal class SettingsConfigurationModel
{
    [RequiredValidationRule]
    [DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductSKUFolderGuid, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 0)]
    public string ProductSKUFolderGuid { get; set; } = string.Empty;

    [RequiredValidationRule]
    [DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductVariantFolderGuid, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 1)]
    public string ProductVariantFolderGuid { get; set; } = string.Empty;

    [RequiredValidationRule]
    [DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductImageFolderGuid, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 2)]
    public string ProductImageFolderGuid { get; set; } = string.Empty;

    [TextInputComponent(Label = K13EcommerceSettingsConstants.SettingsWorkspaceName, Order = 3, ExplanationText = "If empty, default workspace name is used")]
    public string WorkspaceName { get; set; } = string.Empty;

    internal void MapToSettingsInfo(K13EcommerceSettingsInfo infoObject)
    {
        infoObject.K13EcommerceSettingsProductSKUFolderGuid = Guid.Parse(ProductSKUFolderGuid);
        infoObject.K13EcommerceSettingsProductVariantFolderGuid = Guid.Parse(ProductVariantFolderGuid);
        infoObject.K13EcommerceSettingsProductImageFolderGuid = Guid.Parse(ProductImageFolderGuid);
        infoObject.K13EcommerceSettingsWorkspaceName = WorkspaceName;
    }
}
