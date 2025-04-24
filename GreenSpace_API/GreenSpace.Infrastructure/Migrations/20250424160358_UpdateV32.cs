using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ComplaintReasonId",
                table: "Complaints",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComplaintReason",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionPercentage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepositPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPercentage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ComplaintReasonId",
                table: "Complaints",
                column: "ComplaintReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_ComplaintReason_ComplaintReasonId",
                table: "Complaints",
                column: "ComplaintReasonId",
                principalTable: "ComplaintReason",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_ComplaintReason_ComplaintReasonId",
                table: "Complaints");

            migrationBuilder.DropTable(
                name: "ComplaintReason");

            migrationBuilder.DropTable(
                name: "TransactionPercentage");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_ComplaintReasonId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ComplaintReasonId",
                table: "Complaints");
        }
    }
}
