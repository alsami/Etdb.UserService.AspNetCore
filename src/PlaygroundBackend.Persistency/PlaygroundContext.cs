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
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment environment;
        private const string Production = "Production";
        private const string Development = "Development";

        public PlaygroundContext(IConfigurationRoot configurationRoot, IHostingEnvironment environment)
        {
            this.configurationRoot = configurationRoot;
            this.environment = environment;
        }

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoPriority> TodoPriorities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(this.environment.IsDevelopment()
                ? this.configurationRoot.GetConnectionString(PlaygroundContext.Development)
                : this.configurationRoot.GetConnectionString(PlaygroundContext.Production));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoList>()
                .HasIndex(todoList => todoList.Designation)
                .IsUnique();
            modelBuilder.Entity<TodoList>()
                .HasMany(todoList => todoList.TodoItems)
                .WithOne(todoList => todoList.TodoList)
                .HasForeignKey(tdi => tdi.TodoListId);

            modelBuilder.Entity<TodoItem>()
                .HasOne(todoItem => todoItem.TodoPriority)
                .WithMany();
            modelBuilder.Entity<TodoItem>()
                .HasIndex(todoItem => new {
                    todoItem.Task,
                    todoItem.TodoPriorityId
                })
                .IsUnique();

            modelBuilder.Entity<TodoPriority>()
                .HasIndex(todoPriority => todoPriority.Designation)
                .IsUnique();


            // supress cascade delete for all tables
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
