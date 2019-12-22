using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicProject.DataLayer.Migrations
{
    public partial class V2019_11_30_0136 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Mains",
                newName: "Mains",
                newSchema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                schema: "dbo",
                table: "Mains",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                schema: "dbo",
                table: "Mains");

            migrationBuilder.RenameTable(
                name: "Mains",
                schema: "dbo",
                newName: "Mains");
        }
    }
}
