using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PlaygroundBackend.Model.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PlaygroundBackend.Persistency
{
    public class PlaygroundContext : DbContext
    {

        public PlaygroundContext()
        {
            //var configurationBuilder = new ConfigurationBuilder()
            //    .SetBasePath(Assembly.)
            //    .AddJsonFile("settings.json");

            //this.Configuration = configurationBuilder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public DbSet<Test> Tests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // optionsBuilder.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            // temporary workaround since options for constructor aren't available yet
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PlaygroundBackend;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // supress cascade delete
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
