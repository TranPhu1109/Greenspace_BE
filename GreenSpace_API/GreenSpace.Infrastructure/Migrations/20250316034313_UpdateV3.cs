using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignIdeas_Categories_CategoryId",
                table: "DesignIdeas");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "UsersWallets");

            migrationBuilder.RenameColumn(
                name: "WalletAccount",
                table: "UsersWallets",
                newName: "WalletType");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "DesignIdeas",
                newName: "DesignIdeasCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_DesignIdeas_CategoryId",
                table: "DesignIdeas",
                newName: "IX_DesignIdeas_DesignIdeasCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UsersWallets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FCMToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JWTToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DesignIdeasCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignIdeasCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DesignIdeas_DesignIdeasCategories_DesignIdeasCategoryId",
                table: "DesignIdeas",
                column: "DesignIdeasCategoryId",
                principalTable: "DesignIdeasCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignIdeas_DesignIdeasCategories_DesignIdeasCategoryId",
                table: "DesignIdeas");

            migrationBuilder.DropTable(
                name: "DesignIdeasCategories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UsersWallets");

            migrationBuilder.DropColumn(
                name: "FCMToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JWTToken",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "WalletType",
                table: "UsersWallets",
                newName: "WalletAccount");

            migrationBuilder.RenameColumn(
                name: "DesignIdeasCategoryId",
                table: "DesignIdeas",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_DesignIdeas_DesignIdeasCategoryId",
                table: "DesignIdeas",
                newName: "IX_DesignIdeas_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "UsersWallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignIdeas_Categories_CategoryId",
                table: "DesignIdeas",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
