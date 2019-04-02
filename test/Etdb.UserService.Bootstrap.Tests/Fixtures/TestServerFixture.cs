using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Etdb.UserService.Bootstrap.Tests.Startups;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Etdb.UserService.Bootstrap.Tests.Fixtures
{
    public class TestServerFixture
    {
        public TestServer ApiServer { get; private set; }

        public TestServer IdentityServer { get; private set; }

        public Mock<HttpMessageHandler> ExternalIdentityHttpMessageHandlerMock { get; }

        public TestServerFixture()
        {
            this.ExternalIdentityHttpMessageHandlerMock = new Mock<HttpMessageHandler>();

            this.Initialize();
        }

        private void Initialize()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(Path.Combine(AppContext.BaseDirectory, "Logs",
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.log"))
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            this.IdentityServer = new TestServer(new WebHostBuilder()
                .UseSerilog()
                .ConfigureServices(sp =>
                {
                    sp.AddSingleton(this.ExternalIdentityHttpMessageHandlerMock.Object);
                    sp.AddAutofac();
                })
                .UseStartup<IdentityServerStartup>());

            this.ApiServer = new TestServer(new WebHostBuilder()
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(this.IdentityServer);
                    services.AddAutofac();
                })
                .UseStartup<ApiServerStartup>());
        }
    }
}