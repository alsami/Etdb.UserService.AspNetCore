using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Etdb.UserService.Controllers.Tests.Startups;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Etdb.UserService.Controllers.Tests.Fixtures
{
    public class TestServerFixture
    {
        public TestServerFixture()
        {
            this.ExternalIdentityHttpMessageHandlerMock = new Mock<HttpMessageHandler>();

            this.Initialize();
        }

        public TestServer ApiServer { get; private set; }

        public TestServer IdentityServer { get; private set; }

        public Mock<HttpMessageHandler> ExternalIdentityHttpMessageHandlerMock { get; }

        private void Initialize()
        {
            this.IdentityServer = new TestServer(new WebHostBuilder()
                .ConfigureServices(sp =>
                {
                    sp.AddSingleton(this.ExternalIdentityHttpMessageHandlerMock.Object);
                    sp.AddAutofac();
                })
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .UseEnvironment("CI")
                .UseSerilog(TestServerFixture.ConfigureLogger)
                .UseStartup<IdentityServerStartup>()
                .UseContentRoot(AppContext.BaseDirectory));

            this.ApiServer = new TestServer(new WebHostBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(this.IdentityServer);
                    services.AddAutofac();
                })
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .UseEnvironment("CI")
                .UseSerilog(TestServerFixture.ConfigureLogger)
                .UseStartup<ApiServerStartup>()
                .UseContentRoot(AppContext.BaseDirectory));
        }

        private void ConfigureAppConfiguration(WebHostBuilderContext _, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder.SetBasePath(AppContext.BaseDirectory);

            builder.AddJsonFile("appsettings.Development.json", false)
                .AddEnvironmentVariables()
                .AddUserSecrets("Etdb_UserService");
        }

        private static void ConfigureLogger(WebHostBuilderContext _, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate);
        }
    }
}