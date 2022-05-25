using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenders.Migrations
{
    public partial class AddedCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propositions_Users_UserId",
                table: "Propositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Users_ExecutorId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Users_OrganizerId",
                table: "Tenders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Propositions",
                table: "Propositions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Propositions",
                newName: "CompanyId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Propositions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Propositions",
                table: "Propositions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_CompanyId",
                table: "Propositions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Propositions_Companies_CompanyId",
                table: "Propositions",
                column: "CompanyId",
                principalTable: "Companies",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propositions_Companies_CompanyId",
                table: "Propositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Companies_ExecutorId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Companies_OrganizerId",
                table: "Tenders");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Propositions",
                table: "Propositions");

            migrationBuilder.DropIndex(
                name: "IX_Propositions_CompanyId",
                table: "Propositions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Propositions");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Propositions",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Propositions",
                table: "Propositions",
                columns: new[] { "UserId", "TenderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Propositions_Users_UserId",
                table: "Propositions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Users_ExecutorId",
                table: "Tenders",
                column: "ExecutorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Users_OrganizerId",
                table: "Tenders",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
