using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedBackInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    FeedBackInfoStatus = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    OriginalUrl = table.Column<string>(maxLength: 200, nullable: true),
                    DemandType = table.Column<int>(nullable: false),
                    VideoName = table.Column<string>(maxLength: 100, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackInfo", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedBackInfo");

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 19, 22, 38, 2, 707, DateTimeKind.Local).AddTicks(118));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 19, 22, 38, 2, 712, DateTimeKind.Local).AddTicks(1450));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 19, 22, 38, 2, 712, DateTimeKind.Local).AddTicks(2992));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 19, 22, 38, 2, 712, DateTimeKind.Local).AddTicks(3000));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 19, 22, 38, 2, 729, DateTimeKind.Local).AddTicks(7942));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 19, 22, 38, 2, 729, DateTimeKind.Local).AddTicks(8981));
        }
    }
}
