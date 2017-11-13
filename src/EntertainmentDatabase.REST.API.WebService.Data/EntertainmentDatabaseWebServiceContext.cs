using System.Linq;
using EntertainmentDatabase.REST.API.WebService.Data.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EntertainmentDatabase.REST.API.WebService.Data
{
    public class EntertainmentDatabaseWebServiceContext : DbContext
    {
        private const string Production = "Production";
        private const string Development = "Development";
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment hostingEnvironment;

        public EntertainmentDatabaseWebServiceContext(IConfigurationRoot configurationRoot, IHostingEnvironment hostingEnvironment)
        {
            this.configurationRoot = configurationRoot;
            this.hostingEnvironment = hostingEnvironment;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                this.hostingEnvironment.IsDevelopment()
                    ? this.configurationRoot.GetConnectionString(EntertainmentDatabaseWebServiceContext.Development)
                    : this.configurationRoot.GetConnectionString(EntertainmentDatabaseWebServiceContext.Production));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ActionLogConfiguration(modelBuilder)
                .ConfigureEntity();

            new ErrorLogConfiguration(modelBuilder)
                .ConfigureEntity();

            new MovieConfiguration(modelBuilder)
                .ConfigureEntity();

            new MovieCoverImageConfiguration(modelBuilder)
                .ConfigureEntity();

            new MovieFileConfiguration(modelBuilder)
                .ConfigureEntity();

            new ActorConfiguration(modelBuilder)
                .ConfigureEntity();

            new MovieActorConfiguration(modelBuilder)
                .ConfigureEntity();

            this.DisableCascadeDelete(modelBuilder);
        }

        private void DisableCascadeDelete(ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
