using CMS.Integration.K13Ecommerce;
using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.K13Ecommerce.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(K13EcommerceApplicationPage),
    slug: "pagepathmappingrule",
    uiPageType: typeof(PagePathMappingRuleListingPage),
    name: "Page Path mapping rules",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.NoOrder)]

namespace Kentico.Xperience.K13Ecommerce.Admin.UIPages;

internal class PagePathMappingRuleListingPage : ListingPage
{
    protected override string ObjectType => PagePathMappingRuleInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.HeaderActions.AddLink<PagePathMappingRuleCreatePage>("Add Page Path mapping rule");
        PageConfiguration.TableActions.AddDeleteAction(nameof(Delete), "Delete");

        PageConfiguration.AddEditRowAction<PagePathMappingRuleEditPage>();

        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(PagePathMappingRuleInfo.PagePathMappingRuleK13NodeAliasPath), K13EcommerceTableConstants.K13NodeAliasPathCaption, sortable: false)
            .AddColumn(nameof(PagePathMappingRuleInfo.PagePathMappingRuleXbKPagePath), K13EcommerceTableConstants.XbKPagePathCaption, sortable: false)
            .AddColumn(nameof(PagePathMappingRuleInfo.PagePathMappingRuleChannelName), K13EcommerceTableConstants.ChannelName, sortable: false);

        return base.ConfigurePage();
    }

    [PageCommand(Permission = SystemPermissions.DELETE)]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}
