using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class shoppingcartticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ShoppingCarts_ShoppingCartId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShoppingCartId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Tickets");

            migrationBuilder.CreateTable(
                name: "ShoppingCartTickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShoppingCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartTickets", x => new { x.ShoppingCartId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_ShoppingCartTickets_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartTickets_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartTickets_TicketId",
                table: "ShoppingCartTickets",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartTickets");

            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShoppingCartId",
                table: "Tickets",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ShoppingCarts_ShoppingCartId",
                table: "Tickets",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id");
        }
    }
}
