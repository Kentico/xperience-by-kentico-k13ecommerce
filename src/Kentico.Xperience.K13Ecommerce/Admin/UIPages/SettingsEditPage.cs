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
    private K13EcommerceSettingsInfo? settings;
    private SettingsConfigurationModel? model;

    protected override SettingsConfigurationModel Model => model ??= new();
    private K13EcommerceSettingsInfo Settings => settings ??= new();


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

    public override async Task ConfigurePage()
    {
        settings = (await k13EcommerceSettingsInfoProvider.Get()
            .TopN(1)
            .GetEnumerableTypedResultAsync())
            .FirstOrDefault();

        if (settings == null)
        {
            model = new SettingsConfigurationModel();
            return;
        }

        var contentFolderIds = await GetFolderIdsBySettings(settings);
        model = new SettingsConfigurationModel()
        {
            ProductFolderId = contentFolderIds.GetValueOrDefault(settings.K13EcommerceSettingsProductSKUFolderGuid),
            ProductVariantFolderId = contentFolderIds.GetValueOrDefault(settings.K13EcommerceSettingsProductVariantFolderGuid),
            ImageFolderId = contentFolderIds.GetValueOrDefault(settings.K13EcommerceSettingsProductImageFolderGuid),
        };
        await base.ConfigurePage();
    }

    protected override async Task<ICommandResponse> ProcessFormData(
        SettingsConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        Settings.K13EcommerceSettingsWorkspaceName = model.WorkspaceName;
        var contentFolderGuids = await GetFolderGuidsByModel(model);
        Settings.K13EcommerceSettingsProductSKUFolderGuid = contentFolderGuids.GetValueOrDefault(model.ProductFolderId);
        Settings.K13EcommerceSettingsProductVariantFolderGuid = contentFolderGuids.GetValueOrDefault(model.ProductVariantFolderId);
        Settings.K13EcommerceSettingsProductImageFolderGuid = contentFolderGuids.GetValueOrDefault(model.ImageFolderId);

        k13EcommerceSettingsInfoProvider.Set(Settings);

        return await base.ProcessFormData(model, formItems);
    }

    private async Task<Dictionary<Guid, int>> GetFolderIdsBySettings(K13EcommerceSettingsInfo settings)
    {
        ICollection<Guid> contentFolderGuids = [
            settings.K13EcommerceSettingsProductSKUFolderGuid,
            settings.K13EcommerceSettingsProductVariantFolderGuid,
            settings.K13EcommerceSettingsProductImageFolderGuid
        ];

        var folders = await contentFolderManager.Get()
                .WhereIn(nameof(ContentFolderInfo.ContentFolderGUID), contentFolderGuids)
                .Columns(nameof(ContentFolderInfo.ContentFolderID), nameof(ContentFolderInfo.ContentFolderGUID))
                .GetEnumerableTypedResultAsync();

        return folders.ToDictionary(x => x.ContentFolderGUID, x => x.ContentFolderID);
    }

    private async Task<Dictionary<int, Guid>> GetFolderGuidsByModel(SettingsConfigurationModel model)
    {
        ICollection<int> contentFolderIds = [
            model.ProductFolderId,
            model.ProductVariantFolderId,
            model.ImageFolderId
        ];

        var folders = await contentFolderManager.Get()
            .WhereIn(nameof(ContentFolderInfo.ContentFolderID), contentFolderIds)
            .Columns(nameof(ContentFolderInfo.ContentFolderID), nameof(ContentFolderInfo.ContentFolderGUID))
            .GetEnumerableTypedResultAsync();

        return folders.ToDictionary(x => x.ContentFolderID, x => x.ContentFolderGUID);
    }
}
