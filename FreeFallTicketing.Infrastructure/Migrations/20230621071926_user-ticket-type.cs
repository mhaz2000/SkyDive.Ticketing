using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class usertickettype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkyDiveEventTicketTypes_UserTypes_UserTypeId",
                table: "SkyDiveEventTicketTypes");

            migrationBuilder.DropIndex(
                name: "IX_SkyDiveEventTicketTypes_UserTypeId",
                table: "SkyDiveEventTicketTypes");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "SkyDiveEventTicketTypes");

            migrationBuilder.CreateTable(
                name: "UserTypeTicketTypes",
                columns: table => new
                {
                    UserTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypeTicketTypes", x => new { x.TicketTypeId, x.UserTypeId });
                    table.ForeignKey(
                        name: "FK_UserTypeTicketTypes_SkyDiveEventTicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "SkyDiveEventTicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTypeTicketTypes_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeTicketTypes_UserTypeId",
                table: "UserTypeTicketTypes",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTypeTicketTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "UserTypeId",
                table: "SkyDiveEventTicketTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkyDiveEventTicketTypes_UserTypeId",
                table: "SkyDiveEventTicketTypes",
                column: "UserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkyDiveEventTicketTypes_UserTypes_UserTypeId",
                table: "SkyDiveEventTicketTypes",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "Id");
        }
    }
}
