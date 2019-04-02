using System;
using System.IO;
using System.Net.Http;
using Autofac;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Authentication.Configuration;
using Etdb.UserService.Authentication.Services;
using Etdb.UserService.Bootstrap.Extensions;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Etdb.UserService.Bootstrap.Tests.Startups
{
    public class IdentityServerStartup
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly HttpMessageHandler httpMessageHandler;

        public IdentityServerStartup(IHostingEnvironment hostingEnvironment, HttpMessageHandler httpMessageHandler)
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
            
            this.hostingEnvironment.EnvironmentName = EnvironmentName.Development;

            services
                .AddSingleton(new ContextLessRouteProvider())
                .ConfigureMvc()
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityResourceConfiguration.GetIdentityResource())
                .AddInMemoryApiResources(ApiResourceConfiguration.GetApiResource())
                .AddInMemoryClients(ClientConfiguration.GetClients(identityServerConfiguration));

            services.ConfigureAuthorizationPolicies()
                .ConfigureDistributedCaching(configuration)
                .ConfigureDocumentDbContextOptions(configuration)
                .ConfigureIdentityServerConfigurationOptions(configuration)
                .ConfigureFileStoreOptions(configuration, this.hostingEnvironment)
                .ConfigureCompression();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer()
                .SetupMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.SetupDependencies();

            builder.RegisterInstance(new ExternalIdentityServerClient(
                    new HttpClient(this.httpMessageHandler)))
                .As<IExternalIdentityServerClient>()
                .SingleInstance();
        }
    }
}