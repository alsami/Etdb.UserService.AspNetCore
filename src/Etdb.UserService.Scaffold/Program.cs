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
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets(Program.UserSecretsId)
                .Build();

            var service = new ServiceCollection();

            service.AddOptions();

            service.Configure<DocumentDbContextOptions>(options =>
            {
                configuration.GetSection(nameof(DocumentDbContextOptions)).Bind(options);
            });

            var provider = service.BuildServiceProvider();

            ContextScaffold.Scaffold(
                new UserServiceDbContext(provider.GetService<IOptions<DocumentDbContextOptions>>()));
        }
    }
}