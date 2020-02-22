using Etdb.UserService.Autofac.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SetupSwagger(this IApplicationBuilder app,
            string jsonUri,
            string description)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(action => action.SwaggerEndpoint(jsonUri, description));
        }

        public static IApplicationBuilder SetupHsts(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            return environment.IsAnyDevelopment() 
                ? app
                : app.UseHsts()
                    .UseHttpsRedirection();
        }

        public static IApplicationBuilder SetupForwarding(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            };
            
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