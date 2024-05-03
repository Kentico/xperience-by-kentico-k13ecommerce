using CMS;
using CMS.Core;
using CMS.DataEngine;
using CMS.Membership;

using Kentico.Xperience.K13Ecommerce.Users.UserSynchronization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: RegisterModule(typeof(UserSynchronizationModule))]
namespace Kentico.Xperience.K13Ecommerce.Users.UserSynchronization;

internal class UserSynchronizationModule() : Module(nameof(UserSynchronizationModule))
{
    protected override void OnInit()
    {
        base.OnInit();

        //CMS.Membership.Me
        MemberInfo.TYPEINFO.Events.Insert.After += SynchronizeUser;
    }


    private void SynchronizeUser(object? sender, ObjectEventArgs e)
    {
        try
        {
            var user = (MemberInfo)e.Object;
            if (user != null)
            {
                using var serviceScope = Service.Resolve<IServiceProvider>().CreateScope();
                var provider = serviceScope.ServiceProvider;
                var userSynchronizationService = provider.GetRequiredService<IUserSynchronizationService>();
                userSynchronizationService.SynchronizeUser(user).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            var logger = Service.Resolve<ILogger<UserSynchronizationModule>>();
            logger.LogError(ex, "Error occured during user synchronization");
        }
    }
}
