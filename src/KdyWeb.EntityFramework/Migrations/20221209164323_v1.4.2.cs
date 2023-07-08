using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v142 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GameDown_Main",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtInfo",
                table: "GameDown_Main",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MovieList",
                table: "GameDown_Main",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamId",
                table: "GameDown_Main",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamUrl",
                table: "GameDown_Main",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "GameDown_Main");

            migrationBuilder.DropColumn(
                name: "ExtInfo",
                table: "GameDown_Main");

            migrationBuilder.DropColumn(
                name: "MovieList",
                table: "GameDown_Main");

            migrationBuilder.DropColumn(
                name: "SteamId",
                table: "GameDown_Main");

            migrationBuilder.DropColumn(
                name: "SteamUrl",
                table: "GameDown_Main");
        }
    }
}
