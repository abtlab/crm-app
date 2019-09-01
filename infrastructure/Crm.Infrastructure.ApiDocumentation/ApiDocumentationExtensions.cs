﻿using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Crm.Infrastructure.ApiDocumentation
{
    public static class ApiDocumentationExtensions
    {
        private const string DefaultApiVersion = "v1";

        public static IServiceCollection ConfigureApiDocumentation(this IServiceCollection services,
            string apiVersion = DefaultApiVersion)
        {
            var info = new OpenApiInfo
            {
                Title = Assembly.GetCallingAssembly().GetName().Name,
                Version = apiVersion
            };

            return services.AddSwaggerGen(x => x.SwaggerDoc(apiVersion, info));
        }

        public static IApplicationBuilder UseApiDocumentationsMiddleware(this IApplicationBuilder applicationBuilder,
            string apiVersion = DefaultApiVersion)
        {
            var applicationName = Assembly.GetCallingAssembly().GetName().Name;

            var url = $"/swagger/{apiVersion}/swagger.json";

            return applicationBuilder.UseSwagger().UseSwaggerUI(x => x.SwaggerEndpoint(url, applicationName));
        }
    }
}