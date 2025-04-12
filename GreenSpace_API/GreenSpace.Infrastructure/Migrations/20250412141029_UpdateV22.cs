using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Contracts");
        }
    }
}
