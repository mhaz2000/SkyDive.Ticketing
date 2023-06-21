using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class skydiveeventisaddedtoshoppingcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SkyDiveEventId",
                table: "ShoppingCarts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_SkyDiveEventId",
                table: "ShoppingCarts",
                column: "SkyDiveEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_SkyDiveEvents_SkyDiveEventId",
                table: "ShoppingCarts",
                column: "SkyDiveEventId",
                principalTable: "SkyDiveEvents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_SkyDiveEvents_SkyDiveEventId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_SkyDiveEventId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "SkyDiveEventId",
                table: "ShoppingCarts");
        }
    }
}
