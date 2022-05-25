using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenders.Migrations
{
    public partial class RenameFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Companies_ExecutorId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Companies_OrganizerId",
                table: "Tenders");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Tenders",
                newName: "CompanyOrganizerId");

            migrationBuilder.RenameColumn(
                name: "ExecutorId",
                table: "Tenders",
                newName: "CompanyExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_OrganizerId",
                table: "Tenders",
                newName: "IX_Tenders_CompanyOrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_ExecutorId",
                table: "Tenders",
                newName: "IX_Tenders_CompanyExecutorId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Companies",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                newName: "IX_Companies_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_UserId",
                table: "Companies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Companies_CompanyExecutorId",
                table: "Tenders",
                column: "CompanyExecutorId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Companies_CompanyOrganizerId",
                table: "Tenders",
                column: "CompanyOrganizerId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_UserId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Companies_CompanyExecutorId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Companies_CompanyOrganizerId",
                table: "Tenders");

            migrationBuilder.RenameColumn(
                name: "CompanyOrganizerId",
                table: "Tenders",
                newName: "OrganizerId");

            migrationBuilder.RenameColumn(
                name: "CompanyExecutorId",
                table: "Tenders",
                newName: "ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_CompanyOrganizerId",
                table: "Tenders",
                newName: "IX_Tenders_OrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_CompanyExecutorId",
                table: "Tenders",
                newName: "IX_Tenders_ExecutorId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Companies",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                newName: "IX_Companies_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Companies_ExecutorId",
                table: "Tenders",
                column: "ExecutorId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Companies_OrganizerId",
                table: "Tenders",
                column: "OrganizerId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
