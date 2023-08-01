using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyDiveTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class documentschanging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_AttorneyDocuments_AttorneyDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_LogBookDocuments_LogBookDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_MedicalDocuments_MedicalDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_NationalCardDocuments_NationalCardDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_AttorneyDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_LogBookDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_MedicalDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_NationalCardDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "AttorneyDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "LogBookDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "MedicalDocumentFileId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "NationalCardDocumentFileId",
                table: "Passengers");

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "NationalCardDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "MedicalDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "LogBookDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "AttorneyDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NationalCardDocuments_PassengerId",
                table: "NationalCardDocuments",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalDocuments_PassengerId",
                table: "MedicalDocuments",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_LogBookDocuments_PassengerId",
                table: "LogBookDocuments",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttorneyDocuments_PassengerId",
                table: "AttorneyDocuments",
                column: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttorneyDocuments_Passengers_PassengerId",
                table: "AttorneyDocuments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogBookDocuments_Passengers_PassengerId",
                table: "LogBookDocuments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalDocuments_Passengers_PassengerId",
                table: "MedicalDocuments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NationalCardDocuments_Passengers_PassengerId",
                table: "NationalCardDocuments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttorneyDocuments_Passengers_PassengerId",
                table: "AttorneyDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_LogBookDocuments_Passengers_PassengerId",
                table: "LogBookDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalDocuments_Passengers_PassengerId",
                table: "MedicalDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_NationalCardDocuments_Passengers_PassengerId",
                table: "NationalCardDocuments");

            migrationBuilder.DropIndex(
                name: "IX_NationalCardDocuments_PassengerId",
                table: "NationalCardDocuments");

            migrationBuilder.DropIndex(
                name: "IX_MedicalDocuments_PassengerId",
                table: "MedicalDocuments");

            migrationBuilder.DropIndex(
                name: "IX_LogBookDocuments_PassengerId",
                table: "LogBookDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AttorneyDocuments_PassengerId",
                table: "AttorneyDocuments");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "NationalCardDocuments");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "MedicalDocuments");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "LogBookDocuments");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "AttorneyDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "AttorneyDocumentFileId",
                table: "Passengers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LogBookDocumentFileId",
                table: "Passengers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicalDocumentFileId",
                table: "Passengers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NationalCardDocumentFileId",
                table: "Passengers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_AttorneyDocumentFileId",
                table: "Passengers",
                column: "AttorneyDocumentFileId",
                unique: true,
                filter: "[AttorneyDocumentFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_LogBookDocumentFileId",
                table: "Passengers",
                column: "LogBookDocumentFileId",
                unique: true,
                filter: "[LogBookDocumentFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_MedicalDocumentFileId",
                table: "Passengers",
                column: "MedicalDocumentFileId",
                unique: true,
                filter: "[MedicalDocumentFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_NationalCardDocumentFileId",
                table: "Passengers",
                column: "NationalCardDocumentFileId",
                unique: true,
                filter: "[NationalCardDocumentFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_AttorneyDocuments_AttorneyDocumentFileId",
                table: "Passengers",
                column: "AttorneyDocumentFileId",
                principalTable: "AttorneyDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_LogBookDocuments_LogBookDocumentFileId",
                table: "Passengers",
                column: "LogBookDocumentFileId",
                principalTable: "LogBookDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_MedicalDocuments_MedicalDocumentFileId",
                table: "Passengers",
                column: "MedicalDocumentFileId",
                principalTable: "MedicalDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_NationalCardDocuments_NationalCardDocumentFileId",
                table: "Passengers",
                column: "NationalCardDocumentFileId",
                principalTable: "NationalCardDocuments",
                principalColumn: "Id");
        }
    }
}
