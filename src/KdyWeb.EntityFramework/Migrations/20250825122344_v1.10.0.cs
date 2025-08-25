using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class v1100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KdyBase_KdyMsgCenter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MsgType = table.Column<int>(type: "int", nullable: false),
                    BusinessType = table.Column<int>(type: "int", nullable: false),
                    MsgTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MsgContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_KdyBase_KdyMsgCenter", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KdyBase_KdyMsgCenter");
        }
    }
}
