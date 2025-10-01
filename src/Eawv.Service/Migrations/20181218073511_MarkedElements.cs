// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class MarkedElements : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Value",
            table: "MarkedElements",
            newName: "Field");

        migrationBuilder.RenameColumn(
            name: "ReferenceId",
            table: "MarkedElements",
            newName: "CandidateId");

        migrationBuilder.RenameColumn(
            name: "Key",
            table: "MarkedElements",
            newName: "CreatedBy");

        migrationBuilder.AddColumn<DateTime>(
            name: "CreationDate",
            table: "MarkedElements",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "ModifiedBy",
            table: "MarkedElements",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ModifiedDate",
            table: "MarkedElements",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_MarkedElements_CandidateId_Field",
            table: "MarkedElements",
            columns: new[] { "CandidateId", "Field" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_MarkedElements_Candidates_CandidateId",
            table: "MarkedElements",
            column: "CandidateId",
            principalTable: "Candidates",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MarkedElements_Candidates_CandidateId",
            table: "MarkedElements");

        migrationBuilder.DropIndex(
            name: "IX_MarkedElements_CandidateId_Field",
            table: "MarkedElements");

        migrationBuilder.DropColumn(
            name: "CreationDate",
            table: "MarkedElements");

        migrationBuilder.DropColumn(
            name: "ModifiedBy",
            table: "MarkedElements");

        migrationBuilder.DropColumn(
            name: "ModifiedDate",
            table: "MarkedElements");

        migrationBuilder.RenameColumn(
            name: "Field",
            table: "MarkedElements",
            newName: "Value");

        migrationBuilder.RenameColumn(
            name: "CreatedBy",
            table: "MarkedElements",
            newName: "Key");

        migrationBuilder.RenameColumn(
            name: "CandidateId",
            table: "MarkedElements",
            newName: "ReferenceId");
    }
}
