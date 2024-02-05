using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolBusWPF.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Cars_CarId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CarId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Drivers");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriverId",
                table: "Cars",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_DriverId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Drivers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CarId",
                table: "Drivers",
                column: "CarId",
                unique: true,
                filter: "[CarId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Cars_CarId",
                table: "Drivers",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }
    }
}
