using System.Linq;
using EntertainmentDatabase.REST.API.DataAccess.Configuration;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace EntertainmentDatabase.REST.API.DataAccess
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

        public DbSet<Actor> Actors;

        public DbSet<MovieActors> ActorMovies;

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

            new ActionLogConfiguration(modelBuilder)
                .ConfigureEntity();

            new ErrorLogConfiguration(modelBuilder)
                .ConfigureEntity();

            new MovieConfiguration(modelBuilder)
                .ConfigureEntity();

            new MovieFileConfiguration(modelBuilder)
                .ConfigureEntity();

            new ActorConfiguration(modelBuilder)
                .ConfigureEntity();

            new MovieActorsConfiguration(modelBuilder)
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
