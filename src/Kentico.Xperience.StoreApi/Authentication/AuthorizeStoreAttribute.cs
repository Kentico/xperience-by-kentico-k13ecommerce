using CMS.Membership;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Kentico.Xperience.StoreApi.Authentication;

/// <summary>
/// Custom authorize attribute to handle authorization based on JWT, user is read from claims and set to <see cref="MembershipContext.AuthenticatedUser"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeStoreAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public AuthorizeStoreAttribute() => AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;


    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userRole = context.HttpContext.User;
        var userNameClaim = userRole.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);

        if (userNameClaim is null || userNameClaim.Value == AuthenticationHelper.GlobalPublicUser.UserName)
        {
            // do nothing: user is public
            return;
        }

        var user = await UserInfoProvider.ProviderObject.GetAsync(userNameClaim.Value);
        if (user is null || !user.Enabled)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        MembershipContext.AuthenticatedUser = new CurrentUserInfo(user, keepSourceData: false);
    }
}

