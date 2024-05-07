using System.Net.Mime;

using Kentico.Membership;
using Kentico.Xperience.StoreApi.Authentication;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.StoreApi.Sychronization;

[ApiController]
[AuthorizeStore]
[Route("api/store/synchronization")]
public class UserSynchronizationController : ControllerBase
{
    private readonly ApplicationUserManager<ApplicationUser> userManager;


    public UserSynchronizationController(ApplicationUserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }


    /// <summary>
    /// Creates new user.
    /// </summary>
    /// <param name="user">User to create.</param>
    [HttpPost("user-synchronization")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SynchronizeUser([FromBody] KUserSynchronization user)
    {
        var existingUser = await userManager.FindByNameAsync(user.UserName);
        if (existingUser is not null)
        {
            return Conflict();
        }

        var newUser = new ApplicationUser
        {
            UserName = user.UserName,
            Email = user.UserName,
            Enabled = true
        };

        IdentityResult registerResult;

        try
        {
            registerResult = await userManager.CreateAsync(newUser, Guid.NewGuid().ToString());
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return ValidationProblem();
        }

        if (registerResult.Succeeded)
        {
            return Ok();
        }

        foreach (var error in registerResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return ValidationProblem();
    }
}
