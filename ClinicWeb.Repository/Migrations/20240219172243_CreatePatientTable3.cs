using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicWeb.Repository.Migrations
{
    public partial class CreatePatientTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalReports");

            migrationBuilder.DropColumn(
                name: "StatePaid",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Visits",
                newName: "TotalSessions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSessions",
                table: "Visits",
                newName: "SessionId");

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
                name: "MedicalReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treatment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalReports_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalReports_PatientId",
                table: "MedicalReports",
                column: "PatientId");
        }
    }
}
