using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v141 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KdyRole",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "GameDown_Main",
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
                    GameName = table.Column<string>(maxLength: 200, nullable: true),
                    ChineseName = table.Column<string>(maxLength: 200, nullable: true),
                    GameSize = table.Column<string>(maxLength: 50, nullable: true),
                    GameVersion = table.Column<string>(maxLength: 50, nullable: true),
                    GameCovert = table.Column<string>(maxLength: 300, nullable: true),
                    LogoUrl = table.Column<string>(maxLength: 300, nullable: true),
                    ScreenCapture = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(maxLength: 300, nullable: true),
                    SourceMd5 = table.Column<string>(maxLength: 32, nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 300, nullable: true),
                    TorrentUrl = table.Column<string>(maxLength: 300, nullable: true),
                    Magnet = table.Column<string>(maxLength: 300, nullable: true),
                    DetailId = table.Column<string>(maxLength: 32, nullable: true),
                    UserHash = table.Column<string>(maxLength: 50, nullable: true),
                    DownList = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDown_Main", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameDown_Main");

            migrationBuilder.InsertData(
                table: "KdyRole",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "CreatedUserName", "IsActivate", "KdyRoleType", "ModifyTime", "ModifyUserId", "ModifyUserName" },
                values: new object[,]
                {
                    { 1, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, (byte)1, null, null, null },
                    { 2, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, (byte)5, null, null, null },
                    { 3, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, (byte)10, null, null, null },
                    { 4, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, (byte)15, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "CreatedUserName", "KdyRoleId", "ModifyTime", "ModifyUserId", "ModifyUserName", "OldUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1, null, null, null, 0, null, "123456@qq.com", "test", "普通用户测试", "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "CreatedUserName", "KdyRoleId", "ModifyTime", "ModifyUserId", "ModifyUserName", "OldUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 3, null, null, null, 0, null, "137651076@qq.com", "admin", "管理员", "496ec666bef4a074ac39915dfb645e51" });
        }
    }
}
