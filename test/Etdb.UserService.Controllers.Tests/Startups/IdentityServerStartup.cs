using System;
using System.IO;
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
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Controllers.Tests.Startups
{
    public class IdentityServerStartup
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly HttpMessageHandler httpMessageHandler;

        public IdentityServerStartup(IWebHostEnvironment hostingEnvironment, HttpMessageHandler httpMessageHandler)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.httpMessageHandler = httpMessageHandler;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json"))
                .AddEnvironmentVariables()
                .AddUserSecrets("Etdb_UserService")
                .Build();

            var identityServerConfiguration = configuration
                .GetSection(nameof(IdentityServerConfiguration))
                .Get<IdentityServerConfiguration>();

            var redisCacheOptions = configuration
                .GetSection(nameof(RedisCacheOptions))
                .Get<RedisCacheOptions>();

            this.hostingEnvironment.EnvironmentName = Environments.Development;

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
                .ConfigureDistributedCaching(configuration)
                .ConfigureDocumentDbContextOptions(configuration)
                .ConfigureIdentityServerConfigurationOptions(configuration)
                .ConfigureFileStoreOptions(configuration, this.hostingEnvironment)
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