using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using UltimateCoreWebAPI.Model.Entities;

namespace UltimateCoreWebAPI.Persistency
{
    public class CoreWebAPIContext : DbContext
    {
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment environment;
        private const string Production = "Production";
        private const string Development = "Development";

        public CoreWebAPIContext(IConfigurationRoot configurationRoot, IHostingEnvironment environment)
        {
            this.configurationRoot = configurationRoot;
            this.environment = environment;
        }

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoPriority> TodoPriorities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(this.environment.IsDevelopment()
                ? this.configurationRoot.GetConnectionString(CoreWebAPIContext.Development)
                : this.configurationRoot.GetConnectionString(CoreWebAPIContext.Production));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoList>(builder =>
            {
                builder.Property(todoList => todoList.Id)
                    .ForSqlServerHasDefaultValueSql("newid()");
                builder.HasIndex(todoList => todoList.Designation)
                    .IsUnique();
                builder.HasMany(todoList => todoList.TodoItems)
                .WithOne(todoList => todoList.TodoList)
                .HasForeignKey(tdi => tdi.TodoListId);
            });

            modelBuilder.Entity<Todo>(builder =>
            {
                builder.Property(todoList => todoList.Id)
                    .ForSqlServerHasDefaultValueSql("newid()");
                builder
                    .HasOne(todo => todo.TodoPriority)
                    .WithMany();
                builder.HasIndex(todo => new
                {
                    todo.TodoListId,
                    todo.Task
                })
                .IsUnique();
            });

            modelBuilder.Entity<TodoPriority>(builder =>
            {
                builder.Property(todoPriority => todoPriority.Id)
                    .ForSqlServerHasDefaultValueSql("newid()");
                builder
                    .HasIndex(todoPriority => todoPriority.Designation)
                    .IsUnique();
            });


            // supress cascade delete for all tables
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
