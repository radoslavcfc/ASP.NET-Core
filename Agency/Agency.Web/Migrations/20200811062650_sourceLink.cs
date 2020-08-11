using Microsoft.EntityFrameworkCore.Migrations;

namespace Agency.Web.Migrations
{
    public partial class sourceLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionSource",
                table: "Workers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionSource",
                table: "Workers");
        }
    }
}
