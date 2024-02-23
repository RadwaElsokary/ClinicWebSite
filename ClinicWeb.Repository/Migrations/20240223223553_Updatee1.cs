using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicWeb.Repository.Migrations
{
    public partial class Updatee1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "Sessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ServiceId",
                table: "Sessions",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Services_ServiceId",
                table: "Sessions",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Services_ServiceId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_ServiceId",
                table: "Sessions");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
