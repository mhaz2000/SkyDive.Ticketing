using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removecapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "FlightLoads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "FlightLoads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
