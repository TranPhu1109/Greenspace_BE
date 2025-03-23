using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsersWalletId",
                table: "Bills",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bills_UsersWalletId",
                table: "Bills",
                column: "UsersWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_UsersWallets_UsersWalletId",
                table: "Bills",
                column: "UsersWalletId",
                principalTable: "UsersWallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_UsersWallets_UsersWalletId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_UsersWalletId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "UsersWalletId",
                table: "Bills");
        }
    }
}
