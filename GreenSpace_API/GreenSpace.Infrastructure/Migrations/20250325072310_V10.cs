using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "DesignIdeas",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<double>(
                name: "DesignPrice",
                table: "DesignIdeas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaterialPrice",
                table: "DesignIdeas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesignPrice",
                table: "DesignIdeas");

            migrationBuilder.DropColumn(
                name: "MaterialPrice",
                table: "DesignIdeas");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "DesignIdeas",
                newName: "Price");
        }
    }
}
