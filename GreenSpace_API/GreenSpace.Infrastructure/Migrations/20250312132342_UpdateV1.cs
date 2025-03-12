using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordDesigns_Images_ImageId1",
                table: "RecordDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordDesigns_ServiceOrders_ServiceOrderId1",
                table: "RecordDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordSketches_Images_ImageId1",
                table: "RecordSketches");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordSketches_ServiceOrders_ServiceOrderId1",
                table: "RecordSketches");

            migrationBuilder.DropIndex(
                name: "IX_RecordSketches_ImageId1",
                table: "RecordSketches");

            migrationBuilder.DropIndex(
                name: "IX_RecordSketches_ServiceOrderId1",
                table: "RecordSketches");

            migrationBuilder.DropIndex(
                name: "IX_RecordDesigns_ImageId1",
                table: "RecordDesigns");

            migrationBuilder.DropIndex(
                name: "IX_RecordDesigns_ServiceOrderId1",
                table: "RecordDesigns");

            migrationBuilder.DropColumn(
                name: "ImageId1",
                table: "RecordSketches");

            migrationBuilder.DropColumn(
                name: "ServiceOrderId1",
                table: "RecordSketches");

            migrationBuilder.DropColumn(
                name: "ImageId1",
                table: "RecordDesigns");

            migrationBuilder.DropColumn(
                name: "ServiceOrderId1",
                table: "RecordDesigns");

            migrationBuilder.AddColumn<Guid>(
                name: "RecordDesignId",
                table: "ServiceOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RecordSketchId",
                table: "ServiceOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordDesignId",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "RecordSketchId",
                table: "ServiceOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId1",
                table: "RecordSketches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceOrderId1",
                table: "RecordSketches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId1",
                table: "RecordDesigns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceOrderId1",
                table: "RecordDesigns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecordSketches_ImageId1",
                table: "RecordSketches",
                column: "ImageId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecordSketches_ServiceOrderId1",
                table: "RecordSketches",
                column: "ServiceOrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecordDesigns_ImageId1",
                table: "RecordDesigns",
                column: "ImageId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecordDesigns_ServiceOrderId1",
                table: "RecordDesigns",
                column: "ServiceOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordDesigns_Images_ImageId1",
                table: "RecordDesigns",
                column: "ImageId1",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordDesigns_ServiceOrders_ServiceOrderId1",
                table: "RecordDesigns",
                column: "ServiceOrderId1",
                principalTable: "ServiceOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordSketches_Images_ImageId1",
                table: "RecordSketches",
                column: "ImageId1",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordSketches_ServiceOrders_ServiceOrderId1",
                table: "RecordSketches",
                column: "ServiceOrderId1",
                principalTable: "ServiceOrders",
                principalColumn: "Id");
        }
    }
}
