using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Etdb.UserService.Bootstrap
{
    public class Program
    {
        private static readonly string LogPath = Path.Combine(AppContext.BaseDirectory, "Logs", $"{Assembly.GetEntryAssembly().GetName().Name}.log");

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(LogPath)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", 
                    theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices(services => services.AddAutofac())
                .Build();
    }
}
