using System.Linq;
using EntertainmentDatabase.REST.API.Context.Mappings;
using EntertainmentDatabase.REST.API.Entities;
using EntertainmentDatabase.REST.API.Entities.ConsumerMedia;
using EntertainmentDatabase.REST.ServiceBase.Generics.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace EntertainmentDatabase.REST.API.Context
{
    public class EntertainmentDatabaseContext : DbContext
    {
        private const string Production = "Production";
        private const string Development = "Development";
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment hostingEnvironment;

        public EntertainmentDatabaseContext(IConfigurationRoot configurationRoot, IHostingEnvironment hostingEnvironment)
        {
            this.configurationRoot = configurationRoot;
            this.hostingEnvironment = hostingEnvironment;
        }

        public DbSet<Movie> Movies;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                this.hostingEnvironment.IsDevelopment()
                    ? this.configurationRoot.GetConnectionString(EntertainmentDatabaseContext.Development)
                    : this.configurationRoot.GetConnectionString(EntertainmentDatabaseContext.Production));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new MovieEntityMappingConfiguration()
                .Map(modelBuilder);

            modelBuilder.SupressCascadeDelete();
        }
    }
}
