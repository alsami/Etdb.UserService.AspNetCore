using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UltimateCoreWebAPI.Persistency;

namespace UltimateCoreWebAPI.Persistency.Migrations
{
    [DbContext(typeof(PlaygroundContext))]
    [Migration("20161207153506_AddUniqueIndexes")]
    partial class AddUniqueIndexes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PlaygroundBackend.Model.Entities.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CompletedAt");

                    b.Property<bool>("IsCompleted");

                    b.Property<string>("Task");

                    b.Property<int>("TodoListId");

                    b.Property<int>("TodoPriorityId");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.HasIndex("TodoPriorityId");

                    b.HasIndex("Task", "TodoPriorityId")
                        .IsUnique();

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("PlaygroundBackend.Model.Entities.TodoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Designation");

                    b.HasKey("Id");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("PlaygroundBackend.Model.Entities.TodoPriority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Designation");

                    b.Property<short>("Prio");

                    b.HasKey("Id");

                    b.HasIndex("Designation")
                        .IsUnique();

                    b.ToTable("TodoPriorities");
                });

            modelBuilder.Entity("PlaygroundBackend.Model.Entities.TodoItem", b =>
                {
                    b.HasOne("PlaygroundBackend.Model.Entities.TodoList", "TodoList")
                        .WithMany("TodoItems")
                        .HasForeignKey("TodoListId");

                    b.HasOne("PlaygroundBackend.Model.Entities.TodoPriority", "TodoPriority")
                        .WithMany()
                        .HasForeignKey("TodoPriorityId");
                });
        }
    }
}
