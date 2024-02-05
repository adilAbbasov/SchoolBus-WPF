using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolBusWPF.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Rides",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Rides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_CarId",
                table: "Rides",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Cars_CarId",
                table: "Rides",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Cars_CarId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_CarId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Rides");
        }
    }
}
