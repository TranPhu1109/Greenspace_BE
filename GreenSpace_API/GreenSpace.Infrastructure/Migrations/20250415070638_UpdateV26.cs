using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateAppointment",
                table: "Tasks",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeAppointment",
                table: "Tasks",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAppointment",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TimeAppointment",
                table: "Tasks");
        }
    }
}
