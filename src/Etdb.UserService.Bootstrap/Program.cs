using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Etdb.UserService.Bootstrap.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Etdb.UserService.Bootstrap
{
    public class Program
    {
        private static readonly string LogPath = Path.Combine(AppContext.BaseDirectory, "Logs",
            $"{Assembly.GetExecutingAssembly().GetName().Name}.log");

        public static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.ConfigureServices(ConfigureService)
                        .ConfigureLogging(ConfigureLogging)
                        .ConfigureAppConfiguration(ConfigureAppConfiguration)
                        .CaptureStartupErrors(true)
                        .UseSetting(WebHostDefaults.DetailedErrorsKey, true.ToString())
                        .UseContentRoot(AppContext.BaseDirectory)
                        .UseStartup<Startup>()
                        .UseSerilog()
                        .UseKestrel();
                });


        private static void ConfigureService(WebHostBuilderContext context, IServiceCollection services)
        {
            var environment = context.HostingEnvironment;

            environment.EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE:Environment")
                                          ?? Environment.GetEnvironmentVariable("ASPNETCORE__Environment")
                                          ?? Environment.GetEnvironmentVariable("ASPNETCORE_Environment")
                                          ?? Environments.Development;
        }

        private static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder _)
        {
            var environment = context.HostingEnvironment;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(environment.IsAnyLocalDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(Program.LogPath)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
        }

        private static void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            builder
                .AddEnvironmentVariables()
                .AddUserSecrets("Etdb_UserService");
        }
    }
}