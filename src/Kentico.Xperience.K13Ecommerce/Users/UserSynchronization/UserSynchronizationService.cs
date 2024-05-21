using CMS.Membership;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Users.UserSynchronization;

internal class UserSynchronizationService(IKenticoStoreApiClient storeApiClient) : IUserSynchronizationService
{
    private IKenticoStoreApiClient StoreApiClient { get; } = storeApiClient;


    /// <inheritdoc/>
    public async Task SynchronizeUser(MemberInfo user) => await StoreApiClient.UserSynchronizationAsync(new KUserSynchronization()
    {
        UserName = user.MemberName,
        Email = user.MemberEmail
    });
}
