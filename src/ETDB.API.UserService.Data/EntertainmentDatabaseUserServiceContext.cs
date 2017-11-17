using System;
using System.Linq;
using ETDB.API.UserService.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ETDB.API.UserService.Data
{
    public class EntertainmentDatabaseUserServiceContext : DbContext
    {
        private readonly IConfigurationRoot configurationRoot;
        private const string Production = "Production";
        private const string Development = "Development";

        public EntertainmentDatabaseUserServiceContext(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var environmentName = Environment
                                      .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                                  EntertainmentDatabaseUserServiceContext.Development;

            optionsBuilder.UseSqlServer(environmentName
                .Equals(EntertainmentDatabaseUserServiceContext.Development, StringComparison.OrdinalIgnoreCase)
                ? this.configurationRoot.GetConnectionString(EntertainmentDatabaseUserServiceContext.Development)
                : environmentName
                    .Equals(EntertainmentDatabaseUserServiceContext.Production, StringComparison.OrdinalIgnoreCase)
                    ? this.configurationRoot.GetConnectionString(EntertainmentDatabaseUserServiceContext.Production)
                    : throw new ArgumentException(nameof(environmentName)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserMappingConfiguration(modelBuilder)
                .ConfigureEntity();

            new SecurityroleMappingConfiguration(modelBuilder)
                .ConfigureEntity();

            new UserSecurityroleMappingConfiguration(modelBuilder)
                .ConfigureEntity();

            this.DisableCascadeDelete(modelBuilder);
        }

        private void DisableCascadeDelete(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                entity.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
