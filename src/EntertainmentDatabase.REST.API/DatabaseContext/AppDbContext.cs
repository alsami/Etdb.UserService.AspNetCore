using System.Linq;
using Autofac.Core.Activators;
using EntertainmentDatabase.REST.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        private const string Production = "Production";
        private const string Development = "Development";
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment hostingEnvironment;

        public DbSet<Movie> Movies;

        public AppDbContext(IConfigurationRoot configurationRoot, IHostingEnvironment hostingEnvironment)
        {
            this.configurationRoot = configurationRoot;
            this.hostingEnvironment = hostingEnvironment;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                this.hostingEnvironment.IsDevelopment()
                    ? this.configurationRoot.GetConnectionString(AppDbContext.Development)
                    : this.configurationRoot.GetConnectionString(AppDbContext.Production));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.BuildMovieTable(modelBuilder);

            // supress cascade delete for all tables
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private void BuildMovieTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(builder =>
            {
                builder.Property(movie => movie.Id)
                    .ForSqlServerHasDefaultValueSql("newid()");

                builder.Property(movie => movie.RowVersion)
                    .ValueGeneratedOnAddOrUpdate()
                    .IsConcurrencyToken();

                builder.HasIndex(movie => movie.Title)
                    .IsUnique();

                builder.Property(movie => movie.Title)
                    .IsRequired();
            });
        }
    }
}
