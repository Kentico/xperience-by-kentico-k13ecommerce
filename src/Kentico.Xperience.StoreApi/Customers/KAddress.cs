using System.ComponentModel.DataAnnotations;

namespace Kentico.Xperience.StoreApi.Customers;

/// <summary>
/// Dto for <see cref="CMS.Ecommerce.AddressInfo"/>.
/// </summary>
public class KAddress
{
    public int AddressId { get; set; }

    public int AddressCustomerId { get; set; }

    [Required]
    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    [Required]
    public string AddressCity { get; set; }

    [Required]
    public string AddressZip { get; set; }

    public string AddressPhone { get; set; }

    public string AddressName { get; set; }

    public string AddressPersonalName { get; set; }

    [Required]
    public int AddressCountryId { get; set; }

    public int AddressStateId { get; set; }
}
