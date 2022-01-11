using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Infrastructure.Migrations
{
    public partial class photo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Post");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Post");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Post",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
