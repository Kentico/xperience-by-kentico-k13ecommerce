using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kentico.Xperience.StoreApi.Authentication;

public static class StoreApiAuthenticationExtensions
{
    public static void AddKenticoStoreApiJwtAuth(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("CMSStoreApi:Jwt").Get<JwtOptions>();
        builder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            };
        });
    }
}
