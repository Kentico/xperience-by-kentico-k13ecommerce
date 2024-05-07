using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

using CMS.Membership;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kentico.Xperience.StoreApi.Authentication;

[ApiController]
[Route("api/store/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly StoreApiOptions apiOptions;
    private readonly IUserInfoProvider userInfoProvider;
    private readonly JwtOptions jwtOptions;


    public AuthenticationController(IOptions<JwtOptions> jwtOptions, IOptions<StoreApiOptions> apiOptions, IUserInfoProvider userInfoProvider)
    {
        this.apiOptions = apiOptions.Value;
        this.userInfoProvider = userInfoProvider;
        this.jwtOptions = jwtOptions.Value;
    }


    /// <summary>
    /// Endpoint for getting JWT access token.
    /// </summary>
    /// <param name="tokenRequest">Request with client credentials.</param>
    /// <returns></returns>
    [HttpPost("token")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("multipart/form-data", "application/x-www-form-urlencoded")]
    public async Task<ActionResult<TokenResponse>> AccessToken([FromForm] TokenRequest tokenRequest)
    {
        if (tokenRequest.ClientId != apiOptions.ClientId || tokenRequest.ClientSecret != apiOptions.ClientSecret)
        {
            return Unauthorized();
        }

        var user = string.IsNullOrEmpty(tokenRequest.UserName) || tokenRequest.UserName == AuthenticationHelper.GlobalPublicUser.UserName
            ? AuthenticationHelper.GlobalPublicUser
            : await userInfoProvider.GetAsync(tokenRequest.UserName);

        if (user is null)
        {
            return BadRequest("Invalid user name");
        }

        string issuer = jwtOptions.Issuer;
        byte[] key = Encoding.ASCII.GetBytes(jwtOptions.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()), new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.TokenExpiresIn),
            Issuer = issuer,
            Audience = jwtOptions.Audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        string jwtToken = tokenHandler.WriteToken(token);

        return Ok(new TokenResponse
        {
            AccessToken = jwtToken,
            ExpiresIn = (int)TimeSpan.FromMinutes(jwtOptions.TokenExpiresIn).TotalSeconds
        });
    }
}
