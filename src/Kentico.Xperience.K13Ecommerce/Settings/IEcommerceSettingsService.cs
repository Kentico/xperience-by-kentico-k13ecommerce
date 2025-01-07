using CMS.Integration.K13Ecommerce;

namespace Kentico.Xperience.K13Ecommerce.Settings
{
    /// <summary>
    /// Ecommerce settings service.
    /// </summary>
    public interface IEcommerceSettingsService
    {
        /// <summary>
        /// Get Settings of ecommerce from Kentico.
        /// </summary>
        /// <returns></returns>
        Task<K13EcommerceSettingsInfo?> GetEcommerceSettings();
    }
}
