using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtitletomessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "TicketType",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "SkyDiveEventId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "FlightNumber",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FlightDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CancelledTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReserveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SkyDiveEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FlightNumber = table.Column<int>(type: "int", nullable: true),
                    TicketType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlightDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidAmount = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelledTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancelledTickets_AspNetUsers_PaidById",
                        column: x => x.PaidById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CancelledTickets_AspNetUsers_ReservedById",
                        column: x => x.ReservedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CancelledTickets_PaidById",
                table: "CancelledTickets",
                column: "PaidById");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledTickets_ReservedById",
                table: "CancelledTickets",
                column: "ReservedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CancelledTickets");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "TicketType",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SkyDiveEventId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FlightNumber",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FlightDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
