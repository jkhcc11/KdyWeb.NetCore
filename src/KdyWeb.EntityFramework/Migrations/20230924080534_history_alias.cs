using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class history_alias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "CloudParse_ParseRecordHistory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "CloudParse_ParseRecordHistory");
        }
    }
}
