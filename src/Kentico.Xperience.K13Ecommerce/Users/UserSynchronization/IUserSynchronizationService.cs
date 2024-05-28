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

    /// <summary>
    /// Checks if user exists in K13.
    /// </summary>
    /// <param name="userName">User name</param>
    /// <returns>True if user exists</returns>
    Task<bool> UserExists(string userName);
}
