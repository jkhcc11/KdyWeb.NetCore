using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v110 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DouBanInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    VideoTitle = table.Column<string>(maxLength: 100, nullable: false),
                    DouBanInfoStatus = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    OldStatus = table.Column<string>(nullable: true),
                    Subtype = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    OldVideoType = table.Column<string>(nullable: true),
                    VideoYear = table.Column<int>(nullable: false),
                    VideoImg = table.Column<string>(maxLength: 200, nullable: false),
                    VideoCasts = table.Column<string>(maxLength: 200, nullable: true),
                    VideoDirectors = table.Column<string>(maxLength: 200, nullable: true),
                    VideoGenres = table.Column<string>(maxLength: 200, nullable: true),
                    VideoRating = table.Column<double>(nullable: false),
                    VideoSummary = table.Column<string>(nullable: true),
                    VideoDetailId = table.Column<string>(maxLength: 10, nullable: false),
                    VideoCountries = table.Column<string>(maxLength: 200, nullable: true),
                    RatingsCount = table.Column<int>(nullable: true),
                    CommentsCount = table.Column<int>(nullable: true),
                    ReviewsCount = table.Column<int>(nullable: true),
                    Aka = table.Column<string>(maxLength: 100, nullable: true),
                    ImdbStr = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DouBanInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    FeedBackInfoStatus = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    OriginalUrl = table.Column<string>(maxLength: 200, nullable: true),
                    DemandType = table.Column<int>(nullable: false),
                    VideoName = table.Column<string>(maxLength: 100, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kdy.ImgSave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    FileMd5 = table.Column<string>(maxLength: 32, nullable: true),
                    MainUrl = table.Column<string>(maxLength: 200, nullable: true),
                    OneUrl = table.Column<string>(maxLength: 200, nullable: true),
                    TwoUrl = table.Column<string>(maxLength: 200, nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    UserNick = table.Column<string>(nullable: true),
                    UrlBack = table.Column<string>(maxLength: 200, nullable: true),
                    Urls = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kdy.ImgSave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    ControllerName = table.Column<string>(maxLength: 100, nullable: true),
                    ActionName = table.Column<string>(maxLength: 100, nullable: true),
                    MenuName = table.Column<string>(maxLength: 100, nullable: true),
                    OrderBy = table.Column<int>(nullable: false),
                    IsNav = table.Column<bool>(nullable: false),
                    NavIcon = table.Column<string>(maxLength: 50, nullable: true),
                    IsActivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    KdyRoleType = table.Column<byte>(nullable: false, defaultValue: (byte)1),
                    IsActivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrImgSave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    FileMd5 = table.Column<string>(maxLength: 32, nullable: true),
                    ImgPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrImgSave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    KeyId = table.Column<long>(nullable: false),
                    EpId = table.Column<long>(nullable: false),
                    EpName = table.Column<string>(nullable: true),
                    VodName = table.Column<string>(nullable: true),
                    VodUrl = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistory", x => x.Id);
                });

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
                name: "KdyRoleMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    MenuId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsActivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyRoleMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KdyRoleMenu_KdyMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "KdyMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KdyRoleMenu_KdyRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "KdyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KdyUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    ModifyUserId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(maxLength: 100, nullable: true),
                    UserNick = table.Column<string>(maxLength: 50, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 100, nullable: true),
                    UserPwd = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 11, nullable: true),
                    KdyRoleId = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KdyUser_KdyRole_KdyRoleId",
                        column: x => x.KdyRoleId,
                        principalTable: "KdyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "KdyRole",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsActivate", "KdyRoleType", "ModifyTime", "ModifyUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)1, null, null },
                    { 2, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)5, null, null },
                    { 3, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)10, null, null },
                    { 4, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)15, null, null }
                });

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "KdyRoleId", "ModifyTime", "ModifyUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[] { 2L, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, null, "123456@qq.com", "test", "普通用户测试", "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "KdyRoleId", "ModifyTime", "ModifyUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[] { 1L, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, null, "137651076@qq.com", "admin", "管理员", "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.CreateIndex(
                name: "IX_KdyRoleMenu_MenuId",
                table: "KdyRoleMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_KdyRoleMenu_RoleId",
                table: "KdyRoleMenu",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_KdyUser_KdyRoleId",
                table: "KdyUser",
                column: "KdyRoleId");

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
                name: "DouBanInfo");

            migrationBuilder.DropTable(
                name: "FeedBackInfo");

            migrationBuilder.DropTable(
                name: "Kdy.ImgSave");

            migrationBuilder.DropTable(
                name: "KdyRoleMenu");

            migrationBuilder.DropTable(
                name: "KdyUser");

            migrationBuilder.DropTable(
                name: "QrImgSave");

            migrationBuilder.DropTable(
                name: "UserHistory");

            migrationBuilder.DropTable(
                name: "VideoEpisode");

            migrationBuilder.DropTable(
                name: "VideoMainInfo");

            migrationBuilder.DropTable(
                name: "KdyMenu");

            migrationBuilder.DropTable(
                name: "KdyRole");

            migrationBuilder.DropTable(
                name: "VideoEpisodeGroup");

            migrationBuilder.DropTable(
                name: "VideoMain");
        }
    }
}
