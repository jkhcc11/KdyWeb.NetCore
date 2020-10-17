using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class doubanInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "QrImgSave",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 589, DateTimeKind.Local).AddTicks(1195));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyUser",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 819, DateTimeKind.Local).AddTicks(3709));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyRoleMenu",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 818, DateTimeKind.Local).AddTicks(4079));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyRole",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 811, DateTimeKind.Local).AddTicks(1345));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyMenu",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 794, DateTimeKind.Local).AddTicks(6124));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Kdy.ImgSave",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 577, DateTimeKind.Local).AddTicks(1298));

            migrationBuilder.CreateTable(
                name: "DouBanInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    VideoTitle = table.Column<string>(nullable: true),
                    DouBanInfoStatus = table.Column<byte>(nullable: false),
                    OldStatus = table.Column<string>(nullable: true),
                    Subtype = table.Column<byte>(nullable: false),
                    OldVideoType = table.Column<string>(nullable: true),
                    VideoYear = table.Column<int>(nullable: false),
                    VideoImg = table.Column<string>(nullable: true),
                    VideoCasts = table.Column<string>(nullable: true),
                    VideoDirectors = table.Column<string>(nullable: true),
                    VideoGenres = table.Column<string>(nullable: true),
                    VideoRating = table.Column<double>(nullable: false),
                    VideoSummary = table.Column<string>(nullable: true),
                    VideoDetailId = table.Column<string>(nullable: true),
                    VideoCountries = table.Column<string>(nullable: true),
                    RatingsCount = table.Column<int>(nullable: false),
                    CommentsCount = table.Column<int>(nullable: false),
                    ReviewsCount = table.Column<int>(nullable: false),
                    Aka = table.Column<string>(nullable: true),
                    ImdbStr = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DouBanInfo", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 17, 15, 16, 27, 622, DateTimeKind.Local).AddTicks(5004));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 17, 15, 16, 27, 628, DateTimeKind.Local).AddTicks(624));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 17, 15, 16, 27, 628, DateTimeKind.Local).AddTicks(703));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 17, 15, 16, 27, 628, DateTimeKind.Local).AddTicks(706));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 17, 15, 16, 27, 642, DateTimeKind.Local).AddTicks(8908));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 17, 15, 16, 27, 642, DateTimeKind.Local).AddTicks(9600));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DouBanInfo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "QrImgSave",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 589, DateTimeKind.Local).AddTicks(1195),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 819, DateTimeKind.Local).AddTicks(3709),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyRoleMenu",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 818, DateTimeKind.Local).AddTicks(4079),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyRole",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 811, DateTimeKind.Local).AddTicks(1345),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "KdyMenu",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 794, DateTimeKind.Local).AddTicks(6124),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Kdy.ImgSave",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 5, 14, 25, 8, 577, DateTimeKind.Local).AddTicks(1298),
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 5, 14, 25, 8, 812, DateTimeKind.Local).AddTicks(8644));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 5, 14, 25, 8, 813, DateTimeKind.Local).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 5, 14, 25, 8, 813, DateTimeKind.Local).AddTicks(291));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 5, 14, 25, 8, 813, DateTimeKind.Local).AddTicks(293));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 5, 14, 25, 8, 829, DateTimeKind.Local).AddTicks(41));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 9, 5, 14, 25, 8, 829, DateTimeKind.Local).AddTicks(1552));
        }
    }
}
