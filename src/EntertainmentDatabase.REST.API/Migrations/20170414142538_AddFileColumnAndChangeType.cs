using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntertainmentDatabase.REST.API.Migrations
{
    public partial class AddFileColumnAndChangeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UniqueName",
                table: "MovieFile",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "MovieFile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "MovieFile");

            migrationBuilder.AlterColumn<string>(
                name: "UniqueName",
                table: "MovieFile",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
