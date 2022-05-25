using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenders.Migrations
{
    public partial class AddCompanyNameRenameSomeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenderId",
                table: "Tenders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Companies",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Companies",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tenders",
                newName: "TenderId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Companies",
                newName: "PhoneNumber");
        }
    }
}
