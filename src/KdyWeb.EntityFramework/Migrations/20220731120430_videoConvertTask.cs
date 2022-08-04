using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class videoConvertTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KdyTask_ConvertOrder",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    CheckoutAmount = table.Column<decimal>(nullable: false),
                    ActualAmount = table.Column<decimal>(nullable: true),
                    ConvertOrderStatus = table.Column<byte>(nullable: false),
                    OrderContent = table.Column<string>(maxLength: 500, nullable: true),
                    OrderRemark = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyTask_ConvertOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyTask_VideoConvertTask",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    TaskName = table.Column<string>(maxLength: 50, nullable: true),
                    TaskType = table.Column<byte>(nullable: false),
                    TaskStatus = table.Column<byte>(nullable: false),
                    GiftPoints = table.Column<decimal>(nullable: false),
                    SourceLinkType = table.Column<byte>(nullable: false),
                    SourceLink = table.Column<string>(maxLength: 500, nullable: true),
                    SourceLinkExt = table.Column<string>(maxLength: 50, nullable: true),
                    TaskRemark = table.Column<string>(maxLength: 500, nullable: true),
                    TakeUserId = table.Column<long>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyTask_VideoConvertTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyTask_ConvertOrderDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    OrderId = table.Column<long>(nullable: false),
                    TaskId = table.Column<long>(nullable: false),
                    TaskName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyTask_ConvertOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KdyTask_ConvertOrderDetail_KdyTask_ConvertOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "KdyTask_ConvertOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_KdyTask_ConvertOrderDetail_OrderId",
                table: "KdyTask_ConvertOrderDetail",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KdyTask_ConvertOrderDetail");

            migrationBuilder.DropTable(
                name: "KdyTask_VideoConvertTask");

            migrationBuilder.DropTable(
                name: "KdyTask_ConvertOrder");

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
