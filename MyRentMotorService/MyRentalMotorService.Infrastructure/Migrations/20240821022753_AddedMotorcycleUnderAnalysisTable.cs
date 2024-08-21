using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRentalMotorService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMotorcycleUnderAnalysisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotorcyclesUnderAnalysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    LicensePlate = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MotorcycleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorcyclesUnderAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotorcyclesUnderAnalysis_Motorcycles_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotorcyclesUnderAnalysis_MotorcycleId",
                table: "MotorcyclesUnderAnalysis",
                column: "MotorcycleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotorcyclesUnderAnalysis");
        }
    }
}
