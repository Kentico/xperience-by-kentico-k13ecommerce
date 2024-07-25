using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using Kentico.Xperience.K13Ecommerce.Admin.UIPages;

[assembly: UIApplication(
    identifier: K13EcommerceApplicationPage.IDENTIFIER,
    type: typeof(K13EcommerceApplicationPage),
    slug: "k13ecommerce",
    name: "K13Ecommerce",
    category: BaseApplicationCategories.CONFIGURATION,
    icon: Icons.ShoppingCart,
    templateName: TemplateNames.SECTION_LAYOUT)]

namespace Kentico.Xperience.K13Ecommerce.Admin.UIPages;

/// <summary>
/// The root application page for the K13Ecommerce integration.
/// </summary>
[UIPermission(SystemPermissions.VIEW)]
[UIPermission(SystemPermissions.CREATE)]
[UIPermission(SystemPermissions.UPDATE)]
[UIPermission(SystemPermissions.DELETE)]
internal class K13EcommerceApplicationPage : ApplicationPage
{
    /// <summary>
    /// Unique identifier of K13Ecommerce application
    /// </summary>
    public const string IDENTIFIER = "k13ecommerce";
}
