using System;
using ETDB.API.ServiceBase.Repositories.Abstractions.Base;
using ETDB.API.UserService.Data.EntityMaps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ETDB.API.UserService.Data
{
    public class UserServiceContext : AppContextBase
    {
        private readonly IConfigurationRoot configurationRoot;
        private const string Production = "Production";
        private const string Development = "Development";

        public UserServiceContext(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? UserServiceContext.Development;

            optionsBuilder.UseSqlServer(environmentName
                .Equals(UserServiceContext.Development, StringComparison.OrdinalIgnoreCase)
                ? this.configurationRoot.GetConnectionString(UserServiceContext.Development)
                : environmentName
                    .Equals(UserServiceContext.Production, StringComparison.OrdinalIgnoreCase)
                    ? this.configurationRoot.GetConnectionString(UserServiceContext.Production)
                    : throw new ArgumentException(nameof(environmentName)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());

            modelBuilder.ApplyConfiguration(new SecurityroleMap());

            modelBuilder.ApplyConfiguration(new UserSecurityroleMap());

            this.DisableCascadeDelete(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
