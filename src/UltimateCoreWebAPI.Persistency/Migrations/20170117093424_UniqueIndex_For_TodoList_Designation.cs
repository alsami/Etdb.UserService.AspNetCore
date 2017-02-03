using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateCoreWebAPI.Persistency.Migrations
{
    public partial class UniqueIndex_For_TodoList_Designation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Designation",
                table: "TodoLists",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_Designation",
                table: "TodoLists",
                column: "Designation",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TodoLists_Designation",
                table: "TodoLists");

            migrationBuilder.AlterColumn<string>(
                name: "Designation",
                table: "TodoLists",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
