using System.Net.Http;
using Autofac;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Authentication.Configuration;
using Etdb.UserService.Authentication.Services;
using Etdb.UserService.Authentication.Validator;
using Etdb.UserService.Controllers.Tests.Extensions;
using Etdb.UserService.Extensions;
using Etdb.UserService.Misc.Configuration;
using IdentityServer4.Contrib.Caching.Redis.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Etdb.UserService.Controllers.Tests.Startups
{
    public class IdentityServerStartup
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly HttpMessageHandler httpMessageHandler;
        private readonly IConfiguration configuration;
        
        private const string AzureServiceBusConnectionString = "AzureServiceBus";

        public IdentityServerStartup(IWebHostEnvironment hostingEnvironment, HttpMessageHandler httpMessageHandler, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.httpMessageHandler = httpMessageHandler;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var identityServerConfiguration = this.configuration
                .GetSection(nameof(IdentityServerConfiguration))
                .Get<IdentityServerConfiguration>();

            var redisCacheOptions = this.configuration
                .GetSection(nameof(RedisCacheOptions))
                .Get<RedisCacheOptions>();
            
            services.Configure<AzureServiceBusConfiguration>(options =>
                options.ConnectionString = this.configuration.GetConnectionString(IdentityServerStartup.AzureServiceBusConnectionString));

            // this.hostingEnvironment.EnvironmentName = Environments.Development;

            services
                .ConfigureApiControllers()
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityResourceConfiguration.GetIdentityResource())
                .AddInMemoryApiResources(ApiResourceConfiguration.GetApiResource())
                .AddInMemoryClients(ClientConfiguration.GetClients(identityServerConfiguration))
                .AddProfileService<ProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordGrantValidator>()
                .AddExtensionGrantValidator<ExternalGrantValidator>()
                .AddDistributedRedisCache(redisCacheOptions.Configuration, redisCacheOptions.InstanceName);

            services.ConfigureAuthorizationPolicies()
                .ConfigureDistributedCaching(this.configuration)
                .ConfigureDocumentDbContextOptions(this.configuration)
                .ConfigureIdentityServerConfigurationOptions(this.configuration)
                .ConfigureFileStoreOptions(this.configuration, this.hostingEnvironment)
                .ConfigureResponseCompression();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer()
                .UseConfiguredRouting();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.SetupDependencies(this.hostingEnvironment);

            builder.RegisterInstance(new ExternalIdentityServerClient(
                    new HttpClient(this.httpMessageHandler)))
                .As<IExternalIdentityServerClient>()
                .SingleInstance();
        }
    }
}