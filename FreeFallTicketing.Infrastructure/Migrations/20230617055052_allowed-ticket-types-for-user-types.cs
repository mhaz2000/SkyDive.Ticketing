﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class allowedtickettypesforusertypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
