using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class vodManagerRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KdyTask_VodManagerRecord",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    ModifyUserName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    CheckoutAmount = table.Column<decimal>(nullable: false),
                    ActualAmount = table.Column<decimal>(nullable: false),
                    RecordType = table.Column<int>(nullable: false),
                    IsCheckout = table.Column<bool>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    BusinessId = table.Column<long>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyTask_VodManagerRecord", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "KdyRoleType",
                value: (byte)1);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "KdyRoleType",
                value: (byte)5);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "KdyRoleType",
                value: (byte)10);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "KdyRoleType",
                value: (byte)15);

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1L,
                column: "KdyRoleId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                column: "KdyRoleId",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KdyTask_VodManagerRecord");

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "KdyRoleType",
                value: (byte)1);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "KdyRoleType",
                value: (byte)5);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "KdyRoleType",
                value: (byte)10);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "KdyRoleType",
                value: (byte)15);

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1L,
                column: "KdyRoleId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                column: "KdyRoleId",
                value: 1);
        }
    }
}
