using CMS.DataEngine;
using CMS.Integration.K13Ecommerce;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.K13Ecommerce.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(PagePathMappingRulePageEditSection),
    slug: "edit",
    uiPageType: typeof(PagePathMappingRuleEditPage),
    name: "Edit Page Path mapping rule",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.NoOrder)]

namespace Kentico.Xperience.K13Ecommerce.Admin.UIPages;

internal class PagePathMappingRuleEditPage : ModelEditPage<PagePathMappingRuleConfigurationModel>
{
    private PagePathMappingRuleConfigurationModel? model;

    protected override PagePathMappingRuleConfigurationModel Model
    {
        get
        {
            if (model != null)
            {
                return model;
            }

            var info = pagePathMappingRuleInfoProvider.Get(ObjectID);
            if (info == null)
            {
                return new PagePathMappingRuleConfigurationModel();
            }

            model = new PagePathMappingRuleConfigurationModel()
            {
                K13NodeAliasPath = info.PagePathMappingRuleK13NodeAliasPath,
                Order = info.PagePathMappingRuleOrder,
                XbKPagePath = info.PagePathMappingRuleXbKPagePath,
                ChannelName = info.PagePathMappingRuleChannelName,
            };
            return model;
        }
    }

    private readonly IInfoProvider<PagePathMappingRuleInfo> pagePathMappingRuleInfoProvider;

    public PagePathMappingRuleEditPage(Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider formItemCollectionProvider, IFormDataBinder formDataBinder, IInfoProvider<PagePathMappingRuleInfo> pagePathMappingRuleInfoProvider)
        : base(formItemCollectionProvider, formDataBinder)
    {
        this.pagePathMappingRuleInfoProvider = pagePathMappingRuleInfoProvider;
    }

    [PageParameter(typeof(IntPageModelBinder))]
    public int ObjectID { get; set; }

    protected override async Task<ICommandResponse> ProcessFormData(
        PagePathMappingRuleConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        var info = await pagePathMappingRuleInfoProvider.GetAsync(ObjectID);

        model.MapToPagePathMappingRuleInfo(info, info.PagePathMappingRuleOrder);

        pagePathMappingRuleInfoProvider.Set(info);

        return await base.ProcessFormData(model, formItems);
    }
}
