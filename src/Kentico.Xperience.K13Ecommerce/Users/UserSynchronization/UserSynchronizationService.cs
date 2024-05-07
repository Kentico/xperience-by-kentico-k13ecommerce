using CMS.Membership;

using Kentico.Xperience.K13Ecommerce.StoreApi;

namespace Kentico.Xperience.K13Ecommerce.Users.UserSynchronization;

internal class UserSynchronizationService : IUserSynchronizationService
{
    private IKenticoStoreApiClient StoreApiClient { get; }


    public UserSynchronizationService(IKenticoStoreApiClient storeApiClient)
    {
        StoreApiClient = storeApiClient;
    }


    /// <inheritdoc/>
    public async Task SynchronizeUser(MemberInfo user)
    {
        await StoreApiClient.UserSynchronizationAsync(new KUserSynchronization()
        {
            UserName = user.MemberEmail
        });
    }
}
