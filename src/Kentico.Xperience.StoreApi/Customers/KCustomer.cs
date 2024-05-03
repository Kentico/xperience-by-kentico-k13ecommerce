using System.ComponentModel.DataAnnotations;

namespace Kentico.Xperience.StoreApi.Customers;

/// <summary>
/// Dto for <see cref="CMS.Ecommerce.CustomerInfo"/>.
/// </summary>
public class KCustomer
{
    public int CustomerId { get; set; }

    [Required]
    public string CustomerFirstName { get; set; }

    [Required]
    public string CustomerLastName { get; set; }

    public string CustomerCompany { get; set; }

    public string CustomerEmail { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerFax { get; set; }

    public int CustomerUserId { get; set; }

    public string CustomerOrganizationId { get; set; }

    public string CustomerTaxRegistrationId { get; set; }

    public bool CustomerIsRegistered => CustomerUserId > 0;
}
