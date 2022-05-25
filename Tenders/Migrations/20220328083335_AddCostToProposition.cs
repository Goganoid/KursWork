using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenders.Migrations
{
    public partial class AddCostToProposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "Cost",
                table: "Propositions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Propositions");
        }
    }
}
