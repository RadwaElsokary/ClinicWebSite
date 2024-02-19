using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicWeb.Repository.Migrations
{
    public partial class CreatePatientTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatePaid",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Patients",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nurse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    RemainingPrice = table.Column<double>(type: "float", nullable: false),
                    PaidPrice = table.Column<double>(type: "float", nullable: false),
                    Cash = table.Column<double>(type: "float", nullable: true),
                    Visa = table.Column<double>(type: "float", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_PatientId",
                table: "Visits",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropColumn(
                name: "StatePaid",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Patients");
        }
    }
}
