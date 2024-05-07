using Duende.AccessTokenManagement;

namespace Kentico.Xperience.K13Ecommerce.Users;

internal class TokenManagementService(IClientCredentialsTokenCache tokenCache) : ITokenManagementService
{
    public async Task ClearTokenCache() =>
        await tokenCache.DeleteAsync(TokenManagementConstants.StoreApiClientName, null!);
}
