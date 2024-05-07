namespace Kentico.Xperience.StoreApi.Authentication;

/// <summary>
/// Json web token (JWT) config options.
/// </summary>
public class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int TokenExpiresIn { get; set; } = 60;
}
