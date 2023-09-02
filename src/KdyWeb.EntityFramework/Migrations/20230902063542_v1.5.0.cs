using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v150 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudParse_UserChildren_CloudParse_User_UserId",
                table: "CloudParse_UserChildren");

            migrationBuilder.DropIndex(
                name: "IX_CloudParse_UserChildren_UserId",
                table: "CloudParse_UserChildren");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserNick",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserPwd",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserQq",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "CookieType",
                table: "CloudParse_UserChildren");

            migrationBuilder.RenameTable(
                name: "CloudParse_UserChildren",
                newName: "CloudParse_SubAccount");

            migrationBuilder.AddColumn<string>(
                name: "ApiToken",
                table: "CloudParse_User",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "CloudParse_User",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "CloudParse_SubAccount",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessId",
                table: "CloudParse_SubAccount",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CloudParseCookieTypeId",
                table: "CloudParse_SubAccount",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OldSubAccountInfo",
                table: "CloudParse_SubAccount",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CloudParse_SubAccount",
                table: "CloudParse_SubAccount",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CloudParse_CookieType",
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
                    ShowText = table.Column<string>(maxLength: 50, nullable: true),
                    BusinessFlag = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudParse_CookieType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudParse_ServerCookie",
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
                    SubAccountId = table.Column<long>(nullable: false),
                    ServerIp = table.Column<string>(maxLength: 20, nullable: true),
                    CookieInfo = table.Column<string>(maxLength: 1000, nullable: true),
                    ServerCookieStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudParse_ServerCookie", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudParse_SubAccount_CloudParseCookieTypeId",
                table: "CloudParse_SubAccount",
                column: "CloudParseCookieTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudParse_SubAccount_CloudParse_CookieType_CloudParseCookieTypeId",
                table: "CloudParse_SubAccount",
                column: "CloudParseCookieTypeId",
                principalTable: "CloudParse_CookieType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudParse_SubAccount_CloudParse_CookieType_CloudParseCookieTypeId",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropTable(
                name: "CloudParse_CookieType");

            migrationBuilder.DropTable(
                name: "CloudParse_ServerCookie");

            migrationBuilder.DropTable(
                name: "GameDown_Main");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CloudParse_SubAccount",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropIndex(
                name: "IX_CloudParse_SubAccount_CloudParseCookieTypeId",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropColumn(
                name: "ApiToken",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropColumn(
                name: "CloudParseCookieTypeId",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropColumn(
                name: "OldSubAccountInfo",
                table: "CloudParse_SubAccount");

            migrationBuilder.RenameTable(
                name: "CloudParse_SubAccount",
                newName: "CloudParse_UserChildren");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "CloudParse_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CloudParse_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNick",
                table: "CloudParse_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPwd",
                table: "CloudParse_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserQq",
                table: "CloudParse_User",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CloudParse_UserChildren",
                type: "int",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CookieType",
                table: "CloudParse_UserChildren",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CloudParse_UserChildren",
                table: "CloudParse_UserChildren",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_CloudParse_UserChildren_UserId",
                table: "CloudParse_UserChildren",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudParse_UserChildren_CloudParse_User_UserId",
                table: "CloudParse_UserChildren",
                column: "UserId",
                principalTable: "CloudParse_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
