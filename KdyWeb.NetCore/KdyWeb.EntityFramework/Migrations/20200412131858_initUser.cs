using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class initUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsDelete", "KdyRoleId", "ModifyTime", "ModifyUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 4, 12, 21, 18, 58, 124, DateTimeKind.Local).AddTicks(2810), 0, false, 3, null, null, null, "137651076@qq.com", "admin", "管理员", "496ec666bef4a074ac39915dfb645e51" },
                    { 2, new DateTime(2020, 4, 12, 21, 18, 58, 124, DateTimeKind.Local).AddTicks(3567), 0, false, 1, null, null, null, "123456@qq.com", "test", "普通用户测试", "496ec666bef4a074ac39915dfb645e51" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 15, 53, 254, DateTimeKind.Local).AddTicks(9860));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 15, 53, 257, DateTimeKind.Local).AddTicks(3049));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 15, 53, 257, DateTimeKind.Local).AddTicks(3100));

            migrationBuilder.UpdateData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2020, 4, 12, 21, 15, 53, 257, DateTimeKind.Local).AddTicks(3103));
        }
    }
}
