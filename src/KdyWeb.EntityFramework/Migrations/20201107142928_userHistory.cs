using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class userHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2);

            //删除 再加上去
            migrationBuilder.DropColumn("Id", "KdyUser");
            migrationBuilder.AddColumn<long>("Id", "KdyUser", nullable: false);

            //migrationBuilder.AlterColumn<long>(
            //    name: "Id",
            //    table: "KdyUser",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "UserHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    OldUserId = table.Column<int>(nullable: false),
                    OldKeyId = table.Column<int>(nullable: false),
                    OldEpId = table.Column<int>(nullable: false),
                    KeyId = table.Column<long>(nullable: false),
                    EpId = table.Column<long>(nullable: false),
                    EpName = table.Column<string>(nullable: true),
                    VodName = table.Column<string>(nullable: true),
                    VodUrl = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistory", x => x.Id);
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

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "KdyRoleId", "ModifyTime", "ModifyUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[,]
                {
                    { 1L, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, null, "137651076@qq.com", "admin", "管理员", "496ec666bef4a074ac39915dfb645e51" },
                    { 2L, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, null, "123456@qq.com", "test", "普通用户测试", "496ec666bef4a074ac39915dfb645e51" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHistory");

            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "KdyUser",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(long))
            //    .Annotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "KdyRoleId", "ModifyTime", "ModifyUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[,]
                {
                    { 1, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, null, "137651076@qq.com", "admin", "管理员", "496ec666bef4a074ac39915dfb645e51" },
                    { 2, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, null, "123456@qq.com", "test", "普通用户测试", "496ec666bef4a074ac39915dfb645e51" }
                });
        }
    }
}
