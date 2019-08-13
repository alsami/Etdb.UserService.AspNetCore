using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;

namespace Etdb.UserService.Bootstrap.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SetupSwagger(this IApplicationBuilder app, IWebHostEnvironment environment,
            string jsonUri,
            string description)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(action => action.SwaggerEndpoint(jsonUri, description));
        }

        public static IApplicationBuilder SetupHsts(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            return environment.IsAnyLocalDevelopment() ? app : app.UseHsts();
        }

        public static IApplicationBuilder SetupForwarding(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                return app;
            }

            return app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }

        public static void UseConfiguredRouting(this IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}