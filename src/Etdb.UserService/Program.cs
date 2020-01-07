using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Etdb.UserService.Autofac.Extensions;
using Etdb.UserService.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Etdb.UserService
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        private static readonly string LogPath = Path.Combine(AppContext.BaseDirectory, "Logs",
            $"{Assembly.GetExecutingAssembly().GetName().Name}.log");

        public static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(ConfigureLogger)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder
                        .ConfigureAppConfiguration(ConfigureAppConfiguration)
                        .CaptureStartupErrors(true)
                        .UseSetting(WebHostDefaults.DetailedErrorsKey, true.ToString())
                        .UseContentRoot(AppContext.BaseDirectory)
                        .UseStartup<Startup>();
                });

        private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration configuration)
        {
            var environment = context.HostingEnvironment;

            configuration
                .MinimumLevel.Is(environment.IsAnyDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(Program.LogPath)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate);
        }

        private static void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            builder
                .AddEnvironmentVariables()
                .AddUserSecrets("Etdb_UserService");
        }
    }
}