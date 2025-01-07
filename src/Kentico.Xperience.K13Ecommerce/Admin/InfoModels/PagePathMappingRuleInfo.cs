namespace CMS.Integration.K13Ecommerce
{
    public partial class PagePathMappingRuleInfo
    {
        static PagePathMappingRuleInfo()
        {
            TYPEINFO.ContinuousIntegrationSettings.Enabled = true;
            TYPEINFO.OrderColumn = nameof(PagePathMappingRuleOrder);
        }
    }
}
