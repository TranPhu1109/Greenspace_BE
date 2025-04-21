using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "ContructionDate",
                table: "ServiceOrders",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ContructionPrice",
                table: "ServiceOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ContructionTime",
                table: "ServiceOrders",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkecthReport",
                table: "ServiceOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContructionDate",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "ContructionPrice",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "ContructionTime",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "SkecthReport",
                table: "ServiceOrders");
        }
    }
}
