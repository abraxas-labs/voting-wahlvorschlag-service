// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class CandidateListIndexUnique : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Candidates_Lists_ListId",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "AddressLine1",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "AddressLine2",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "CallName",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "CandidateId",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "CandidateReference",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "CandidateTextInfo",
            table: "Candidates");

        migrationBuilder.RenameColumn(
            name: "UserId",
            table: "Candidates",
            newName: "BallotOccupationalTitle");

        migrationBuilder.RenameColumn(
            name: "Town",
            table: "Candidates",
            newName: "BallotFirstName");

        migrationBuilder.RenameColumn(
            name: "State",
            table: "Candidates",
            newName: "Index");

        migrationBuilder.RenameColumn(
            name: "PartyTenantId",
            table: "Candidates",
            newName: "BallotFamilyName");

        migrationBuilder.RenameColumn(
            name: "DwellingNumber",
            table: "Candidates",
            newName: "OriginCanton");

        migrationBuilder.AlterColumn<string>(
            name: "Indenture",
            table: "Lists",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(int),
            oldNullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "IndentureModifiedDate",
            table: "Lists",
            nullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "OccupationalTitle",
            table: "Candidates",
            nullable: false,
            oldClrType: typeof(string),
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Locality",
            table: "Candidates",
            nullable: false,
            oldClrType: typeof(string),
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "ListId",
            table: "Candidates",
            nullable: false,
            oldClrType: typeof(Guid),
            oldNullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "Cloned",
            table: "Candidates",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddForeignKey(
            name: "FK_Candidates_Lists_ListId",
            table: "Candidates",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Candidates_Lists_ListId",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "IndentureModifiedDate",
            table: "Lists");

        migrationBuilder.DropColumn(
            name: "Cloned",
            table: "Candidates");

        migrationBuilder.RenameColumn(
            name: "OriginCanton",
            table: "Candidates",
            newName: "DwellingNumber");

        migrationBuilder.RenameColumn(
            name: "Index",
            table: "Candidates",
            newName: "State");

        migrationBuilder.RenameColumn(
            name: "BallotOccupationalTitle",
            table: "Candidates",
            newName: "UserId");

        migrationBuilder.RenameColumn(
            name: "BallotFirstName",
            table: "Candidates",
            newName: "Town");

        migrationBuilder.RenameColumn(
            name: "BallotFamilyName",
            table: "Candidates",
            newName: "PartyTenantId");

        migrationBuilder.AlterColumn<int>(
            name: "Indenture",
            table: "Lists",
            type: "INTEGER",
            nullable: true,
            oldClrType: typeof(string),
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "OccupationalTitle",
            table: "Candidates",
            nullable: true,
            oldClrType: typeof(string));

        migrationBuilder.AlterColumn<string>(
            name: "Locality",
            table: "Candidates",
            nullable: true,
            oldClrType: typeof(string));

        migrationBuilder.AlterColumn<Guid>(
            name: "ListId",
            table: "Candidates",
            nullable: true,
            oldClrType: typeof(Guid));

        migrationBuilder.AddColumn<string>(
            name: "AddressLine1",
            table: "Candidates",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "AddressLine2",
            table: "Candidates",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CallName",
            table: "Candidates",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "CandidateId",
            table: "Candidates",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CandidateReference",
            table: "Candidates",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "CandidateTextInfo",
            table: "Candidates",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddForeignKey(
            name: "FK_Candidates_Lists_ListId",
            table: "Candidates",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }
}
