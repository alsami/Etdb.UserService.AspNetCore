﻿using System;
using System.IO;
using System.Net.Http;
using Autofac;
using Etdb.ServiceBase.Constants;
using Etdb.UserService.Bootstrap.Extensions;
using Etdb.UserService.Controllers.Tests.Extensions;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Controllers.Tests.Startups
{
    public class ApiServerStartup
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        private const string AuthenticationSchema = "Bearer";

        private readonly TestServer identityTestServer;

        public ApiServerStartup(IWebHostEnvironment hostingEnvironment, TestServer identityTestServer)
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

            this.hostingEnvironment.EnvironmentName = Environments.Development;

            services
                .ConfigureApiControllers()
                .ConfigureAllowedOriginsOptions(configuration)
                .ConfigureAuthorizationPolicies()
                .ConfigureDistributedCaching(configuration)
                .ConfigureDocumentDbContextOptions(configuration)
                .ConfigureIdentityServerConfigurationOptions(configuration)
                .ConfigureFileStoreOptions(configuration, this.hostingEnvironment)
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