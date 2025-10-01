// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eawv.Service.Migrations;

/// <inheritdoc />
public partial class RemoveCandidateDocuments : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CandidateDocuments");

        migrationBuilder.DropColumn(
            name: "FileUploadActivated",
            table: "Elections");

        migrationBuilder.DropColumn(
            name: "FileUploadDescription",
            table: "Elections");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "FileUploadActivated",
            table: "Elections",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "FileUploadDescription",
            table: "Elections",
            type: "text",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "CandidateDocuments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedBy = table.Column<string>(type: "text", nullable: false),
                CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Document = table.Column<byte[]>(type: "bytea", nullable: false),
                ModifiedBy = table.Column<string>(type: "text", nullable: true),
                ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CandidateDocuments", x => x.Id);
                table.ForeignKey(
                    name: "FK_CandidateDocuments_Candidates_CandidateId",
                    column: x => x.CandidateId,
                    principalTable: "Candidates",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CandidateDocuments_CandidateId",
            table: "CandidateDocuments",
            column: "CandidateId");
    }
}
