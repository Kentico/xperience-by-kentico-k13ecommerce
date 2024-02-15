using Kentico.Xperience.StoreApi.Products;
using Kentico.Xperience.StoreApi.Products.Pages;
using Kentico.Xperience.StoreApi.Products.SKU;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Kentico.Xperience.StoreApi;

public static class StoreApiServiceCollectionExtensions
{
    public static IServiceCollection AddKenticoStoreApi(this IServiceCollection services)
    {
        services.AddScoped<IKProductService, KProductService>();
        services.AddScoped<IProductPageConverter<KProductNode>, ProductPageConverter<KProductNode>>();
        services.AddScoped<IProductSKUConverter<KProductSKU>, ProductSKUConverter<KProductSKU>>();
        return services;
    }

    public static IServiceCollection AddKenticoStoreApiSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("storeapi", new OpenApiInfo { Title = "Kentico Xperience 13 Store API", Version = "v1" });
            c.DocumentFilter<StoreApiDocumentFilter>();
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            // prevent decimal encode as double in api schema
            c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
        });

        return services;
    }
}

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