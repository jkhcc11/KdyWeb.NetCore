using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class createUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "QrImgSave",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyRoleMenu",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyRole",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyMenu",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "Kdy.ImgSave",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 21, 54, 837, DateTimeKind.Local).AddTicks(5770), null });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 21, 54, 841, DateTimeKind.Local).AddTicks(2038), null });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 21, 54, 841, DateTimeKind.Local).AddTicks(2105), null });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 21, 54, 841, DateTimeKind.Local).AddTicks(2108), null });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 21, 54, 855, DateTimeKind.Local).AddTicks(8239), null });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 21, 54, 855, DateTimeKind.Local).AddTicks(8917), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "QrImgSave",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyRoleMenu",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyMenu",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "Kdy.ImgSave",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 19, 40, 679, DateTimeKind.Local).AddTicks(5864), 0 });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 19, 40, 682, DateTimeKind.Local).AddTicks(5963), 0 });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 19, 40, 682, DateTimeKind.Local).AddTicks(6036), 0 });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 19, 40, 682, DateTimeKind.Local).AddTicks(6040), 0 });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 19, 40, 698, DateTimeKind.Local).AddTicks(8693), 0 });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "CreatedUserId" },
                values: new object[] { new DateTime(2020, 9, 5, 14, 19, 40, 698, DateTimeKind.Local).AddTicks(9639), 0 });
        }
    }
}
