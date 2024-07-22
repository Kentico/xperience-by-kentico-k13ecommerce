using CMS.DataEngine;
using CMS.Integration.K13Ecommerce;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.K13Ecommerce.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(K13EcommerceApplicationPage),
    slug: "editsettings",
    uiPageType: typeof(SettingsEditPage),
    name: "K13Ecommerce settings",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.K13Ecommerce.Admin.UIPages;

internal class SettingsEditPage : ModelEditPage<SettingsConfigurationModel>
{
    private SettingsConfigurationModel? model;
    private int objectID;

    protected override SettingsConfigurationModel Model
    {
        get
        {
            if (model != null)
            {
                return model;
            }

            var info = k13EcommerceSettingsInfoProvider.Get(ObjectID);
            if (info == null)
            {
                return new SettingsConfigurationModel();
            }

            model = new SettingsConfigurationModel()
            {
                ProductSKUFolderID = info.K13EcommerceSettingsProductSKUFolderID.ToString(),
                ProductVariantFolderID = info.K13EcommerceSettingsProductVariantFolderID.ToString(),
                ProductImageFolderID = info.K13EcommerceSettingsProductImageFolderID.ToString(),
            };
            return model;
        }
    }

    private readonly IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider;

    public SettingsEditPage(Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider formItemCollectionProvider, IFormDataBinder formDataBinder, IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider)
        : base(formItemCollectionProvider, formDataBinder)
    {
        this.k13EcommerceSettingsInfoProvider = k13EcommerceSettingsInfoProvider;
    }

    public int ObjectID
    {
        get
        {
            if (objectID == 0)
            {
                var settings = k13EcommerceSettingsInfoProvider.Get()
                    .TopN(1)
                    .Column(nameof(K13EcommerceSettingsInfo.K13EcommerceSettingsID))
                    .GetEnumerableTypedResult()
                    .FirstOrDefault();
                objectID = settings?.K13EcommerceSettingsID ?? 0;
            }
            return objectID;
        }
    }

    protected override async Task<ICommandResponse> ProcessFormData(
        SettingsConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        var info = await k13EcommerceSettingsInfoProvider.GetAsync(ObjectID);

        model.MapToSettingsInfo(info);

        k13EcommerceSettingsInfoProvider.Set(info);

        return await base.ProcessFormData(model, formItems);
    }
}
