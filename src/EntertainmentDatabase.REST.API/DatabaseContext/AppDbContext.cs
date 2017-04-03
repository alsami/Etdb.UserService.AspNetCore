using System.Linq;
using Autofac.Core.Activators;
using EntertainmentDatabase.REST.Domain.Abstraction;
using EntertainmentDatabase.REST.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

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
            modelBuilder.Entity<Movie>(builder =>
            {
                builder.Property(movie => movie.Id)
                    .ForSqlServerHasDefaultValueSql("newid()");
            });

            //modelBuilder.Entity<TodoList>(builder =>
            //{
            //    builder.Property(todoList => todoList.Id)
            //        .ForSqlServerHasDefaultValueSql("newid()");
            //    builder.HasIndex(todoList => todoList.Designation)
            //        .IsUnique();
            //    builder.HasMany(todoList => todoList.Todos)
            //    .WithOne(todoList => todoList.TodoList)
            //    .HasForeignKey(tdi => tdi.TodoListId);
            //});

            //modelBuilder.Entity<Todo>(builder =>
            //{
            //    builder.Property(todoList => todoList.Id)
            //        .ForSqlServerHasDefaultValueSql("newid()");
            //    builder
            //        .HasOne(todo => todo.TodoPriority)
            //        .WithMany();
            //    builder.HasIndex(todo => new
            //    {
            //        todo.TodoListId,
            //        todo.Task
            //    })
            //    .IsUnique();
            //});

            //modelBuilder.Entity<TodoPriority>(builder =>
            //{
            //    builder.Property(todoPriority => todoPriority.Id)
            //        .ForSqlServerHasDefaultValueSql("newid()");
            //    builder
            //        .HasIndex(todoPriority => todoPriority.Designation)
            //        .IsUnique();
            //});


            // supress cascade delete for all tables
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
