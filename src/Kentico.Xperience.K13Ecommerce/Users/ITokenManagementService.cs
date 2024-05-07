namespace Kentico.Xperience.K13Ecommerce.Users;

/// <summary>
/// Token management service.
/// </summary>
public interface ITokenManagementService
{
    /// <summary>
    /// Deletes current token from cache.
    /// </summary>
    /// <returns></returns>
    Task ClearTokenCache();
}
