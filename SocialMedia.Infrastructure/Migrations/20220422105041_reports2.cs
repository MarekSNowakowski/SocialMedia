using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Infrastructure.Migrations
{
    public partial class reports2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Post_PostId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Reports_ReportsId",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_UserData_ReportsId",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportsId",
                table: "UserData");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Reports",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReporterId",
                table: "Reports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                column: "ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Post_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_UserData_ReporterId",
                table: "Reports",
                column: "ReporterId",
                principalTable: "UserData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Post_PostId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_UserData_ReporterId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "ReportsId",
                table: "UserData",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Reports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserData_ReportsId",
                table: "UserData",
                column: "ReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Post_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Reports_ReportsId",
                table: "UserData",
                column: "ReportsId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
