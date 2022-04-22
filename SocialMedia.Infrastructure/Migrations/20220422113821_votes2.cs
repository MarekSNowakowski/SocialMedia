using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Infrastructure.Migrations
{
    public partial class votes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Votes_VotesId",
                table: "UserData");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Post_PostId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_PostId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_UserData_VotesId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "VotesId",
                table: "UserData");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Votes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UpvoterId",
                table: "Votes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PostId",
                table: "Votes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UpvoterId",
                table: "Votes",
                column: "UpvoterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Post_PostId",
                table: "Votes",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_UserData_UpvoterId",
                table: "Votes",
                column: "UpvoterId",
                principalTable: "UserData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Post_PostId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_UserData_UpvoterId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_PostId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_UpvoterId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "UpvoterId",
                table: "Votes");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Votes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VotesId",
                table: "UserData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PostId",
                table: "Votes",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserData_VotesId",
                table: "UserData",
                column: "VotesId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Votes_VotesId",
                table: "UserData",
                column: "VotesId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Post_PostId",
                table: "Votes",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
