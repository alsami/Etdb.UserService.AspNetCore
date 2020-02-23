using System.Net.Http;
using Autofac;
using Etdb.ServiceBase.Constants;
using Etdb.UserService.Autofac.Extensions;
using Etdb.UserService.Controllers.Tests.Extensions;
using Etdb.UserService.Extensions;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Etdb.UserService.Controllers.Tests.Startups
{
    public class ApiServerStartup
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        private const string AuthenticationSchema = "Bearer";

        private readonly TestServer identityTestServer;

        private readonly IConfiguration configuration;

        private const string AzureServiceBusConnectionString = "AzureServiceBus";

        public ApiServerStartup(IWebHostEnvironment hostingEnvironment, TestServer identityTestServer,
            IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.identityTestServer = identityTestServer;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var identityServerConfiguration = this.configuration
                .GetSection(nameof(IdentityServerConfiguration))
                .Get<IdentityServerConfiguration>();

            services.Configure<AzureServiceBusConfiguration>(options =>
                options.ConnectionString =
                    this.configuration.GetConnectionString(ApiServerStartup.AzureServiceBusConnectionString));

            // this.hostingEnvironment.EnvironmentName = Environments.Development;

            services
                .ConfigureApiControllers()
                .ConfigureAllowedOriginsOptions(this.configuration)
                .ConfigureAuthorizationPolicies()
                .ConfigureDistributedCaching(this.configuration)
                .ConfigureDocumentDbContextOptions(this.configuration)
                .ConfigureIdentityServerConfigurationOptions(this.configuration)
                .ConfigureFileStoreOptions(this.configuration, this.hostingEnvironment)
                .ConfigureResponseCompression();

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
                .UseConfiguredRouting();
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