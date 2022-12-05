using Microsoft.EntityFrameworkCore.Migrations;

namespace KdyWeb.EntityFramework.Migrations
{
    public partial class parse_children : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudParse_UserChildren_CloudParse_User_UserId",
                table: "CloudParse_UserChildren");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CloudParse_UserChildren",
                table: "CloudParse_UserChildren");

            migrationBuilder.DropIndex(
                name: "IX_CloudParse_UserChildren_UserId",
                table: "CloudParse_UserChildren");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserNick",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserPwd",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserQq",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "CloudParse_User");

            migrationBuilder.RenameTable(
                name: "CloudParse_UserChildren",
                newName: "CloudParse_SubAccount");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "CloudParse_User",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "CloudParse_SubAccount",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldSubAccountInfo",
                table: "CloudParse_SubAccount",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CloudParse_SubAccount",
                table: "CloudParse_SubAccount",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CloudParse_SubAccount",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CloudParse_User");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "CloudParse_SubAccount");

            migrationBuilder.DropColumn(
                name: "OldSubAccountInfo",
                table: "CloudParse_SubAccount");

            migrationBuilder.RenameTable(
                name: "CloudParse_SubAccount",
                newName: "CloudParse_UserChildren");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "CloudParse_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CloudParse_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNick",
                table: "CloudParse_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPwd",
                table: "CloudParse_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserQq",
                table: "CloudParse_User",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                table: "CloudParse_User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CloudParse_UserChildren",
                type: "int",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CloudParse_UserChildren",
                table: "CloudParse_UserChildren",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CloudParse_UserChildren_CloudParse_User_UserId",
                table: "CloudParse_UserChildren",
                column: "UserId",
                principalTable: "CloudParse_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
