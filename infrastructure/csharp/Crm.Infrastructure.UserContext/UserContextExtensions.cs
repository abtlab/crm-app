﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.UserContext
{
    public static class UserContextExtensions
    {
        public static IServiceCollection ConfigureUserContext<TUserContext, TUserContextImplementation>(
            this IServiceCollection services)
            where TUserContext : class 
            where TUserContextImplementation : class, TUserContext
        {
            return services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<TUserContext, TUserContextImplementation>();
        }
    }
}