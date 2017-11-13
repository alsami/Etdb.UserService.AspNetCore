using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.AccessTokenValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Builder;

namespace EntertainmentDatabase.REST.API.ServiceBase.Extensions.DependenyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services, string name, string title, string version)
        {
            services.AddMvcCore()
                .AddApiExplorer();

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name, new Info
                {
                    Title = title,
                    Version = version
                });
            });
        }

        public static IServiceCollection AddDbContextForSqlServer<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            return services.AddDbContext<TDbContext>()
                .AddEntityFrameworkSqlServer();
        }

        public static IServiceCollection AddBearerAuthenticationForIdentityServer(this IServiceCollection services, Action<IdentityServerAuthenticationOptions> options)
        {
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options);

            return services;
        }
    }
}
