using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.UserService.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Scaffold
{
    public class Program
    {
        private const string UserSecretsId = "Etdb_UserService";

        public static void Main(string[] _)
        {
            Console.WriteLine("Building config");
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Program.UserSecretsId)
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("Configuring services");
            var service = new ServiceCollection();

            service.AddOptions();

            service.Configure<DocumentDbContextOptions>(options =>
            {
                configuration.GetSection(nameof(DocumentDbContextOptions)).Bind(options);
            });

            Console.WriteLine("Building provider");
            var provider = service.BuildServiceProvider();

            Console.WriteLine("Running scaffold");
            try
            {
                ContextScaffold.Scaffold(
                                new UserServiceDbContext(provider.GetService<IOptions<DocumentDbContextOptions>>()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}