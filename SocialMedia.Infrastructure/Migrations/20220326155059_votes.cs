using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Infrastructure.Migrations
{
    public partial class votes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Post_PostId",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_UserData_PostId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "UserData");

            migrationBuilder.AddColumn<int>(
                name: "VotesId",
                table: "UserData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserData_VotesId",
                table: "UserData",
                column: "VotesId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PostId",
                table: "Votes",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Votes_VotesId",
                table: "UserData",
                column: "VotesId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Votes_VotesId",
                table: "UserData");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_UserData_VotesId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "VotesId",
                table: "UserData");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "UserData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserData_PostId",
                table: "UserData",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Post_PostId",
                table: "UserData",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
