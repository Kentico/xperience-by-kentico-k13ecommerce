using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.Authentication;

/// <summary>
/// Token request.
/// </summary>
public class TokenRequest
{
    [RegularExpression("client_credentials")]
    [FromForm(Name = "grant_type")]
    [DefaultValue("client_credentials")]
    public string GrantType { get; set; }

    [Required]
    [FromForm(Name = "client_id")]
    [MinLength(16)]
    public string ClientId { get; set; }

    [Required]
    [FromForm(Name = "client_secret")]
    [MinLength(16)]
    public string ClientSecret { get; set; }

    [FromForm(Name = "userEmail")]
    public string UserEmail { get; set; }
}
