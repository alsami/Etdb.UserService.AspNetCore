using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extensions.FluentBuilder;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.UserService.Repositories;
using Etdb.UserService.Scaffolder.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace Etdb.UserService.Scaffolder
{
    public class Program
    {
        private const string UserSecretsId = "Etdb_UserService";

        public static async Task  Main(string[] args)
        {
            using var host = BuildHost(args);
            
            var scaffolder = host.Services.GetRequiredService<DatabaseScaffolder>();

            await scaffolder.ScaffoldAsync();
        }

        private static IHost BuildHost(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureContainer))
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();

                    builder.AddEnvironmentVariables()
                        .AddUserSecrets(Program.UserSecretsId);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();

                    services.Configure<DocumentDbContextOptions>(options =>
                    {
                        hostContext.Configuration.GetSection(nameof(DocumentDbContextOptions)).Bind(options);
                    });

                    services.AddScoped<UserServiceDbContext>();
                })
                .Build();

        private static void ConfigureContainer(ContainerBuilder builder)
        {
            new AutofacFluentBuilder(
                    builder.RegisterSerilog(Path.Combine(AppContext.BaseDirectory, "UserServiceScaffold.og")))
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypeAsScoped<DatabaseScaffolder>()
                .RegisterTypeAsScoped<CollectionsFactory>()
                .RegisterTypeAsScoped<IndicesFactory>()
                .RegisterTypeAsScoped<DefaultDataFactory>();
        }
    }
}
