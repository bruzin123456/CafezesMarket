using CafezesMarket.Infrastructure.Database.Context;
using CafezesMarket.Services;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CafezesMarket.DependencyInjection
{
    public static class Extensions
    {
        public static IServiceCollection AddCommons(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(configure =>
                {
                    configure.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    configure.SlidingExpiration = true;
                });

            return services;
        }

        public static IServiceCollection AddContexts(this IServiceCollection services)
        {
            services.AddDbContextPool<DefaultContext>((provider, options) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var connStr = config.GetConnectionString("database");

                if (string.IsNullOrWhiteSpace(connStr))
                {
                    throw new ArgumentNullException(nameof(connStr));
                }

                options.UseSqlServer(connStr, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure());
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IProdutoService, ProdutoService>();
            services.AddTransient<ICredencialService, CredencialService>();

            return services;
        }
    }
}
