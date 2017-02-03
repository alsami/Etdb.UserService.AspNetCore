using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateCoreWebAPI.Persistency.Migrations
{
    public partial class AddUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Designation",
                table: "TodoPriorities",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Task",
                table: "TodoItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoPriorities_Designation",
                table: "TodoPriorities",
                column: "Designation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_Task_TodoPriorityId",
                table: "TodoItems",
                columns: new[] { "Task", "TodoPriorityId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TodoPriorities_Designation",
                table: "TodoPriorities");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_Task_TodoPriorityId",
                table: "TodoItems");

            migrationBuilder.AlterColumn<string>(
                name: "Designation",
                table: "TodoPriorities",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Task",
                table: "TodoItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
