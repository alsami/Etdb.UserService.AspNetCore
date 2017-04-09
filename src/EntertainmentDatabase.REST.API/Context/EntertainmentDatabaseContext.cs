using System.Linq;
using EntertainmentDatabase.REST.API.Entities.ConsumerMedia;
using EntertainmentDatabase.REST.ServiceBase.DataAccess.Extensions;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstraction;
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

        public DbSet<Movie> Movies;

        public EntertainmentDatabaseContext(IConfigurationRoot configurationRoot, IHostingEnvironment hostingEnvironment)
        {
            this.configurationRoot = configurationRoot;
            this.hostingEnvironment = hostingEnvironment;
        }

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

        //private void SetMovieDefinition(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Movie>(builder =>
        //    {
        //        modelBuilder.SetGuidAsPrimaryKey<Movie>();

        //        builder.Property(movie => movie.RowVersion)
        //            .ValueGeneratedOnAddOrUpdate()
        //            .IsConcurrencyToken();

        //        builder.HasIndex(movie => movie.Title)
        //            .IsUnique();

        //        builder.Property(movie => movie.Title)
        //            .IsRequired();
        //    });
        //}
    }
}
