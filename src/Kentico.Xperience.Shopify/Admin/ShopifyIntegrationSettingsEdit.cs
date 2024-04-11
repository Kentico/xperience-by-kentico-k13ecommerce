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

        private ShopifySettingsModel? model;

        protected override ShopifySettingsModel Model => model ??= new ShopifySettingsModel() { ShopifyStoreUrl = "test" };

        protected override Task<ICommandResponse> ProcessFormData(ShopifySettingsModel model,
       ICollection<IFormItem> formItems)
        {
            return base.ProcessFormData(model, formItems);
        }
    }
}
