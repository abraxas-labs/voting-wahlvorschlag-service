// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class CandidateDocuments : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CandidateDocuments",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                Name = table.Column<string>(nullable: false),
                Document = table.Column<byte[]>(nullable: false),
                CandidateId = table.Column<Guid>(nullable: false)
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

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CandidateDocuments");
    }
}
