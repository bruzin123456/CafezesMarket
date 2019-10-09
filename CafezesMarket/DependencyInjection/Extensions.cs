using CafezesMarket.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CafezesMarket.DependencyInjection
{
    public static class Extensions
    {
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
    }
}
