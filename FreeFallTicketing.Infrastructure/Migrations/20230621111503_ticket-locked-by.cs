using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ticketlockedby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LockedById",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LockedById",
                table: "Tickets",
                column: "LockedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_LockedById",
                table: "Tickets",
                column: "LockedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_LockedById",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_LockedById",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LockedById",
                table: "Tickets");
        }
    }
}
