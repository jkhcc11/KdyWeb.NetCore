using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VideoTitle",
                table: "DouBanInfo",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoImg",
                table: "DouBanInfo",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoGenres",
                table: "DouBanInfo",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoDirectors",
                table: "DouBanInfo",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoDetailId",
                table: "DouBanInfo",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoCountries",
                table: "DouBanInfo",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoCasts",
                table: "DouBanInfo",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Subtype",
                table: "DouBanInfo",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewsCount",
                table: "DouBanInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RatingsCount",
                table: "DouBanInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ImdbStr",
                table: "DouBanInfo",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "DouBanInfoStatus",
                table: "DouBanInfo",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "CommentsCount",
                table: "DouBanInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Aka",
                table: "DouBanInfo",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VideoTitle",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoImg",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoGenres",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoDirectors",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoDetailId",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoCountries",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoCasts",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Subtype",
                table: "DouBanInfo",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)0);

            migrationBuilder.AlterColumn<int>(
                name: "ReviewsCount",
                table: "DouBanInfo",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatingsCount",
                table: "DouBanInfo",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImdbStr",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "DouBanInfoStatus",
                table: "DouBanInfo",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)0);

            migrationBuilder.AlterColumn<int>(
                name: "CommentsCount",
                table: "DouBanInfo",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Aka",
                table: "DouBanInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 18, 17, 27, 53, 530, DateTimeKind.Local).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 18, 17, 27, 53, 533, DateTimeKind.Local).AddTicks(8385));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 18, 17, 27, 53, 533, DateTimeKind.Local).AddTicks(8434));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 18, 17, 27, 53, 533, DateTimeKind.Local).AddTicks(8438));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 18, 17, 27, 53, 553, DateTimeKind.Local).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 10, 18, 17, 27, 53, 553, DateTimeKind.Local).AddTicks(4462));
        }
    }
}
