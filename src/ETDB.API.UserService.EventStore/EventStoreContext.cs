using System;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ETDB.API.UserService.EventStore
{
    public class EventStoreContext : EventStoreContextBase
    {
        private readonly IConfigurationRoot configurationRoot;
        private const string Production = "Production";
        private const string Development = "Development";

        public EventStoreContext(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? EventStoreContext.Development;

            optionsBuilder.UseSqlServer(environmentName
                .Equals(EventStoreContext.Development, StringComparison.OrdinalIgnoreCase)
                ? this.configurationRoot.GetConnectionString(EventStoreContext.Development)
                : environmentName
                    .Equals(EventStoreContext.Production, StringComparison.OrdinalIgnoreCase)
                    ? this.configurationRoot.GetConnectionString(EventStoreContext.Production)
                    : throw new ArgumentException(nameof(environmentName)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoreEventMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
