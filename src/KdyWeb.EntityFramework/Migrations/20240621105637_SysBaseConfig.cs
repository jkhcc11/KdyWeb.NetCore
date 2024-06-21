using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class SysBaseConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysBaseConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ConfigType = table.Column<byte>(type: "tinyint", nullable: false),
                    ConfigStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    ConfigName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
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
                    table.PrimaryKey("PK_SysBaseConfig", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysBaseConfig");
        }
    }
}
