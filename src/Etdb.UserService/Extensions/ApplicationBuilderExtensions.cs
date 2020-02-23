using Etdb.UserService.Autofac.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;

namespace Etdb.UserService.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app,
            string jsonUri,
            string description)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(action => action.SwaggerEndpoint(jsonUri, description));
        }

        public static IApplicationBuilder UseConfiguredHsts(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            return environment.IsAnyDevelopment()
                ? app
                : app.UseHsts()
                    .UseHttpsRedirection();
        }

        public static IApplicationBuilder UseConfigureForwarding(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            };

            // TODO: check if this is really required
            // https://stackoverflow.com/questions/51143761/asp-net-core-docker-https-on-azure-app-service-containers
            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            return app.UseForwardedHeaders(forwardOptions);
        }

        public static void UseConfiguredRouting(this IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}