using CMS.Integration.K13Ecommerce;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.K13Ecommerce.Admin.Providers;

namespace Kentico.Xperience.K13Ecommerce.Admin;

internal class SettingsConfigurationModel
{
    [RequiredValidationRule]
    [DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductSKUFolderID, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 0)]
    public string ProductSKUFolderID { get; set; } = string.Empty;

    [RequiredValidationRule]
    [DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductVariantFolderID, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 1)]
    public string ProductVariantFolderID { get; set; } = string.Empty;

    [RequiredValidationRule]
    [DropDownComponent(Label = K13EcommerceSettingsConstants.SettingsProductImageFolderID, DataProviderType = typeof(ContentFolderOptionsProvider), Order = 2)]
    public string ProductImageFolderID { get; set; } = string.Empty;

    internal void MapToSettingsInfo(K13EcommerceSettingsInfo infoObject)
    {
        infoObject.K13EcommerceSettingsProductSKUFolderID = int.Parse(ProductSKUFolderID);
        infoObject.K13EcommerceSettingsProductVariantFolderID = int.Parse(ProductVariantFolderID);
        infoObject.K13EcommerceSettingsProductImageFolderID = int.Parse(ProductImageFolderID);
    }
}
