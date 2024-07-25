using CMS.Integration.K13Ecommerce;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.K13Ecommerce.Admin.Providers;
using Kentico.Xperience.K13Ecommerce.Admin.Validations;

namespace Kentico.Xperience.K13Ecommerce.Admin
{
    internal class PagePathMappingRuleConfigurationModel
    {
        [RequiredValidationRule]
        [NodeAliasPathValidationRule]
        [TextInputComponent(Label = K13EcommerceTableConstants.K13NodeAliasPathCaption, Order = 0)]
        public string K13NodeAliasPath { get; set; } = string.Empty;

        [RequiredValidationRule]
        [NodeAliasPathValidationRule]
        [XbKPagePathValidationRule]
        [TextInputComponent(Label = K13EcommerceTableConstants.XbKPagePathCaption, Order = 1)]
        public string XbKPagePath { get; set; } = string.Empty;

        [RequiredValidationRule]
        [DropDownComponent(Label = K13EcommerceTableConstants.ChannelName, DataProviderType = typeof(ChannelOptionsProvider), Order = 3)]
        public string ChannelName { get; set; } = "";

        public int Order { get; set; }

        internal void MapToPagePathMappingRuleInfo(PagePathMappingRuleInfo infoObject, int order)
        {
            infoObject.PagePathMappingRuleK13NodeAliasPath = K13NodeAliasPath;
            infoObject.PagePathMappingRuleXbKPagePath = XbKPagePath;
            infoObject.PagePathMappingRuleChannelName = ChannelName;
            infoObject.PagePathMappingRuleOrder = order;
        }
    }
}
