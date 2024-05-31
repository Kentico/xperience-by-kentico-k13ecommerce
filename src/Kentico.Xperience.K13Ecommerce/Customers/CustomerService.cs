using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Customers;

internal class CustomerService(IKenticoStoreApiClient apiClient) : ICustomerService
{
    /// <inheritdoc/>
    public async Task<ICollection<KAddress>> GetCurrentCustomerAddresses() =>
        await apiClient.CurrentCustomerAddressesAsync();


    /// <inheritdoc/>
    public async Task<ICollection<KAddress>> GetAdminCustomerAddresses(int customerId) =>
        await apiClient.AdminCustomerAddressesAsync(customerId);
}
