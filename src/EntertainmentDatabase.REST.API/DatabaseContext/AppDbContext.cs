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
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment environment;

        public DbSet<Movie> Movies;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>(builder =>
            {
                builder.Property(movie => movie.Id)
                    .ForSqlServerHasComputedColumnSql("newid()");
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
