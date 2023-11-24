using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class v161 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VodImgUrl",
                table: "VideoHistory");

            migrationBuilder.AlterColumn<string>(
                name: "VodName",
                table: "VideoHistory",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EpName",
                table: "VideoHistory",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "CloudParse_User",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "CloudParse_User");

            migrationBuilder.AlterColumn<string>(
                name: "VodName",
                table: "VideoHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EpName",
                table: "VideoHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VodImgUrl",
                table: "VideoHistory",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
