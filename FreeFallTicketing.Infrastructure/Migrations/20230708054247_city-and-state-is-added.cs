using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cityandstateisadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Passengers");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Passengers",
                newName: "CityAndState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CityAndState",
                table: "Passengers",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
