using Microsoft.EntityFrameworkCore.Migrations;

namespace PandaWeb.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Receipts_PackageId",
                table: "Receipts");

            migrationBuilder.AlterColumn<int>(
                name: "Apartment",
                table: "Flats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_PackageId",
                table: "Receipts",
                column: "PackageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Receipts_PackageId",
                table: "Receipts");

            migrationBuilder.AlterColumn<int>(
                name: "Apartment",
                table: "Flats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_PackageId",
                table: "Receipts",
                column: "PackageId",
                unique: true,
                filter: "[PackageId] IS NOT NULL");
        }
    }
}
