using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicWeb.Repository.Migrations
{
    public partial class update22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "TotalPriceSessions",
                table: "Patients",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Patients",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Patients",
                newName: "TotalPriceSessions");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Sessions",
                type: "int",
                nullable: true);
        }
    }
}
