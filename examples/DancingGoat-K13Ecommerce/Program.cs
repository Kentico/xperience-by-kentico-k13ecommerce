using System.Text.Json;

using DancingGoat;
using DancingGoat.HealthChecks;
using DancingGoat.Helpers;
using DancingGoat.Models;

using Kentico.Activities.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Membership;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Kentico.Xperience.K13Ecommerce;
using Kentico.Xperience.K13Ecommerce.ShoppingCart;
using Kentico.Xperience.K13Ecommerce.Users;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddKentico(features =>
{
    features.UsePageBuilder(new PageBuilderOptions
    {
        DefaultSectionIdentifier = ComponentIdentifiers.SINGLE_COLUMN_SECTION,
        RegisterDefaultSection = false,
        ContentTypeNames = new[]
        {
            LandingPage.CONTENT_TYPE_NAME,
            ContactsPage.CONTENT_TYPE_NAME,
            ArticlePage.CONTENT_TYPE_NAME
        }
    });

    features.UseWebPageRouting();
    features.UseEmailMarketing();
    features.UseEmailStatisticsLogging();
    features.UseActivityTracking();
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddLocalization()
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
    });

// K13 Store helath check
builder.Services.AddHttpClient(
    nameof(K13StoreApiHealthCheck),
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetSection("CMSKenticoStoreConfig:StoreApiUrl").Value);
        client.Timeout = TimeSpan.FromSeconds(5);
    }
);
builder.Services.AddHealthChecks()
    .AddCheck<K13StoreApiHealthCheck>("k13store_health_check");

builder.Services.AddSession();

builder.Services.AddDancingGoatServices();


ConfigureMembershipServices(builder.Services);

//Kentico Store API
builder.Services.AddKenticoStoreServices(builder.Configuration);


var app = builder.Build();

app.InitKentico();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseSession();

app.UseKentico();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.Kentico().MapRoutes();

app.MapControllerRoute(
   name: "error",
   pattern: "error/{code}",
   defaults: new { controller = "HttpErrors", action = "Error" }
);

app.MapHealthChecks("/status", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        // Set response content type to JSON
        context.Response.ContentType = "application/json";

        // Create a JSON object with detailed health check information
        string result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                exception = FormatExceptionHelper.Format(entry.Value.Exception),
                data = entry.Value.Data
            })
        });

        await context.Response.WriteAsync(result);
    }
});

app.MapControllerRoute(
    name: DancingGoatConstants.DEFAULT_ROUTE_NAME,
    pattern: $"{{{WebPageRoutingOptions.LANGUAGE_ROUTE_VALUE_KEY}}}/{{controller}}/{{action}}",
    constraints: new
    {
        controller = DancingGoatConstants.CONSTRAINT_FOR_NON_ROUTER_PAGE_CONTROLLERS
    }
);

app.MapControllerRoute(
    name: DancingGoatConstants.DEFAULT_ROUTE_WITHOUT_LANGUAGE_PREFIX_NAME,
    pattern: "{controller}/{action}",
    constraints: new
    {
        controller = DancingGoatConstants.CONSTRAINT_FOR_NON_ROUTER_PAGE_CONTROLLERS
    }
);

LogConfiguration(builder.Configuration);
app.Run();


static void ConfigureMembershipServices(IServiceCollection services)
{
    services.AddIdentity<ApplicationUser, NoOpApplicationRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredUniqueChars = 0;
        // Ensures, that disabled member cannot sign in.
        options.SignIn.RequireConfirmedAccount = true;
    })
        .AddUserStore<ApplicationUserStore<ApplicationUser>>()
        .AddRoleStore<NoOpApplicationRoleStore>()
        .AddUserManager<UserManager<ApplicationUser>>()
        .AddSignInManager<SignInManager<ApplicationUser>>();

    services.ConfigureApplicationCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = new PathString("/account/login");
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            var factory = ctx.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = factory.GetUrlHelper(new ActionContext(ctx.HttpContext, new RouteData(ctx.HttpContext.Request.RouteValues), new ActionDescriptor()));
            var url = urlHelper.Action("Login", "Account") + new Uri(ctx.RedirectUri).Query;

            ctx.Response.Redirect(url);

            return Task.CompletedTask;
        };

        //custom: clear token from cache when user sign in/sign out
        options.Events.OnSignedIn = async context =>
        {
            var tokenManagementService = context.HttpContext.RequestServices.GetRequiredService<ITokenManagementService>();
            await tokenManagementService.ClearTokenCache();
            var shoppingService = context.HttpContext.RequestServices.GetRequiredService<IShoppingService>();
            shoppingService.ClearCaches();
        };
        options.Events.OnSigningOut = async context =>
        {
            var tokenManagementService = context.HttpContext.RequestServices.GetRequiredService<ITokenManagementService>();
            await tokenManagementService.ClearTokenCache();
            var shoppingService = context.HttpContext.RequestServices.GetRequiredService<IShoppingService>();
            shoppingService.ClearCaches();
        };
    });

    services.AddAuthorization();
}

static void LogConfiguration(IConfiguration config)
{
    Console.WriteLine("Application Settings:");

    foreach (var kvp in config.AsEnumerable())
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
}
