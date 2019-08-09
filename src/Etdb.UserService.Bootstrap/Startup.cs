using Autofac;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Configuration;
using Etdb.UserService.Authentication.Strategies;
using Etdb.UserService.Bootstrap.Extensions;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Repositories;
using Etdb.UserService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ApplicationBuilderExtensions = Etdb.UserService.Bootstrap.Extensions.ApplicationBuilderExtensions;

namespace Etdb.UserService.Bootstrap
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        private const string SwaggerDocDescription = "Etdb " + ServiceNames.UserService + " V1";
        private const string SwaggerDocVersion = "v1";
        private const string SwaggerDocJsonUri = "/swagger/v1/swagger.json";
        private const string CorsPolicyName = "AllowAll";
        private const string AuthenticationSchema = "Bearer";

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var allowedOrigins = this.configuration
                .GetSection(nameof(AllowedOriginsConfiguration))
                .Get<string[]>();


            var identityServerConfiguration =
                this.configuration.GetSection(nameof(IdentityServerConfiguration))
                    .Get<IdentityServerConfiguration>();

            var redisCacheOptions =
                this.configuration.GetSection(nameof(RedisCacheOptions))
                    .Get<RedisCacheOptions>();

            services
                .AddSingleton(new ContextLessRouteProvider())
                .ConfigureCors(this.environment, allowedOrigins, Startup.CorsPolicyName)
                .ConfigureMvc()
                .ConfigureAllowedOriginsOptions(this.configuration)
                .ConfigureIdentityServerAuthorization(identityServerConfiguration, redisCacheOptions, this.environment)
                .ConfigureIdentityServerAuthentication(this.environment, Startup.AuthenticationSchema,
                    ServiceNames.UserService, identityServerConfiguration.Authority)
                .ConfigureAuthorizationPolicies()
                .ConfigureDocumentDbContextOptions(this.configuration)
                .ConfigureIdentityServerConfigurationOptions(this.configuration)
                .ConfigureFileStoreOptions(this.configuration, this.environment)
                .ConfigureCompression()
                .ConfigureHttpClients()
                .ConfigureSwaggerGen(this.environment, new OpenApiInfo
                {
                    Title = Startup.SwaggerDocDescription,
                    Version = Startup.SwaggerDocVersion
                }, Startup.SwaggerDocVersion);

            services.AddHttpClient<IGoogleAuthenticationStrategy, GoogleAuthenticationStrategy>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.SetupSwagger(this.environment, Startup.SwaggerDocJsonUri, Startup.SwaggerDocDescription)
                .SetupHsts(this.environment)
                .SetupForwarding(this.environment)
                .UseResponseCompression()
                .UseCors(Startup.CorsPolicyName)
                .UseIdentityServer()
                .UseMvcAndCaptureRouteData();

            var context = (UserServiceDbContext) app.ApplicationServices.GetRequiredService<DocumentDbContext>();

            ContextScaffold.Scaffold(context);
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetupDependencies(this.environment);
        }
    }
}