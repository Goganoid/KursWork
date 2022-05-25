using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenders.Migrations
{
    public partial class RenameFK2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_UserId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Companies",
                newName: "UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                newName: "IX_Companies_UserOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_UserOwnerId",
                table: "Companies",
                column: "UserOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_UserOwnerId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "Companies",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_UserOwnerId",
                table: "Companies",
                newName: "IX_Companies_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_UserId",
                table: "Companies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
