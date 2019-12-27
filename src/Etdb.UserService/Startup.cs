using Autofac;
using Etdb.ServiceBase.Constants;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Configuration;
using Etdb.UserService.Authentication.Strategies;
using Etdb.UserService.Extensions;
using Etdb.UserService.Misc.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Etdb.UserService
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
        private const string AzureServiceBusConnectionString = "AzureServiceBus";

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configEnvironment = this.configuration["ASPNETCORE:Environment"];

            this.environment.EnvironmentName = string.IsNullOrWhiteSpace(configEnvironment)
                ? this.environment.EnvironmentName
                : configEnvironment;

            Log.Logger.Information($"Environment:\n{this.environment.EnvironmentName}");

            Log.Logger.Information($"Config:\n{(this.configuration as IConfigurationRoot)!.GetDebugView()}");

            if (this.environment.IsClientGen())
            {
                services.ConfigureApiControllers()
                    .ConfigureSwaggerGen(new OpenApiInfo
                    {
                        Title = Startup.SwaggerDocDescription,
                        Version = Startup.SwaggerDocVersion
                    }, Startup.SwaggerDocVersion);

                return;
            }

            var allowedOrigins = this.configuration
                .GetSection(nameof(AllowedOriginsConfiguration))
                .Get<string[]>();

            var identityServerConfiguration =
                this.configuration.GetSection(nameof(IdentityServerConfiguration))
                    .Get<IdentityServerConfiguration>();

            var redisCacheOptions =
                this.configuration.GetSection(nameof(RedisCacheOptions))
                    .Get<RedisCacheOptions>();

            services.Configure<AzureServiceBusConfiguration>(options =>
                options.ConnectionString = this.configuration.GetConnectionString(Startup.AzureServiceBusConnectionString));

            services
                .ConfigureCors(this.environment, allowedOrigins, Startup.CorsPolicyName)
                .ConfigureApiControllers()
                .ConfigureAllowedOriginsOptions(this.configuration)
                .ConfigureIdentityServerAuthorization(identityServerConfiguration, redisCacheOptions, this.environment)
                .ConfigureIdentityServerAuthentication(this.environment, Startup.AuthenticationSchema,
                    ServiceNames.UserService, identityServerConfiguration.Authority)
                .ConfigureAuthorizationPolicies()
                .ConfigureDocumentDbContextOptions(this.configuration)
                .ConfigureIdentityServerConfigurationOptions(this.configuration)
                .ConfigureFileStoreOptions(this.configuration, this.environment)
                .ConfigureResponseCompression()
                .ConfigureHttpClients();

            services.AddHttpClient<IGoogleAuthenticationStrategy, GoogleAuthenticationStrategy>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (this.environment.IsClientGen())
            {
                app.SetupSwagger(Startup.SwaggerDocJsonUri, Startup.SwaggerDocDescription)
                    .UseConfiguredRouting();

                return;
            }

            app.SetupHsts(this.environment)
                .SetupForwarding(this.environment)
                .UseResponseCompression()
                .UseCors(Startup.CorsPolicyName)
                .UseIdentityServer()
                .UseConfiguredRouting();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            if (this.environment.IsClientGen()) return;

            containerBuilder.SetupDependencies(this.environment);
        }
    }
}