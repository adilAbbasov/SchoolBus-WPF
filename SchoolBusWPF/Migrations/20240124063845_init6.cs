using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolBusWPF.Migrations
{
    /// <inheritdoc />
    public partial class init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_DriverId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Drivers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriverId",
                table: "Cars",
                column: "DriverId",
                unique: true,
                filter: "[DriverId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_DriverId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Drivers");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriverId",
                table: "Cars",
                column: "DriverId");
        }
    }
}
