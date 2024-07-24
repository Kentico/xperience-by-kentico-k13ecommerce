using CMS.DataEngine;
using CMS.Integration.K13Ecommerce;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.K13Ecommerce.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(PagePathMappingRuleListingPage),
    slug: "create",
    uiPageType: typeof(PagePathMappingRuleCreatePage),
    name: "Create new Page Path mapping rule",
    templateName: TemplateNames.EDIT,
    order: 200)]

namespace Kentico.Xperience.K13Ecommerce.Admin.UIPages;

internal class PagePathMappingRuleCreatePage : ModelEditPage<PagePathMappingRuleConfigurationModel>
{
    private readonly IInfoProvider<PagePathMappingRuleInfo> pagePathMappingRuleInfoProvider;
    private readonly IPageUrlGenerator pageUrlGenerator;

    public PagePathMappingRuleCreatePage(Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider formItemCollectionProvider, IFormDataBinder formDataBinder, IInfoProvider<PagePathMappingRuleInfo> pagePathMappingRuleInfoProvider, IPageUrlGenerator pageUrlGenerator)
        : base(formItemCollectionProvider, formDataBinder)
    {
        this.pagePathMappingRuleInfoProvider = pagePathMappingRuleInfoProvider;
        this.pageUrlGenerator = pageUrlGenerator;
    }

    private PagePathMappingRuleConfigurationModel? model;
    protected override PagePathMappingRuleConfigurationModel Model => model ??= new PagePathMappingRuleConfigurationModel();

    protected override async Task<ICommandResponse> ProcessFormData(PagePathMappingRuleConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        await CreatePagePathMappingRuleInfo(model);

        var navigateResponse = await NavigateToEditPage(model, formItems);

        return navigateResponse;
    }

    private async Task<INavigateResponse> NavigateToEditPage(PagePathMappingRuleConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        var baseResult = await base.ProcessFormData(model, formItems);

        var navigateResponse = NavigateTo(
            pageUrlGenerator.GenerateUrl<PagePathMappingRuleListingPage>());

        foreach (var message in baseResult.Messages)
        {
            navigateResponse.Messages.Add(message);
        }

        return navigateResponse;
    }

    private async Task CreatePagePathMappingRuleInfo(PagePathMappingRuleConfigurationModel model)
    {
        var infoObject = new PagePathMappingRuleInfo();
        var lastObject = (await pagePathMappingRuleInfoProvider.Get()
            .TopN(1)
            .Column(nameof(PagePathMappingRuleInfo.PagePathMappingRuleOrder))
            .OrderByDescending(nameof(PagePathMappingRuleInfo.PagePathMappingRuleOrder))
            .GetEnumerableTypedResultAsync())
            .FirstOrDefault();

        int order = 1;
        if (lastObject is not null)
        {
            order = lastObject.PagePathMappingRuleOrder + 1;
        }

        model.MapToPagePathMappingRuleInfo(infoObject, order);

        pagePathMappingRuleInfoProvider.Set(infoObject);
    }
}
