using System;
using System.IO;
using System.Net.Http;
using Autofac;
using Etdb.ServiceBase.Constants;
using Etdb.UserService.Bootstrap.Extensions;
using Etdb.UserService.Bootstrap.Tests.Extensions;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace Etdb.UserService.Bootstrap.Tests.Startups
{
    public class ApiServerStartup
    {
        private readonly IHostingEnvironment hostingEnvironment;

        private const string AuthenticationSchema = "Bearer";

        private readonly TestServer identityTestServer;

        public ApiServerStartup(IHostingEnvironment hostingEnvironment, TestServer identityTestServer)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.identityTestServer = identityTestServer;
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
                .ConfigureAllowedOriginsOptions(configuration)
                .ConfigureAuthorizationPolicies()
                .ConfigureDistributedCaching(configuration)
                .ConfigureDocumentDbContextOptions(configuration)
                .ConfigureIdentityServerConfigurationOptions(configuration)
                .ConfigureFileStoreOptions(configuration, this.hostingEnvironment)
                .ConfigureCompression();

            services.AddAuthentication(ApiServerStartup.AuthenticationSchema)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityServerConfiguration.Authority;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = ServiceNames.UserService;
                    options.JwtBackChannelHandler = this.identityTestServer.CreateHandler();
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCompression()
                .UseAuthentication()
                .SetupMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.SetupDependencies(this.hostingEnvironment);
            builder.Register(_ =>
                    new IdentityServerClient(new HttpClient(this.identityTestServer.CreateHandler())))
                .As<IIdentityServerClient>()
                .InstancePerLifetimeScope();
        }
    }
}