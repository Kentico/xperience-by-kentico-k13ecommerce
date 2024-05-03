using CMS.Membership;

namespace Kentico.Xperience.K13Ecommerce.Users.UserSynchronization;

public interface IUserSynchronizationService
{
    /// <summary>
    /// Synchronize XbyK member to K13.
    /// </summary>
    Task SynchronizeUser(MemberInfo user);
}
