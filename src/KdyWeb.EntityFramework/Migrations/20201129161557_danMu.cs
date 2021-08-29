using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class danMu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KdyRoleMenu_KdyMenu_MenuId",
                table: "KdyRoleMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_KdyRoleMenu_KdyRole_RoleId",
                table: "KdyRoleMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_KdyUser_KdyRole_KdyRoleId",
                table: "KdyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoEpisode_VideoEpisodeGroup_EpisodeGroupId",
                table: "VideoEpisode");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoEpisodeGroup_VideoMain_MainId",
                table: "VideoEpisodeGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoMainInfo_VideoMain_MainId",
                table: "VideoMainInfo");

            migrationBuilder.DropColumn(
                name: "OldEpId",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "OldKeyId",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "OldUserId",
                table: "UserHistory");

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "VideoMainInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "VideoMainInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "VideoMain",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "VideoMain",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldKeyId",
                table: "VideoMain",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "VideoEpisodeGroup",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "VideoEpisodeGroup",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                table: "VideoEpisodeGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "VideoEpisode",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "VideoEpisode",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldEpId",
                table: "VideoEpisode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "UserHistory",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "UserHistory",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "QrImgSave",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "QrImgSave",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "KdyUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "KdyUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldUserId",
                table: "KdyUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "KdyRoleMenu",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "KdyRoleMenu",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "KdyRole",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "KdyRole",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "KdyMenu",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "KdyMenu",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "Kdy.ImgSave",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "Kdy.ImgSave",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "FeedBackInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "FeedBackInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyUserId",
                table: "DouBanInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedUserId",
                table: "DouBanInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                    VideoDouBan = table.Column<double>(nullable: true),
                    VideoYear = table.Column<int>(nullable: true),
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
                name: "Old.SearchSys.Subscribe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ObjId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Subscribe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    UserNick = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(nullable: true),
                    UserPwd = table.Column<string>(nullable: true),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.UserHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    KeyId = table.Column<int>(nullable: false),
                    EpId = table.Column<int>(nullable: false),
                    EpName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    VodName = table.Column<string>(nullable: true),
                    VodUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.UserHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscribe",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    BusinessId = table.Column<long>(nullable: false),
                    BusinessFeature = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(nullable: true),
                    UserSubscribeType = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscribe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoDanMu",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DTime = table.Column<float>(nullable: false),
                    DColor = table.Column<string>(maxLength: 10, nullable: true),
                    Msg = table.Column<string>(maxLength: 200, nullable: true),
                    EpId = table.Column<long>(nullable: false),
                    DMode = table.Column<int>(nullable: false),
                    DSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoDanMu", x => x.Id);
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
                columns: new[] { "CreatedTime", "KdyRoleId", "UserPwd" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "6e4a32d64eaab4f86a61a4c73f76f37b" });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedTime", "KdyRoleId", "UserPwd" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "6e4a32d64eaab4f86a61a4c73f76f37b" });

            migrationBuilder.CreateIndex(
                name: "IX_Old.SearchSys.Episode_KeyId",
                table: "Old.SearchSys.Episode",
                column: "KeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_KdyRoleMenu_KdyMenu_MenuId",
                table: "KdyRoleMenu",
                column: "MenuId",
                principalTable: "KdyMenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KdyRoleMenu_KdyRole_RoleId",
                table: "KdyRoleMenu",
                column: "RoleId",
                principalTable: "KdyRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KdyUser_KdyRole_KdyRoleId",
                table: "KdyUser",
                column: "KdyRoleId",
                principalTable: "KdyRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoEpisode_VideoEpisodeGroup_EpisodeGroupId",
                table: "VideoEpisode",
                column: "EpisodeGroupId",
                principalTable: "VideoEpisodeGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoEpisodeGroup_VideoMain_MainId",
                table: "VideoEpisodeGroup",
                column: "MainId",
                principalTable: "VideoMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoMainInfo_VideoMain_MainId",
                table: "VideoMainInfo",
                column: "MainId",
                principalTable: "VideoMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KdyRoleMenu_KdyMenu_MenuId",
                table: "KdyRoleMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_KdyRoleMenu_KdyRole_RoleId",
                table: "KdyRoleMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_KdyUser_KdyRole_KdyRoleId",
                table: "KdyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoEpisode_VideoEpisodeGroup_EpisodeGroupId",
                table: "VideoEpisode");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoEpisodeGroup_VideoMain_MainId",
                table: "VideoEpisodeGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoMainInfo_VideoMain_MainId",
                table: "VideoMainInfo");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Episode");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Subscribe");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.User");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.UserHistory");

            migrationBuilder.DropTable(
                name: "UserSubscribe");

            migrationBuilder.DropTable(
                name: "VideoDanMu");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Main");

            migrationBuilder.DropColumn(
                name: "OldKeyId",
                table: "VideoMain");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "VideoEpisodeGroup");

            migrationBuilder.DropColumn(
                name: "OldEpId",
                table: "VideoEpisode");

            migrationBuilder.DropColumn(
                name: "OldUserId",
                table: "KdyUser");

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "VideoMainInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "VideoMainInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "VideoMain",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "VideoMain",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "VideoEpisodeGroup",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "VideoEpisodeGroup",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "VideoEpisode",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "VideoEpisode",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "UserHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "UserHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldEpId",
                table: "UserHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OldKeyId",
                table: "UserHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OldUserId",
                table: "UserHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "QrImgSave",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "QrImgSave",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "KdyUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "KdyRoleMenu",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyRoleMenu",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "KdyRole",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyRole",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "KdyMenu",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "KdyMenu",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "Kdy.ImgSave",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "Kdy.ImgSave",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "FeedBackInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "FeedBackInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyUserId",
                table: "DouBanInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserId",
                table: "DouBanInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

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
                columns: new[] { "CreatedTime", "KdyRoleId", "UserPwd" },
                values: new object[] { new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedTime", "KdyRoleId", "UserPwd" },
                values: new object[] { new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.AddForeignKey(
                name: "FK_KdyRoleMenu_KdyMenu_MenuId",
                table: "KdyRoleMenu",
                column: "MenuId",
                principalTable: "KdyMenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KdyRoleMenu_KdyRole_RoleId",
                table: "KdyRoleMenu",
                column: "RoleId",
                principalTable: "KdyRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KdyUser_KdyRole_KdyRoleId",
                table: "KdyUser",
                column: "KdyRoleId",
                principalTable: "KdyRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoEpisode_VideoEpisodeGroup_EpisodeGroupId",
                table: "VideoEpisode",
                column: "EpisodeGroupId",
                principalTable: "VideoEpisodeGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoEpisodeGroup_VideoMain_MainId",
                table: "VideoEpisodeGroup",
                column: "MainId",
                principalTable: "VideoMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoMainInfo_VideoMain_MainId",
                table: "VideoMainInfo",
                column: "MainId",
                principalTable: "VideoMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
