using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class gameDownextinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GameVersion",
                table: "GameDown_Main",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GameSize",
                table: "GameDown_Main",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "GameVersion",
                table: "GameDown_Main",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GameSize",
                table: "GameDown_Main",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
