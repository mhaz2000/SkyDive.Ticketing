using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class reservedforisadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InvoiceNumber",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservedForId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReservedForId",
                table: "Tickets",
                column: "ReservedForId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_ReservedForId",
                table: "Tickets",
                column: "ReservedForId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_ReservedForId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ReservedForId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ReservedForId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceNumber",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
