namespace Kentico.Xperience.K13Ecommerce.Config;

/// <summary>
/// Config for e-commerce integration to K13
/// </summary>
public class KenticoStoreConfig
{
    public required string StoreApiUrl { get; set; } = string.Empty;
    public required string ClientId { get; set; } = string.Empty;
    public required string ClientSecret { get; set; } = string.Empty;
}
