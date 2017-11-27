using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ETDB.API.ServiceBase.General.Abstractions.Hasher;
using ETDB.API.ServiceBase.General.Hasher;
using ETDB.API.UserService.Data;
using ETDB.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ETDB.API.UserService.Scaffold
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables();

            var configurationRoot = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(configurationRoot)
                .AddSingleton<IHasher, Hasher>()
                .AddScoped<UserServiceContext>()
                .BuildServiceProvider();

            var databaseContext = serviceProvider
                .GetService<UserServiceContext>();

            if (databaseContext.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Migration(s) not applied, apply now? Y = Confirm!");
                Console.WriteLine($"Using Server { databaseContext.Database.GetDbConnection().DataSource }\n" +
                                  $"and Database { databaseContext.Database.GetDbConnection().Database }");

                if (!Console.ReadKey().KeyChar.ToString().Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                databaseContext.Database.Migrate();
            }

            var hasher = serviceProvider.GetService<IHasher>();

            var salt = hasher
                .GenerateSalt();

            var admin = new User(Guid.NewGuid(), "Admin", "Admin", "admin@etdb.io", "admin", 
                hasher.CreateSaltedHash("admin", salt), salt);

            databaseContext.Set<User>().Add(admin);

            databaseContext.SaveChanges();

            var securityRoles = new List<Securityrole>
            {
                new Securityrole(Guid.NewGuid(), "Administrator", null, true),
                new Securityrole(Guid.NewGuid(), "User", null, true)
            };

            foreach (var role in securityRoles)
            {
                databaseContext.Set<Securityrole>().Add(role);
                databaseContext.SaveChanges();
                databaseContext.Set<UserSecurityrole>().Add(new UserSecurityrole(Guid.NewGuid(), admin.Id, role.Id));
                databaseContext.SaveChanges();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
        }
    }
}
