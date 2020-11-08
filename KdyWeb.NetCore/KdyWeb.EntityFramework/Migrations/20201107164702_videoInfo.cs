using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class videoInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoMain",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Subtype = table.Column<byte>(nullable: false),
                    OrderBy = table.Column<int>(nullable: false),
                    IsEnd = table.Column<bool>(nullable: false),
                    KeyWord = table.Column<string>(maxLength: 100, nullable: false),
                    VideoImg = table.Column<string>(maxLength: 200, nullable: false),
                    IsMatchInfo = table.Column<bool>(nullable: false),
                    VideoMainStatus = table.Column<byte>(nullable: false),
                    Aka = table.Column<string>(maxLength: 100, nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 280, nullable: true),
                    VideoContentFeature = table.Column<string>(maxLength: 32, nullable: true),
                    VideoDouBan = table.Column<double>(nullable: false),
                    VideoYear = table.Column<int>(nullable: false),
                    VideoInfoUrl = table.Column<string>(maxLength: 280, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoMain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoEpisodeGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    GroupName = table.Column<string>(maxLength: 50, nullable: false),
                    EpisodeGroupStatus = table.Column<byte>(nullable: false),
                    EpisodeGroupType = table.Column<byte>(nullable: false),
                    MainId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoEpisodeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoEpisodeGroup_VideoMain_MainId",
                        column: x => x.MainId,
                        principalTable: "VideoMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoMainInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    VideoGenres = table.Column<string>(maxLength: 200, nullable: true),
                    VideoSummary = table.Column<string>(nullable: true),
                    VideoCasts = table.Column<string>(maxLength: 200, nullable: true),
                    VideoDirectors = table.Column<string>(maxLength: 200, nullable: true),
                    VideoCountries = table.Column<string>(maxLength: 200, nullable: true),
                    NarrateUrl = table.Column<string>(maxLength: 280, nullable: true),
                    BanVideoJumpUrl = table.Column<string>(maxLength: 280, nullable: true),
                    MainId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoMainInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoMainInfo_VideoMain_MainId",
                        column: x => x.MainId,
                        principalTable: "VideoMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoEpisode",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    EpisodeUrl = table.Column<string>(maxLength: 280, nullable: false),
                    EpisodeName = table.Column<string>(maxLength: 80, nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    OrderBy = table.Column<int>(nullable: false),
                    EpisodeGroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoEpisode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoEpisode_VideoEpisodeGroup_EpisodeGroupId",
                        column: x => x.EpisodeGroupId,
                        principalTable: "VideoEpisodeGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_VideoEpisode_EpisodeGroupId",
                table: "VideoEpisode",
                column: "EpisodeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoEpisodeGroup_MainId",
                table: "VideoEpisodeGroup",
                column: "MainId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoMainInfo_MainId",
                table: "VideoMainInfo",
                column: "MainId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoEpisode");

            migrationBuilder.DropTable(
                name: "VideoMainInfo");

            migrationBuilder.DropTable(
                name: "VideoEpisodeGroup");

            migrationBuilder.DropTable(
                name: "VideoMain");

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
