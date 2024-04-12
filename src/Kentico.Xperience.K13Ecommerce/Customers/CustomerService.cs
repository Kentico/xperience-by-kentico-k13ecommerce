using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Customers;

internal class CustomerService(IKenticoStoreApiClient apiClient) : ICustomerService
{
    public async Task<ICollection<KAddress>> GetCustomerAddresses(int customerId) =>
        await apiClient.CustomerAddressesAsync(customerId);
}
