using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addticketamounttypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmount_SkyDiveEventTicketTypes_TypeId",
                table: "SkyDiveEventTicketTypeAmount");

            migrationBuilder.DropForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmount_SkyDiveEvents_SkyDiveEventId",
                table: "SkyDiveEventTicketTypeAmount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkyDiveEventTicketTypeAmount",
                table: "SkyDiveEventTicketTypeAmount");

            migrationBuilder.RenameTable(
                name: "SkyDiveEventTicketTypeAmount",
                newName: "SkyDiveEventTicketTypeAmounts");

            migrationBuilder.RenameIndex(
                name: "IX_SkyDiveEventTicketTypeAmount_TypeId",
                table: "SkyDiveEventTicketTypeAmounts",
                newName: "IX_SkyDiveEventTicketTypeAmounts_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SkyDiveEventTicketTypeAmount_SkyDiveEventId",
                table: "SkyDiveEventTicketTypeAmounts",
                newName: "IX_SkyDiveEventTicketTypeAmounts_SkyDiveEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkyDiveEventTicketTypeAmounts",
                table: "SkyDiveEventTicketTypeAmounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmounts_SkyDiveEventTicketTypes_TypeId",
                table: "SkyDiveEventTicketTypeAmounts",
                column: "TypeId",
                principalTable: "SkyDiveEventTicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmounts_SkyDiveEvents_SkyDiveEventId",
                table: "SkyDiveEventTicketTypeAmounts",
                column: "SkyDiveEventId",
                principalTable: "SkyDiveEvents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmounts_SkyDiveEventTicketTypes_TypeId",
                table: "SkyDiveEventTicketTypeAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmounts_SkyDiveEvents_SkyDiveEventId",
                table: "SkyDiveEventTicketTypeAmounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkyDiveEventTicketTypeAmounts",
                table: "SkyDiveEventTicketTypeAmounts");

            migrationBuilder.RenameTable(
                name: "SkyDiveEventTicketTypeAmounts",
                newName: "SkyDiveEventTicketTypeAmount");

            migrationBuilder.RenameIndex(
                name: "IX_SkyDiveEventTicketTypeAmounts_TypeId",
                table: "SkyDiveEventTicketTypeAmount",
                newName: "IX_SkyDiveEventTicketTypeAmount_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SkyDiveEventTicketTypeAmounts_SkyDiveEventId",
                table: "SkyDiveEventTicketTypeAmount",
                newName: "IX_SkyDiveEventTicketTypeAmount_SkyDiveEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkyDiveEventTicketTypeAmount",
                table: "SkyDiveEventTicketTypeAmount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmount_SkyDiveEventTicketTypes_TypeId",
                table: "SkyDiveEventTicketTypeAmount",
                column: "TypeId",
                principalTable: "SkyDiveEventTicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkyDiveEventTicketTypeAmount_SkyDiveEvents_SkyDiveEventId",
                table: "SkyDiveEventTicketTypeAmount",
                column: "SkyDiveEventId",
                principalTable: "SkyDiveEvents",
                principalColumn: "Id");
        }
    }
}
