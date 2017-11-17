using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ETDB.API.UserService.Data.Factory
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntertainmentDatabaseUserServiceContext>
    {
        public EntertainmentDatabaseUserServiceContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var context = new EntertainmentDatabaseUserServiceContext(configuration);
            return context;
        }
    }
}
