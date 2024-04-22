namespace Kentico.Xperience.K13Ecommerce.Config;

/// <summary>
/// Config for e-commerce integration to K13
/// </summary>
public class KenticoStoreConfig
{
    /// <summary>
    /// URL for live site (or app with Store API)
    /// </summary>
    public required string StoreApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// Store API Client Id
    /// </summary>
    public required string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Store API Client secret
    /// </summary>
    public required string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// When true, product synchronization is enabled
    /// </summary>
    public required bool ProductSyncEnabled { get; set; } = true;

    /// <summary>
    /// Interval in minutes for product synchronization
    /// </summary>
    public required int ProductSyncInterval { get; set; } = 10;
}
