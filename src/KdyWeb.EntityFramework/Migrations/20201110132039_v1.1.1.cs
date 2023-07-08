using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OldKeyId",
                table: "VideoMain",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OldEpId",
                table: "VideoEpisode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Main",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    VideoType = table.Column<string>(nullable: true),
                    MovieType = table.Column<string>(nullable: true),
                    IsEnd = table.Column<string>(nullable: true),
                    KeyWord = table.Column<string>(nullable: true),
                    ResultImg = table.Column<string>(nullable: true),
                    ResultUrl = table.Column<string>(nullable: true),
                    VideoContentFeature = table.Column<string>(nullable: true),
                    VideoDescribe = table.Column<string>(nullable: true),
                    VideoDouBan = table.Column<double>(nullable: false),
                    VideoYear = table.Column<int>(nullable: false),
                    VideoDetail = table.Column<string>(nullable: true),
                    VideoCasts = table.Column<string>(nullable: true),
                    VideoStatus = table.Column<int>(nullable: true),
                    VideoDirectors = table.Column<string>(nullable: true),
                    VideoCountries = table.Column<string>(nullable: true),
                    IsMatchInfo = table.Column<int>(nullable: false),
                    NarrateUrl = table.Column<string>(nullable: true),
                    BanVideoJumpUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Main", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Episode",
                columns: table => new
                {
                    EpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    KeyId = table.Column<int>(nullable: false),
                    EpisodeUrl = table.Column<string>(nullable: true),
                    EpisodeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Episode", x => x.EpId);
                    table.ForeignKey(
                        name: "FK_Old.SearchSys.Episode_Old.SearchSys.Main_KeyId",
                        column: x => x.KeyId,
                        principalTable: "Old.SearchSys.Main",
                        principalColumn: "KeyId",
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
                column: "KdyRoleId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                column: "KdyRoleId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Old.SearchSys.Episode_KeyId",
                table: "Old.SearchSys.Episode",
                column: "KeyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Old.SearchSys.Episode");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Main");

            migrationBuilder.DropColumn(
                name: "OldKeyId",
                table: "VideoMain");

            migrationBuilder.DropColumn(
                name: "OldEpId",
                table: "VideoEpisode");

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
