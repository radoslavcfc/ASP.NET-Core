using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PandaWeb.Migrations
{
    public partial class DatesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Packages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredOn",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "RegisteredOn",
                table: "AspNetUsers");

          
        }
    }
}
