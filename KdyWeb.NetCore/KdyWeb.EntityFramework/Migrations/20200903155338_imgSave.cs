using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class imgSave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kdy.ImgSave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: false),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    FileMd5 = table.Column<string>(maxLength: 32, nullable: true),
                    MainUrl = table.Column<string>(maxLength: 200, nullable: true),
                    OneUrl = table.Column<string>(maxLength: 200, nullable: true),
                    TwoUrl = table.Column<string>(maxLength: 200, nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    UserNick = table.Column<string>(nullable: true),
                    UrlBack = table.Column<string>(maxLength: 200, nullable: true),
                    Urls = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kdy.ImgSave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrImgSave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: false),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    FileMd5 = table.Column<string>(maxLength: 32, nullable: true),
                    ImgPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrImgSave", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 3, 23, 53, 37, 424, DateTimeKind.Local).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 3, 23, 53, 37, 428, DateTimeKind.Local).AddTicks(7993));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 3, 23, 53, 37, 428, DateTimeKind.Local).AddTicks(8085));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 3, 23, 53, 37, 428, DateTimeKind.Local).AddTicks(8089));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 3, 23, 53, 37, 450, DateTimeKind.Local).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 3, 23, 53, 37, 450, DateTimeKind.Local).AddTicks(9449));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kdy.ImgSave");

            migrationBuilder.DropTable(
                name: "QrImgSave");

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 18, 58, 101, DateTimeKind.Local).AddTicks(4818));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 18, 58, 104, DateTimeKind.Local).AddTicks(7997));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 18, 58, 104, DateTimeKind.Local).AddTicks(8059));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 18, 58, 104, DateTimeKind.Local).AddTicks(8062));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 18, 58, 124, DateTimeKind.Local).AddTicks(2810));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 18, 58, 124, DateTimeKind.Local).AddTicks(3567));
        }
    }
}
