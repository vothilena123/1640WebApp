using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1640WebApp.Data.Migrations
{
    public partial class _120 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorEmail",
                table: "Ideas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorEmail",
                table: "Ideas");
        }
    }
}
