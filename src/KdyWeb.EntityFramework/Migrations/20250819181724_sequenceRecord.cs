using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KdyWeb.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class sequenceRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Old.SearchSys.DanMu");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Episode");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Series");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.SeriesList");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Subscribe");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.User");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.UserHistory");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Wait");

            migrationBuilder.DropTable(
                name: "Old.SearchSys.Main");

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                table: "SysBaseConfig",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SequenceRecord_GiftUserConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GiftUserType = table.Column<int>(type: "int", nullable: false),
                    UserShowName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    GiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiftEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiftPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VenuesId = table.Column<long>(type: "bigint", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceRecord_GiftUserConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SequenceRecord_SequenceUseRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserShowName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserNickName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiftUserType = table.Column<int>(type: "int", nullable: true),
                    VenuesId = table.Column<long>(type: "bigint", nullable: false),
                    VenuesShortName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceRecord_SequenceUseRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SequenceRecord_SequenceUserRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserNickName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserShowName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceRecord_SequenceUserRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SequenceRecord_VenuesConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    VenuesName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsEnableGift = table.Column<bool>(type: "bit", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberSteps = table.Column<int>(type: "int", nullable: false),
                    VipFreeSteps = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceRecord_VenuesConfig", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SequenceRecord_GiftUserConfig");

            migrationBuilder.DropTable(
                name: "SequenceRecord_SequenceUseRecord");

            migrationBuilder.DropTable(
                name: "SequenceRecord_SequenceUserRecord");

            migrationBuilder.DropTable(
                name: "SequenceRecord_VenuesConfig");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "SysBaseConfig");

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.DanMu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DMode = table.Column<int>(type: "int", nullable: false),
                    DMsg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DSize = table.Column<int>(type: "int", nullable: false),
                    DTime = table.Column<float>(type: "real", nullable: false),
                    DVideoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.DanMu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Main",
                columns: table => new
                {
                    KeyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BanVideoJumpUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsEnd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMatchInfo = table.Column<int>(type: "int", nullable: false),
                    KeyWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MovieType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NarrateUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoCasts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoContentFeature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoCountries = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoDescribe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoDirectors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoDouBan = table.Column<double>(type: "float", nullable: true),
                    VideoStatus = table.Column<int>(type: "int", nullable: true),
                    VideoType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Main", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Series",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LiveUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrderBy = table.Column<int>(type: "int", nullable: false),
                    SeriesDesUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesRemark = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.SeriesList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    KeyId = table.Column<int>(type: "int", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.SeriesList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Subscribe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ObjId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Subscribe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserNick = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPwd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.UserHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EpId = table.Column<int>(type: "int", nullable: false),
                    EpName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    KeyId = table.Column<int>(type: "int", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VodUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.UserHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Wait",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DouBanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old.SearchSys.Wait", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Old.SearchSys.Episode",
                columns: table => new
                {
                    EpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeyId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EpisodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EpisodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Old.SearchSys.Episode_KeyId",
                table: "Old.SearchSys.Episode",
                column: "KeyId");
        }
    }
}
