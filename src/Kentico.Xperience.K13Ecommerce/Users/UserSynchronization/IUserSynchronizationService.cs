using CMS.Membership;

namespace Kentico.Xperience.K13Ecommerce.Users.UserSynchronization;

/// <summary>
/// User synchronization service.
/// </summary>
public interface IUserSynchronizationService
{
    /// <summary>
    /// Synchronize XbyK member to K13.
    /// </summary>
    Task SynchronizeUser(MemberInfo user);
}
