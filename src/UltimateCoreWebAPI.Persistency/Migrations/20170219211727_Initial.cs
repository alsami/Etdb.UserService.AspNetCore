using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateCoreWebAPI.Persistency.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Designation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoPriorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Designation = table.Column<string>(nullable: true),
                    Prio = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    IsCompleted = table.Column<bool>(nullable: false),
                    Task = table.Column<string>(nullable: true),
                    TodoListId = table.Column<Guid>(nullable: false),
                    TodoPriorityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Todos_TodoLists_TodoListId",
                        column: x => x.TodoListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Todos_TodoPriorities_TodoPriorityId",
                        column: x => x.TodoPriorityId,
                        principalTable: "TodoPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos",
                column: "TodoListId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TodoPriorityId",
                table: "Todos",
                column: "TodoPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Task_TodoPriorityId",
                table: "Todos",
                columns: new[] { "Task", "TodoPriorityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_Designation",
                table: "TodoLists",
                column: "Designation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoPriorities_Designation",
                table: "TodoPriorities",
                column: "Designation",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.DropTable(
                name: "TodoLists");

            migrationBuilder.DropTable(
                name: "TodoPriorities");
        }
    }
}
