using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class role_menu_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "IconPrefix",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "IsCache",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "IsRootPath",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "LocalFilePath",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "MenuName",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "MenuUrl",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "RouteName",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.RenameColumn(
                name: "ParentMenuId",
                table: "KdyBase_KdyRoleMenu",
                newName: "MenuId");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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

            migrationBuilder.CreateIndex(
                name: "IX_KdyBase_KdyRoleMenu_MenuId",
                table: "KdyBase_KdyRoleMenu",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_KdyBase_KdyRoleMenu_KdyBase_KdyMenu_MenuId",
                table: "KdyBase_KdyRoleMenu",
                column: "MenuId",
                principalTable: "KdyBase_KdyMenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KdyBase_KdyRoleMenu_KdyBase_KdyMenu_MenuId",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.DropTable(
                name: "KdyBase_KdyMenu");

            migrationBuilder.DropIndex(
                name: "IX_KdyBase_KdyRoleMenu_MenuId",
                table: "KdyBase_KdyRoleMenu");

            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "KdyBase_KdyRoleMenu",
                newName: "ParentMenuId");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconPrefix",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCache",
                table: "KdyBase_KdyRoleMenu",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRootPath",
                table: "KdyBase_KdyRoleMenu",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocalFilePath",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuName",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MenuUrl",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RouteName",
                table: "KdyBase_KdyRoleMenu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
