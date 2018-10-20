using System;
using System.IO;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.UserService.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Scaffold
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
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