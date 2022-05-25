using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenders.Migrations
{
    public partial class ExplicitManyToManyUserTender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenderUser");

            migrationBuilder.CreateTable(
                name: "Propositions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TenderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propositions", x => new { x.UserId, x.TenderId });
                    table.ForeignKey(
                        name: "FK_Propositions_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "TenderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Propositions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_TenderId",
                table: "Propositions",
                column: "TenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Propositions");

            migrationBuilder.CreateTable(
                name: "TenderUser",
                columns: table => new
                {
                    ParticipantsUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TendersWithParticipationTenderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderUser", x => new { x.ParticipantsUserId, x.TendersWithParticipationTenderId });
                    table.ForeignKey(
                        name: "FK_TenderUser_Tenders_TendersWithParticipationTenderId",
                        column: x => x.TendersWithParticipationTenderId,
                        principalTable: "Tenders",
                        principalColumn: "TenderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenderUser_Users_ParticipantsUserId",
                        column: x => x.ParticipantsUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenderUser_TendersWithParticipationTenderId",
                table: "TenderUser",
                column: "TendersWithParticipationTenderId");
        }
    }
}
