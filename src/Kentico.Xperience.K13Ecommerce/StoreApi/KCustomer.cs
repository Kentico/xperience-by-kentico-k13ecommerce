namespace Kentico.Xperience.K13Ecommerce.StoreApi;

/// <summary>
/// Partial class for customer extension.
/// </summary>
public partial class KCustomer
{
    /// <summary>
    /// Returns true if customer has company information.
    /// </summary>
    public virtual bool CustomerHasCompanyInfo
    {
        get
        {
            if (string.IsNullOrEmpty(CustomerCompany) && string.IsNullOrEmpty(CustomerOrganizationId))
            {
                return !string.IsNullOrEmpty(CustomerTaxRegistrationId);
            }

            return true;
        }
    }
}
