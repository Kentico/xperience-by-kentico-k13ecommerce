using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.Authentication;

public class TokenRequest
{
    [RegularExpression("client_credentials")]
    [FromForm(Name = "grant_type")]
    [DefaultValue("client_credentials")]
    public string GrantType { get; set; }

    [Required]
    [FromForm(Name = "client_id")]
    public string ClientId { get; set; }

    [Required]
    [FromForm(Name = "client_secret")]
    public string ClientSecret { get; set; }

    [FromForm(Name = "username")]
    public string UserName { get; set; }
}
