using System.ComponentModel.DataAnnotations;

namespace Kentico.Xperience.StoreApi.Sychronization;

/// <summary>
/// Represents model for synchronizing user.
/// </summary>
public class KUserSynchronization
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; }

    [MaxLength(254)]
    [EmailAddress]
    public string Email { get; set; }
}
