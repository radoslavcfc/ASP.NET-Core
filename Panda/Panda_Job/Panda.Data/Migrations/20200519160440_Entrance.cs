using Microsoft.EntityFrameworkCore.Migrations;

namespace Panda.Data.Migrations
{
    public partial class Entrance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flat",
                table: "Addresses");

            migrationBuilder.AlterColumn<int>(
                name: "Floor",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Apartment",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Entrance",
                table: "Addresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entrance",
                table: "Addresses");

            migrationBuilder.AlterColumn<int>(
                name: "Floor",
                table: "Addresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Apartment",
                table: "Addresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Flat",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
