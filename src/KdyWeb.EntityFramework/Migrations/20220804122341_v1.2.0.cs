using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class v120 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoSeriesList",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoSeriesList",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoSeries",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoSeries",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoMainInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoMainInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoMain",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoMain",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoHistory",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoHistory",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoEpisodeGroup",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoEpisodeGroup",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoEpisode",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoEpisode",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoDownInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoDownInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "VideoDanMu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "VideoDanMu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "UserSubscribe",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "UserSubscribe",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "UserHistory",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "UserHistory",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "RecurrentUrlConfig",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "RecurrentUrlConfig",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "QrImgSave",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "QrImgSave",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "PageSearchConfig",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "PageSearchConfig",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.Wait",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.Wait",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.UserHistory",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.UserHistory",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.User",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.User",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.Subscribe",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.Subscribe",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.SeriesList",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.SeriesList",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.Series",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.Series",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.Main",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.Main",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.Episode",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.Episode",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Old.SearchSys.DanMu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Old.SearchSys.DanMu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "KdyUser",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "KdyUser",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "KdyRoleMenu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "KdyRoleMenu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "KdyRole",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "KdyRole",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "KdyMenu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "KdyMenu",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Kdy.ImgSave",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "Kdy.ImgSave",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "FeedBackInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "FeedBackInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "DouBanInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyUserName",
                table: "DouBanInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CloudParse_User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    ModifyUserName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(maxLength: 100, nullable: true),
                    UserNick = table.Column<string>(maxLength: 50, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 100, nullable: true),
                    UserQq = table.Column<string>(maxLength: 15, nullable: true),
                    UserPwd = table.Column<string>(maxLength: 50, nullable: true),
                    UserStatus = table.Column<int>(nullable: false),
                    SelfApiUrl = table.Column<string>(maxLength: 150, nullable: true),
                    IsHoldLink = table.Column<bool>(nullable: false),
                    HoldLinkHost = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudParse_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyTask_ConvertOrder",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    ModifyUserName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    CheckoutAmount = table.Column<decimal>(nullable: false),
                    ActualAmount = table.Column<decimal>(nullable: true),
                    ConvertOrderStatus = table.Column<byte>(nullable: false),
                    OrderContent = table.Column<string>(maxLength: 500, nullable: true),
                    OrderRemark = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyTask_ConvertOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KdyTask_VideoConvertTask",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    ModifyUserName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    TaskName = table.Column<string>(maxLength: 50, nullable: true),
                    TaskType = table.Column<byte>(nullable: false),
                    TaskStatus = table.Column<byte>(nullable: false),
                    GiftPoints = table.Column<decimal>(nullable: false),
                    SourceLinkType = table.Column<byte>(nullable: false),
                    SourceLink = table.Column<string>(maxLength: 500, nullable: true),
                    SourceLinkExt = table.Column<string>(maxLength: 50, nullable: true),
                    TaskRemark = table.Column<string>(maxLength: 500, nullable: true),
                    TakeUserId = table.Column<long>(nullable: true),
                    TakeUserName = table.Column<string>(maxLength: 50, nullable: true),
                    TakeTime = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyTask_VideoConvertTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudParse_UserChildren",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    ModifyUserName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    CookieType = table.Column<int>(nullable: false),
                    CookieInfo = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudParse_UserChildren", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloudParse_UserChildren_CloudParse_User_UserId",
                        column: x => x.UserId,
                        principalTable: "CloudParse_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KdyTask_ConvertOrderDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(nullable: true),
                    ModifyUserName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    OrderId = table.Column<long>(nullable: false),
                    TaskId = table.Column<long>(nullable: false),
                    TaskName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KdyTask_ConvertOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KdyTask_ConvertOrderDetail_KdyTask_ConvertOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "KdyTask_ConvertOrder",
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
                column: "KdyRoleId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "KdyUser",
                keyColumn: "Id",
                keyValue: 2L,
                column: "KdyRoleId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_CloudParse_UserChildren_UserId",
                table: "CloudParse_UserChildren",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KdyTask_ConvertOrderDetail_OrderId",
                table: "KdyTask_ConvertOrderDetail",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudParse_UserChildren");

            migrationBuilder.DropTable(
                name: "KdyTask_ConvertOrderDetail");

            migrationBuilder.DropTable(
                name: "KdyTask_VideoConvertTask");

            migrationBuilder.DropTable(
                name: "CloudParse_User");

            migrationBuilder.DropTable(
                name: "KdyTask_ConvertOrder");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoSeriesList");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoSeriesList");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoSeries");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoSeries");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoMainInfo");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoMainInfo");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoMain");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoMain");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoHistory");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoHistory");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoEpisodeGroup");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoEpisodeGroup");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoEpisode");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoEpisode");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoDownInfo");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoDownInfo");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "VideoDanMu");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "VideoDanMu");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "UserSubscribe");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "UserSubscribe");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "RecurrentUrlConfig");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "RecurrentUrlConfig");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "QrImgSave");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "QrImgSave");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "PageSearchConfig");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "PageSearchConfig");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.Wait");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.Wait");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.UserHistory");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.UserHistory");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.User");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.User");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.Subscribe");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.Subscribe");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.SeriesList");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.SeriesList");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.Series");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.Series");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.Main");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.Main");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.Episode");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.Episode");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Old.SearchSys.DanMu");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Old.SearchSys.DanMu");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "KdyUser");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "KdyUser");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "KdyRoleMenu");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "KdyRole");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "KdyRole");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "KdyMenu");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "KdyMenu");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Kdy.ImgSave");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "Kdy.ImgSave");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "FeedBackInfo");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "FeedBackInfo");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "DouBanInfo");

            migrationBuilder.DropColumn(
                name: "ModifyUserName",
                table: "DouBanInfo");

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
