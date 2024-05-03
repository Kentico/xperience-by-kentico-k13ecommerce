using System.Reflection;

using Kentico.Xperience.StoreApi.Authentication;
using Kentico.Xperience.StoreApi.Products;
using Kentico.Xperience.StoreApi.Products.Pages;
using Kentico.Xperience.StoreApi.Products.SKU;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kentico.Xperience.StoreApi;

public static class StoreApiServiceCollectionExtensions
{
    /// <summary>
    /// Registers store API services and options.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <returns></returns>
    public static IServiceCollection AddKenticoStoreApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IKProductService, KProductService>();
        services.AddScoped<IProductPageConverter<KProductNode>, ProductPageConverter<KProductNode>>();
        services.AddScoped<IProductSKUConverter<KProductSKU>, ProductSKUConverter<KProductSKU>>();
        services.AddOptions<JwtOptions>().Bind(configuration.GetSection("CMSStoreApi:Jwt"));
        services.AddOptions<StoreApiOptions>().Bind(configuration.GetSection("CMSStoreApi"));
        services.AddAutoMapper(typeof(StoreApiServiceCollectionExtensions).Assembly);
        return services;
    }


    /// <summary>
    /// Adds Store API Swagger.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns></returns>
    public static IServiceCollection AddKenticoStoreApiSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("storeapi", new OpenApiInfo { Title = "Kentico Xperience 13 Store API", Version = "v1" });
            c.DocumentFilter<StoreApiDocumentFilter>();
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            // prevent decimal encode as double in api schema
            c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}

/// <summary>
/// Filter endpoints that are not related to the store API.
/// </summary>
internal class StoreApiDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var pathsToRemove = swaggerDoc.Paths
            .Where(pathItem => !pathItem.Key.Contains("api/store"))
            .ToList();

        foreach (var item in pathsToRemove)
        {
            swaggerDoc.Paths.Remove(item.Key);
        }
    }
}
