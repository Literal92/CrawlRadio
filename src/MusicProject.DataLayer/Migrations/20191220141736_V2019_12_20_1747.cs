using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicProject.DataLayer.Migrations
{
    public partial class V2019_12_20_1747 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Music",
                schema: "dbo",
                table: "Mains",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Music",
                schema: "dbo",
                table: "Mains",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
