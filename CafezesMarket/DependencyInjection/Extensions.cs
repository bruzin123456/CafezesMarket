using CafezesMarket.Infrastructure.Database.Context;
using CafezesMarket.Services;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
            services.AddMemoryCache();

            services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(configure =>
                {
                    configure.LoginPath = new PathString("/Login/SignIn");
                    configure.LogoutPath = new PathString("/Login/SignOut");

                    configure.SlidingExpiration = true;
                    configure.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });

            services.AddControllers()
                .AddJsonOptions(configure =>
                {
                    configure.JsonSerializerOptions.WriteIndented = true;
                });

            return services;
        }

        public static IServiceCollection AddContexts(this IServiceCollection services)
        {
            services.AddDbContextPool<DefaultContext>((provider, options) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var connStr = config.GetConnectionString("azureDB");

                if (string.IsNullOrWhiteSpace(connStr))
                {
                    throw new ArgumentNullException(nameof(connStr));
                }

                options.UseSqlServer(connStr, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure());
            }, poolSize: 4);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<IProdutoService, ProdutoService>();
            services.AddTransient<ICredencialService, CredencialService>();
            services.AddTransient<ICarrinhoService, CarrinhoService>();
            services.AddTransient<IRelatorioService, RelatorioService>();

            return services;
        }
    }
}
