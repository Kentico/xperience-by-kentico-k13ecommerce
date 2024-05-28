using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Customers;

/// <summary>
/// Customer service.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Get list of addresses for current customer.
    /// </summary>    
    Task<ICollection<KAddress>> GetCurrentCustomerAddresses();


    /// <summary>
    /// Get list of addresses for customer with specified ID to display in XByK administration.
    /// </summary>
    /// <param name="customerId">Customer ID.</param>    
    Task<ICollection<KAddress>> GetAdminCustomerAddresses(int customerId);
}
