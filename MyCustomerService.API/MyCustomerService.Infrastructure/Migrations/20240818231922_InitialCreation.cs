using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCustomerService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cnpj = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DriverLicense = table.Column<string>(type: "text", nullable: false),
                    DriverLicenseType = table.Column<int>(type: "integer", nullable: false),
                    DriverLicenseImageFileName = table.Column<string>(type: "text", nullable: true),
                    DriverLicenseImageLocation = table.Column<string>(type: "text", nullable: true),
                    DriverLicenseImageContentType = table.Column<string>(type: "text", nullable: true),
                    ActiveCustomer = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.UniqueConstraint("AK_Customers_Cnpj", x => x.Cnpj);
                    table.UniqueConstraint("AK_Customers_DriverLicense", x => x.DriverLicense);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
