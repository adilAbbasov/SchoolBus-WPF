using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolBusWPF.Migrations
{
    /// <inheritdoc />
    public partial class init8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Holidays");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Holidays",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Holidays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Holidays",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Holidays");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Holidays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
