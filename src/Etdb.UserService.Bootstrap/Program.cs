using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Etdb.UserService.Bootstrap.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureService)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, true.ToString())
                .UseContentRoot(AppContext.BaseDirectory)
                .UseStartup<Startup>()
                .UseSerilog()
                .UseKestrel();

        private static void ConfigureService(IServiceCollection services) => services.AddAutofac();

        private static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder _)
        {
            var environment = ((Microsoft.Extensions.Hosting.IHostingEnvironment) context.HostingEnvironment);
            
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
            var environment = ((Microsoft.Extensions.Hosting.IHostingEnvironment) context.HostingEnvironment);


            if (!environment.IsAnyLocalDevelopment())
            {
                return;
            }

            builder.AddUserSecrets("Etdb_UserService");
        }
    }
}