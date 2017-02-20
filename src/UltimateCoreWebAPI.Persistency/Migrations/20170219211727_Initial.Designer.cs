using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UltimateCoreWebAPI.Persistency;

namespace UltimateCoreWebAPI.Persistency.Migrations
{
    [DbContext(typeof(CoreWebAPIContext))]
    [Migration("20170219211727_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UltimateCoreWebAPI.Model.Entities.Todo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<DateTime?>("CompletedAt");

                    b.Property<bool>("IsCompleted");

                    b.Property<string>("Task");

                    b.Property<Guid>("TodoListId");

                    b.Property<Guid>("TodoPriorityId");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.HasIndex("TodoPriorityId");

                    b.HasIndex("Task", "TodoPriorityId")
                        .IsUnique();

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("UltimateCoreWebAPI.Model.Entities.TodoList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("Designation");

                    b.HasKey("Id");

                    b.HasIndex("Designation")
                        .IsUnique();

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("UltimateCoreWebAPI.Model.Entities.TodoPriority", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("Designation");

                    b.Property<short>("Prio");

                    b.HasKey("Id");

                    b.HasIndex("Designation")
                        .IsUnique();

                    b.ToTable("TodoPriorities");
                });

            modelBuilder.Entity("UltimateCoreWebAPI.Model.Entities.Todo", b =>
                {
                    b.HasOne("UltimateCoreWebAPI.Model.Entities.TodoList", "TodoList")
                        .WithMany("TodoItems")
                        .HasForeignKey("TodoListId");

                    b.HasOne("UltimateCoreWebAPI.Model.Entities.TodoPriority", "TodoPriority")
                        .WithMany()
                        .HasForeignKey("TodoPriorityId");
                });
        }
    }
}
