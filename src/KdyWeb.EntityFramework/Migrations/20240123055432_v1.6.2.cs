using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class v162 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSeriesList_VideoMain_KeyId",
                table: "VideoSeriesList");

            migrationBuilder.DropIndex(
                name: "IX_VideoSeriesList_KeyId",
                table: "VideoSeriesList");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "CloudParse_User",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "RelationalUserIds",
                table: "CloudParse_SubAccount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoSeriesList_KeyId",
                table: "VideoSeriesList",
                column: "KeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSeriesList_VideoMain_KeyId",
                table: "VideoSeriesList",
                column: "KeyId",
                principalTable: "VideoMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSeriesList_VideoMain_KeyId",
                table: "VideoSeriesList");

            migrationBuilder.DropIndex(
                name: "IX_VideoSeriesList_KeyId",
                table: "VideoSeriesList");

            migrationBuilder.DropColumn(
                name: "RelationalUserIds",
                table: "CloudParse_SubAccount");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "CloudParse_User",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoSeriesList_KeyId",
                table: "VideoSeriesList",
                column: "KeyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSeriesList_VideoMain_KeyId",
                table: "VideoSeriesList",
                column: "KeyId",
                principalTable: "VideoMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
