using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v115 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageSearchConfig",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    HostName = table.Column<string>(maxLength: 50, nullable: false),
                    HostRemark = table.Column<string>(maxLength: 200, nullable: true),
                    BaseHost = table.Column<string>(maxLength: 50, nullable: false),
                    OtherHost = table.Column<string>(maxLength: 200, nullable: true),
                    UserAgent = table.Column<string>(maxLength: 300, nullable: true),
                    ConfigHttpMethod = table.Column<byte>(nullable: false),
                    SearchPath = table.Column<string>(maxLength: 100, nullable: true),
                    SearchData = table.Column<string>(maxLength: 100, nullable: true),
                    ServiceFullName = table.Column<string>(maxLength: 150, nullable: false),
                    SearchXpath = table.Column<string>(maxLength: 100, nullable: false),
                    EndXpath = table.Column<string>(maxLength: 100, nullable: true),
                    NotEndKey = table.Column<string>(maxLength: 50, nullable: true),
                    ImgAttr = table.Column<string>(nullable: true),
                    NameAttr = table.Column<string>(nullable: true),
                    DetailXpath = table.Column<string>(maxLength: 100, nullable: false),
                    DetailImgXpath = table.Column<string>(maxLength: 100, nullable: true),
                    DetailNameXpath = table.Column<string>(maxLength: 100, nullable: true),
                    DetailEndXpath = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSearchConfig", x => x.Id);
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
                name: "PageSearchConfig");

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
