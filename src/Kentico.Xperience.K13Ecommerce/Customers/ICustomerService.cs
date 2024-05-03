using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Customers;

public interface ICustomerService
{
    /// <summary>
    /// Get list of addresses for given customer.
    /// </summary>
    /// <param name="customerId">Customer ID.</param>
    /// <returns></returns>
    Task<ICollection<KAddress>> GetCustomerAddresses(int customerId);
}
