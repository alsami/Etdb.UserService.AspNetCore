using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateCoreWebAPI.Persistency.Migrations
{
    public partial class Indexchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_Task_TodoPriorityId",
                table: "Todos");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TodoListId_Task",
                table: "Todos",
                columns: new[] { "TodoListId", "Task" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Todos_TodoListId_Task",
                table: "Todos");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos",
                column: "TodoListId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Task_TodoPriorityId",
                table: "Todos",
                columns: new[] { "Task", "TodoPriorityId" },
                unique: true);
        }
    }
}
