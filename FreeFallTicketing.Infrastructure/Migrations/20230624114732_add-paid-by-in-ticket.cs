using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpaidbyinticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaidById",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PaidById",
                table: "Tickets",
                column: "PaidById");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_PaidById",
                table: "Tickets",
                column: "PaidById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_PaidById",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PaidById",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PaidById",
                table: "Tickets");
        }
    }
}
