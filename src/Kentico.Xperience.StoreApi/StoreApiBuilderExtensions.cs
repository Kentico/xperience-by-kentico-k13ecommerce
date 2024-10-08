﻿using Microsoft.AspNetCore.Builder;

namespace Kentico.Xperience.StoreApi;

public static class StoreApiBuilderExtensions
{
    /// <summary>
    /// Store API Swagger.
    /// </summary>
    /// <param name="app">App builder.</param>
    public static IApplicationBuilder UseStoreApiSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("storeapi/swagger.json", "Kentico Xperience 13 Store API");
        });
        return app;
    }
}
