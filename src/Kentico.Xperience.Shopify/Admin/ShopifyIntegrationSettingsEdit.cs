using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.Shopify.Admin;

[assembly: UIPage(
    parentType: typeof(ShopifyIntegrationSettingsApplication),
    slug: "shopify-settings-edit",
    uiPageType: typeof(ShopifyIntegrationSettingsEdit),
    name: "Shopify settings",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.Shopify.Admin
{
    public class ShopifyIntegrationSettingsEdit : ModelEditPage<ShopifySettingsModel>
    {
        public ShopifyIntegrationSettingsEdit(Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider formItemCollectionProvider, IFormDataBinder formDataBinder) : base(formItemCollectionProvider, formDataBinder)
        {
        }

        private IntegrationSettingsInfo? settingsInfo;

        private IntegrationSettingsInfo? SettingsInfo => settingsInfo ??= IntegrationSettingsInfo.Provider.Get()
            .TopN(1)
            .FirstOrDefault();

        private ShopifySettingsModel? model;


        protected override ShopifySettingsModel Model => model ??= CreateShopifySettingsModel(SettingsInfo);



        protected override Task<ICommandResponse> ProcessFormData(ShopifySettingsModel model, ICollection<IFormItem> formItems)
        {
            var info = SettingsInfo ?? new IntegrationSettingsInfo();

            info.ShopifyUrl = model.ShopifyStoreUrl;
            info.AdminApiKey = model.AdminApiKey;
            info.StorefrontApiKey = model.StorefrontApiKey;
            info.StorefrontApiVersion = model.StorefrontApiVersion;

            IntegrationSettingsInfo.Provider.Set(info);

            return base.ProcessFormData(model, formItems);
        }

        private ShopifySettingsModel CreateShopifySettingsModel(IntegrationSettingsInfo? integrationSettings)
        {
            if (integrationSettings == null)
            {
                return new ShopifySettingsModel();
            }

            return new ShopifySettingsModel()
            {
                AdminApiKey = integrationSettings.AdminApiKey,
                StorefrontApiKey = integrationSettings.StorefrontApiKey,
                ShopifyStoreUrl = integrationSettings.ShopifyUrl,
                StorefrontApiVersion = integrationSettings.StorefrontApiVersion,
            };
        }
    }
}
