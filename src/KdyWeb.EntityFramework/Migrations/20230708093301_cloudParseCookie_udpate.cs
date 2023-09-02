using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class cloudParseCookie_udpate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookieType",
                table: "CloudParse_SubAccount");

            migrationBuilder.AddColumn<long>(
                name: "CloudParseCookieTypeId",
                table: "CloudParse_SubAccount",
                nullable: false,
                defaultValue: 0L);

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
                    ShowText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudParse_CookieType", x => x.Id);
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

            migrationBuilder.DropIndex(
                name: "IX_CloudParse_SubAccount_CloudParseCookieTypeId",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropColumn(
                name: "CloudParseCookieTypeId",
                table: "CloudParse_SubAccount");

            migrationBuilder.AddColumn<int>(
                name: "CookieType",
                table: "CloudParse_SubAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
