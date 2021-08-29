using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class series : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Old.SearchSys.DanMu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DTime = table.Column<float>(nullable: false),
                    DColor = table.Column<string>(nullable: true),
                    DMsg = table.Column<string>(nullable: true),
                    DVideoId = table.Column<string>(nullable: true),
                    DMode = table.Column<int>(nullable: false),
                    DSize = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.DanMu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Series",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    SeriesName = table.Column<string>(nullable: true),
                    SeriesImg = table.Column<string>(nullable: true),
                    SeriesRemark = table.Column<string>(nullable: true),
                    OrderBy = table.Column<int>(nullable: false),
                    LiveUrl = table.Column<string>(nullable: true),
                    SeriesDesUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.SeriesList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    SeriesId = table.Column<int>(nullable: false),
                    KeyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.SeriesList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Wait",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    DouBanId = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Wait", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoSeries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    SeriesName = table.Column<string>(nullable: true),
                    SeriesImg = table.Column<string>(nullable: true),
                    SeriesRemark = table.Column<string>(nullable: true),
                    OrderBy = table.Column<int>(nullable: false),
                    LiveUrl = table.Column<string>(nullable: true),
                    SeriesDesUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoSeriesList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    SeriesId = table.Column<long>(nullable: false),
                    KeyId = table.Column<long>(nullable: false),
                    OldKeyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoSeriesList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoSeriesList_VideoMain_KeyId",
                        column: x => x.KeyId,
                        principalTable: "VideoMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoSeriesList_VideoSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "VideoSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                columns: new[] { "KdyRoleId", "UserPwd" },
                values: new object[] { 3, "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "KdyRoleId", "UserPwd" },
                values: new object[] { 1, "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.CreateIndex(
                name: "IX_VideoSeriesList_KeyId",
                table: "VideoSeriesList",
                column: "KeyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoSeriesList_SeriesId",
                table: "VideoSeriesList",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Old.SearchSys.DanMu");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Series");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.SeriesList");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Wait");

            migrationBuilder.DropTable(
                name: "VideoSeriesList");

            migrationBuilder.DropTable(
                name: "VideoSeries");

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
                columns: new[] { "KdyRoleId", "UserPwd" },
                values: new object[] { 3, "fc895814516f2588fefc6ca371c61243" });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "KdyRoleId", "UserPwd" },
                values: new object[] { 1, "fc895814516f2588fefc6ca371c61243" });
        }
    }
}
