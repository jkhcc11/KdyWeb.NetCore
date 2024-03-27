using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class v170 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KdyBase_KdyMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ParentMenuId = table.Column<long>(type: "bigint", nullable: false),
                    MenuUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MenuName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RouteName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IconPrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsRootPath = table.Column<bool>(type: "bit", nullable: false),
                    IsCache = table.Column<bool>(type: "bit", nullable: false),
                    LocalFilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    OrderBy = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyBase_KdyMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyBase_KdyRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleFlag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleRemark = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyBase_KdyRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyBase_KdyRoleMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MenuId = table.Column<long>(type: "bigint", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActivate = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyBase_KdyRoleMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KdyBase_KdyRoleMenu_KdyBase_KdyMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "KdyBase_KdyMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KdyBase_KdyRoleMenu_MenuId",
                table: "KdyBase_KdyRoleMenu",
                column: "MenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KdyBase_KdyRole");

            migrationBuilder.DropTable(
                name: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropTable(
                name: "KdyBase_KdyMenu");
        }
    }
}
