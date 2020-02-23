using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extensions.FluentBuilder;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.UserService.Autofac.Modules;
using Etdb.UserService.Repositories;
using Etdb.UserService.Scaffolder.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace Etdb.UserService.Scaffolder
{
    public class Program
    {
        private const string UserSecretsId = "Etdb_UserService";

        public static async Task Main(string[] args)
        {
            using var host = BuildHost(args);

            var scaffolder = host.Services.GetRequiredService<DatabaseScaffolder>();

            await scaffolder.ScaffoldAsync();
        }

        private static IHost BuildHost(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .Build();

        private static void ConfigureAppConfiguration(IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder.AddEnvironmentVariables()
                .AddUserSecrets(Program.UserSecretsId);
        }

        private static void ConfigureContainer(HostBuilderContext context, ContainerBuilder builder)
        {
            var environmentMock = new Mock<IHostEnvironment>();

            var documentDbContextOptins = context.Configuration
                .GetSection(nameof(DocumentDbContextOptions))
                .Get<DocumentDbContextOptions>();

            builder.RegisterInstance(Options.Create(documentDbContextOptins))
                .As<IOptions<DocumentDbContextOptions>>()
                .SingleInstance();

            environmentMock.Setup(env => env.EnvironmentName)
                .Returns("Development");

            new AutofacFluentBuilder(
                    builder.RegisterSerilog(Path.Combine(AppContext.BaseDirectory, "UserServiceScaffold.log")))
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypeAsScoped<DatabaseScaffolder>()
                .RegisterTypeAsScoped<CollectionsFactory>()
                .RegisterTypeAsScoped<IndicesFactory>()
                .RegisterTypeAsScoped<DefaultDataFactory>()
                .ApplyModule(new DocumentDbContextModule(environmentMock.Object));
        }
    }
}