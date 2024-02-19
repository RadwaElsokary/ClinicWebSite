using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicWeb.Repository.Migrations
{
    public partial class CreatePatientTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Visits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Visits");
        }
    }
}
