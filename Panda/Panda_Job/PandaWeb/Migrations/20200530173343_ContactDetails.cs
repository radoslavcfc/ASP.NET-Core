using Microsoft.EntityFrameworkCore.Migrations;

namespace PandaWeb.Migrations
{
    public partial class ContactDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainContactNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondContactNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressType",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MainContactNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecondContactNumber",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AddressType",
                table: "Addresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
