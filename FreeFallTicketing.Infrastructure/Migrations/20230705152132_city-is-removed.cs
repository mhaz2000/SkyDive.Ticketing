using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cityisremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Cities_CityId",
                table: "Passengers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_CityId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Passengers");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Passengers");

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "Passengers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_CityId",
                table: "Passengers",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Cities_CityId",
                table: "Passengers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
