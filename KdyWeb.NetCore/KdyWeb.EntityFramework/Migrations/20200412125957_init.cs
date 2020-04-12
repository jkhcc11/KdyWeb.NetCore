using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KdyMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: false),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    ControllerName = table.Column<string>(maxLength: 100, nullable: true),
                    ActionName = table.Column<string>(maxLength: 100, nullable: true),
                    MenuName = table.Column<string>(maxLength: 100, nullable: true),
                    OrderBy = table.Column<int>(nullable: false),
                    IsNav = table.Column<bool>(nullable: false),
                    NavIcon = table.Column<string>(maxLength: 50, nullable: true),
                    IsActivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: false),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    KdyRoleType = table.Column<byte>(nullable: false, defaultValue: (byte)1),
                    IsActivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyRoleMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: false),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    MenuId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsActivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyRoleMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KdyRoleMenu_KdyMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "KdyMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KdyRoleMenu_KdyRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "KdyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KdyUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: false),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(maxLength: 100, nullable: true),
                    UserNick = table.Column<string>(maxLength: 50, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 100, nullable: true),
                    UserPwd = table.Column<string>(maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 11, nullable: true),
                    KdyRoleId = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KdyUser_KdyRole_KdyRoleId",
                        column: x => x.KdyRoleId,
                        principalTable: "KdyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KdyRole",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsActivate", "IsDelete", "KdyRoleType", "ModifyTime", "ModifyUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 4, 12, 20, 59, 56, 896, DateTimeKind.Local).AddTicks(1109), 0, true, false, (byte)1, null, null },
                    { 2, new DateTime(2020, 4, 12, 20, 59, 56, 898, DateTimeKind.Local).AddTicks(3113), 0, true, false, (byte)5, null, null },
                    { 3, new DateTime(2020, 4, 12, 20, 59, 56, 898, DateTimeKind.Local).AddTicks(3163), 0, true, false, (byte)10, null, null },
                    { 4, new DateTime(2020, 4, 12, 20, 59, 56, 898, DateTimeKind.Local).AddTicks(3166), 0, true, false, (byte)15, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_KdyRoleMenu_MenuId",
                table: "KdyRoleMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_KdyRoleMenu_RoleId",
                table: "KdyRoleMenu",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_KdyUser_KdyRoleId",
                table: "KdyUser",
                column: "KdyRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KdyRoleMenu");

            migrationBuilder.DropTable(
                name: "KdyUser");

            migrationBuilder.DropTable(
                name: "KdyMenu");

            migrationBuilder.DropTable(
                name: "KdyRole");
        }
    }
}
