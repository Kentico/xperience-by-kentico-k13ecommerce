using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Integration.K13Ecommerce;
using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.Admin.Base.Forms.Internal;
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
                WorkspaceName = info.K13EcommerceSettingsEffectiveWorkspaceName,
                ProductSKUFolderId = GetFolderIdByGuid(info.K13EcommerceSettingsProductSKUFolderGuid),
                ProductVariantFolderId = GetFolderIdByGuid(info.K13EcommerceSettingsProductVariantFolderGuid),
                ProductImageFolderId = GetFolderIdByGuid(info.K13EcommerceSettingsProductImageFolderGuid),
            };
            return model;
        }
    }

    private readonly IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider;
    private readonly IContentFolderManager contentFolderManager;

    public SettingsEditPage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider,
        IContentFolderManagerFactory contentFolderManagerFactory
        )
        : base(formItemCollectionProvider, formDataBinder)
    {
        this.k13EcommerceSettingsInfoProvider = k13EcommerceSettingsInfoProvider;
        contentFolderManager = contentFolderManagerFactory.Create(UserInfoProvider.AdministratorUser.UserID);
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

        info.K13EcommerceSettingsWorkspaceName = model.WorkspaceName;
        info.K13EcommerceSettingsProductSKUFolderGuid = await GetFolderGuidById(model.ProductSKUFolderId);
        info.K13EcommerceSettingsProductVariantFolderGuid = await GetFolderGuidById(model.ProductVariantFolderId);
        info.K13EcommerceSettingsProductImageFolderGuid = await GetFolderGuidById(model.ProductImageFolderId);

        k13EcommerceSettingsInfoProvider.Set(info);

        return await base.ProcessFormData(model, formItems);
    }

    private async Task<Guid> GetFolderGuidById(int contentFolderId)
    {
        var contentFolder = await contentFolderManager.Get(contentFolderId);
        return contentFolder?.ContentFolderGUID ?? Guid.Empty;
    }

    private int GetFolderIdByGuid(Guid contentFolderGuid)
    {
        var contentFolder = contentFolderManager.Get()
            .WhereEquals(nameof(ContentFolderInfo.ContentFolderGUID), contentFolderGuid)
            .GetEnumerableTypedResult()
            .FirstOrDefault();
        return contentFolder?.ContentFolderID ?? 0;
    }
}
