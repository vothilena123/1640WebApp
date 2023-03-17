using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1640WebApp.Data.Migrations
{
    public partial class _90 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Anonymous",
                table: "Ideas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anonymous",
                table: "Ideas");
        }
    }
}
