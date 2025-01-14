using K13Store;

namespace Kentico.Xperience.K13Ecommerce.Admin;

internal static class K13EcommerceResourceConstants
{
    public const string ResourceDisplayName = "Kentico Integration - K13Ecommerce";
    public const string ResourceName = "CMS.Integration.K13Ecommerce";
    public const string ResourceDescription = "The module integrates connection betweeen XbK and K13 Ecommerce store.";
    public const bool ResourceIsInDevelopment = false;

}

internal static class K13EcommerceTableConstants
{
    public const string K13NodeAliasPathCaption = "K13 NodeAliasPath";
    public const string XbKPagePathCaption = "XbK Page path";
    public const string ChannelName = "Channel name";
    public const string OrderCaption = "Order";
}

internal static class K13EcommerceSettingsConstants
{
    public const string SettingsProductSKUFolderGuid = "Content item folder for '" + ProductSKU.CONTENT_TYPE_NAME + "'";
    public const string SettingsProductVariantFolderGuid = "Content item folder for '" + ProductVariant.CONTENT_TYPE_NAME + "'";
    public const string SettingsProductImageFolderGuid = "Content item folder for '" + ProductImage.CONTENT_TYPE_NAME + "'";
    public const string SettingsWorkspaceName = "Content item workspace name";
}
