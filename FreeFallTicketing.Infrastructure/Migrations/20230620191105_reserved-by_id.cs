using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class reservedby_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_ReservedById",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReservedById",
                table: "Tickets",
                column: "ReservedById",
                unique: true,
                filter: "[ReservedById] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_ReservedById",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReservedById",
                table: "Tickets",
                column: "ReservedById");
        }
    }
}
