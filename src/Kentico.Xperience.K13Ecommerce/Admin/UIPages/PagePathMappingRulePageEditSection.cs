using CMS.Integration.K13Ecommerce;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.K13Ecommerce.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(PagePathMappingRuleListingPage),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(PagePathMappingRulePageEditSection),
    name: "Edit",
    templateName: TemplateNames.SECTION_LAYOUT,
    order: 0)]

namespace Kentico.Xperience.K13Ecommerce.Admin.UIPages;

[UINavigation(false)]
[UIBreadcrumbs(false)]
internal class PagePathMappingRulePageEditSection : EditSectionPage<PagePathMappingRuleInfo>
{
}
