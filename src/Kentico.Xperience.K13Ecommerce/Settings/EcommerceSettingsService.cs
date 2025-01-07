using CMS.DataEngine;
using CMS.Integration.K13Ecommerce;

namespace Kentico.Xperience.K13Ecommerce.Settings
{
    internal class EcommerceSettingsService(IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider)
        : IEcommerceSettingsService
    {
        public async Task<K13EcommerceSettingsInfo?> GetEcommerceSettings()
        {
            var settings = (await k13EcommerceSettingsInfoProvider.Get()
            .TopN(1)
            .GetEnumerableTypedResultAsync())
            .FirstOrDefault();

            return settings;
        }
    }
}
