using Etdb.UserService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Etdb.UserService.Bootstrap.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SetupSwagger(this IApplicationBuilder app, IHostingEnvironment environment,
            string jsonUri,
            string description)
        {
            if (!environment.IsDevelopment() && !environment.IsLocalDevelopment())
            {
                return app;
            }

            return app
                .UseSwagger()
                .UseSwaggerUI(action => action.SwaggerEndpoint(jsonUri, description));
        }

        public static IApplicationBuilder SetupHsts(this IApplicationBuilder app, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment() || environment.IsLocalDevelopment())
            {
                return app;
            }

            return app.UseHsts();
        }

        public static IApplicationBuilder SetupForwarding(this IApplicationBuilder app, IHostingEnvironment environment)
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

        public static IApplicationBuilder SetupMvc(this IApplicationBuilder app)
        {
            //// very hacky shit
            //// we need to access the route data outside of mvc-requests
            //// in order to be able to build the profile-image url using the url helper provided by mvc
            //// therefore I must store the routes manually
            //// see for the reason https://github.com/aspnet/Mvc/issues/5164
            var contextLessRouteProvider = app.ApplicationServices.GetRequiredService<ContextLessRouteProvider>();

            IRouteBuilder routeBuilder = null;
            app.UseMvc(builder => routeBuilder = builder);

            contextLessRouteProvider.Router = routeBuilder.Build();

            return app;
            // TODO use new logic
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-2.2
            // https://stackoverflow.com/questions/46096068/asp-net-core-2-0-creating-urlhelper-without-request
            //var routes = new RouteBuilder(app).Build();
            //app.UseRouter(routes);
        }
    }
}