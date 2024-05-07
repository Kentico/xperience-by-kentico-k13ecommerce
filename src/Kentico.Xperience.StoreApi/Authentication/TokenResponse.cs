using System.Text.Json.Serialization;

namespace Kentico.Xperience.StoreApi.Authentication;

/// <summary>
/// Token response.
/// </summary>
public class TokenResponse
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
