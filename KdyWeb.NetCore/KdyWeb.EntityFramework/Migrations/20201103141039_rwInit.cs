using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class rwInit : Migration
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KdyRoleMenu_KdyRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "KdyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KdyUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KdyRole",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsActivate", "KdyRoleType", "ModifyTime", "ModifyUserId" },
                values: new object[] { 1, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)1, null, null });

            migrationBuilder.InsertData(
                table: "KdyRole",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsActivate", "KdyRoleType", "ModifyTime", "ModifyUserId" },
                values: new object[] { 2, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)5, null, null });

            migrationBuilder.InsertData(
                table: "KdyRole",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsActivate", "KdyRoleType", "ModifyTime", "ModifyUserId" },
                values: new object[] { 3, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)10, null, null });

            migrationBuilder.InsertData(
                table: "KdyRole",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsActivate", "KdyRoleType", "ModifyTime", "ModifyUserId" },
                values: new object[] { 4, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, (byte)15, null, null });

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "KdyRoleId", "ModifyTime", "ModifyUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[] { 2, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, null, "123456@qq.com", "test", "普通用户测试", "496ec666bef4a074ac39915dfb645e51" });

            migrationBuilder.InsertData(
                table: "KdyUser",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "KdyRoleId", "ModifyTime", "ModifyUserId", "PhoneNumber", "UserEmail", "UserName", "UserNick", "UserPwd" },
                values: new object[] { 1, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, null, "137651076@qq.com", "admin", "管理员", "496ec666bef4a074ac39915dfb645e51" });

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
                name: "KdyMenu");

            migrationBuilder.DropTable(
                name: "KdyRole");
        }
    }
}
