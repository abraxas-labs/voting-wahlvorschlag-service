// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class Changefktoid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BallotDocuments_Elections_ElectionFk",
            table: "BallotDocuments");

        migrationBuilder.DropForeignKey(
            name: "FK_Candidates_Countries_CountryFk",
            table: "Candidates");

        migrationBuilder.DropForeignKey(
            name: "FK_Candidates_Lists_ListFk",
            table: "Candidates");

        migrationBuilder.DropForeignKey(
            name: "FK_DomainOfInfluenceElections_Elections_ElectionFk",
            table: "DomainOfInfluenceElections");

        migrationBuilder.DropForeignKey(
            name: "FK_ElectionQuorumExemptions_Elections_ElectionFk",
            table: "ElectionQuorumExemptions");

        migrationBuilder.DropForeignKey(
            name: "FK_InfoTexts_Elections_ElectionFk",
            table: "InfoTexts");

        migrationBuilder.DropForeignKey(
            name: "FK_Lists_Elections_ElectionFk",
            table: "Lists");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnionElements_Lists_ListFk",
            table: "ListUnionElements");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnionElements_ListUnions_ListUnionFk",
            table: "ListUnionElements");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_Lists_ListFk",
            table: "ListUnions");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_ListUnions_ListUnionFk",
            table: "ListUnions");

        migrationBuilder.RenameColumn(
            name: "ListUnionFk",
            table: "ListUnions",
            newName: "ListUnionId");

        migrationBuilder.RenameColumn(
            name: "ListFk",
            table: "ListUnions",
            newName: "ListId");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnions_ListUnionFk",
            table: "ListUnions",
            newName: "IX_ListUnions_ListUnionId");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnions_ListFk",
            table: "ListUnions",
            newName: "IX_ListUnions_ListId");

        migrationBuilder.RenameColumn(
            name: "ListUnionFk",
            table: "ListUnionElements",
            newName: "ListUnionId");

        migrationBuilder.RenameColumn(
            name: "ListFk",
            table: "ListUnionElements",
            newName: "ListId");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnionElements_ListUnionFk",
            table: "ListUnionElements",
            newName: "IX_ListUnionElements_ListUnionId");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnionElements_ListFk",
            table: "ListUnionElements",
            newName: "IX_ListUnionElements_ListId");

        migrationBuilder.RenameColumn(
            name: "ElectionFk",
            table: "Lists",
            newName: "ElectionId");

        migrationBuilder.RenameIndex(
            name: "IX_Lists_ElectionFk",
            table: "Lists",
            newName: "IX_Lists_ElectionId");

        migrationBuilder.RenameColumn(
            name: "ElectionFk",
            table: "InfoTexts",
            newName: "ElectionId");

        migrationBuilder.RenameIndex(
            name: "IX_InfoTexts_ElectionFk",
            table: "InfoTexts",
            newName: "IX_InfoTexts_ElectionId");

        migrationBuilder.RenameColumn(
            name: "ElectionFk",
            table: "ElectionQuorumExemptions",
            newName: "ElectionId");

        migrationBuilder.RenameIndex(
            name: "IX_ElectionQuorumExemptions_ElectionFk",
            table: "ElectionQuorumExemptions",
            newName: "IX_ElectionQuorumExemptions_ElectionId");

        migrationBuilder.RenameColumn(
            name: "ElectionFk",
            table: "DomainOfInfluenceElections",
            newName: "ElectionId");

        migrationBuilder.RenameColumn(
            name: "DomainOfInfluenceFk",
            table: "DomainOfInfluenceElections",
            newName: "DomainOfInfluenceId");

        migrationBuilder.RenameIndex(
            name: "IX_DomainOfInfluenceElections_ElectionFk",
            table: "DomainOfInfluenceElections",
            newName: "IX_DomainOfInfluenceElections_ElectionId");

        migrationBuilder.RenameIndex(
            name: "IX_DomainOfInfluenceElections_DomainOfInfluenceFk",
            table: "DomainOfInfluenceElections",
            newName: "IX_DomainOfInfluenceElections_DomainOfInfluenceId");

        migrationBuilder.RenameColumn(
            name: "ListFk",
            table: "Candidates",
            newName: "ListId");

        migrationBuilder.RenameColumn(
            name: "CountryFk",
            table: "Candidates",
            newName: "CountryId");

        migrationBuilder.RenameColumn(
            name: "CandidateFk",
            table: "Candidates",
            newName: "CandidateId");

        migrationBuilder.RenameIndex(
            name: "IX_Candidates_ListFk",
            table: "Candidates",
            newName: "IX_Candidates_ListId");

        migrationBuilder.RenameIndex(
            name: "IX_Candidates_CountryFk",
            table: "Candidates",
            newName: "IX_Candidates_CountryId");

        migrationBuilder.RenameColumn(
            name: "ElectionFk",
            table: "BallotDocuments",
            newName: "ElectionId");

        migrationBuilder.RenameIndex(
            name: "IX_BallotDocuments_ElectionFk",
            table: "BallotDocuments",
            newName: "IX_BallotDocuments_ElectionId");

        migrationBuilder.AddForeignKey(
            name: "FK_BallotDocuments_Elections_ElectionId",
            table: "BallotDocuments",
            column: "ElectionId",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Candidates_Countries_CountryId",
            table: "Candidates",
            column: "CountryId",
            principalTable: "Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Candidates_Lists_ListId",
            table: "Candidates",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_DomainOfInfluenceElections_Elections_ElectionId",
            table: "DomainOfInfluenceElections",
            column: "ElectionId",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ElectionQuorumExemptions_Elections_ElectionId",
            table: "ElectionQuorumExemptions",
            column: "ElectionId",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_InfoTexts_Elections_ElectionId",
            table: "InfoTexts",
            column: "ElectionId",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Lists_Elections_ElectionId",
            table: "Lists",
            column: "ElectionId",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnionElements_Lists_ListId",
            table: "ListUnionElements",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnionElements_ListUnions_ListUnionId",
            table: "ListUnionElements",
            column: "ListUnionId",
            principalTable: "ListUnions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_ListUnions_ListUnionId",
            table: "ListUnions",
            column: "ListUnionId",
            principalTable: "ListUnions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BallotDocuments_Elections_ElectionId",
            table: "BallotDocuments");

        migrationBuilder.DropForeignKey(
            name: "FK_Candidates_Countries_CountryId",
            table: "Candidates");

        migrationBuilder.DropForeignKey(
            name: "FK_Candidates_Lists_ListId",
            table: "Candidates");

        migrationBuilder.DropForeignKey(
            name: "FK_DomainOfInfluenceElections_Elections_ElectionId",
            table: "DomainOfInfluenceElections");

        migrationBuilder.DropForeignKey(
            name: "FK_ElectionQuorumExemptions_Elections_ElectionId",
            table: "ElectionQuorumExemptions");

        migrationBuilder.DropForeignKey(
            name: "FK_InfoTexts_Elections_ElectionId",
            table: "InfoTexts");

        migrationBuilder.DropForeignKey(
            name: "FK_Lists_Elections_ElectionId",
            table: "Lists");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnionElements_Lists_ListId",
            table: "ListUnionElements");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnionElements_ListUnions_ListUnionId",
            table: "ListUnionElements");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_ListUnions_ListUnionId",
            table: "ListUnions");

        migrationBuilder.RenameColumn(
            name: "ListUnionId",
            table: "ListUnions",
            newName: "ListUnionFk");

        migrationBuilder.RenameColumn(
            name: "ListId",
            table: "ListUnions",
            newName: "ListFk");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnions_ListUnionId",
            table: "ListUnions",
            newName: "IX_ListUnions_ListUnionFk");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnions_ListId",
            table: "ListUnions",
            newName: "IX_ListUnions_ListFk");

        migrationBuilder.RenameColumn(
            name: "ListUnionId",
            table: "ListUnionElements",
            newName: "ListUnionFk");

        migrationBuilder.RenameColumn(
            name: "ListId",
            table: "ListUnionElements",
            newName: "ListFk");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnionElements_ListUnionId",
            table: "ListUnionElements",
            newName: "IX_ListUnionElements_ListUnionFk");

        migrationBuilder.RenameIndex(
            name: "IX_ListUnionElements_ListId",
            table: "ListUnionElements",
            newName: "IX_ListUnionElements_ListFk");

        migrationBuilder.RenameColumn(
            name: "ElectionId",
            table: "Lists",
            newName: "ElectionFk");

        migrationBuilder.RenameIndex(
            name: "IX_Lists_ElectionId",
            table: "Lists",
            newName: "IX_Lists_ElectionFk");

        migrationBuilder.RenameColumn(
            name: "ElectionId",
            table: "InfoTexts",
            newName: "ElectionFk");

        migrationBuilder.RenameIndex(
            name: "IX_InfoTexts_ElectionId",
            table: "InfoTexts",
            newName: "IX_InfoTexts_ElectionFk");

        migrationBuilder.RenameColumn(
            name: "ElectionId",
            table: "ElectionQuorumExemptions",
            newName: "ElectionFk");

        migrationBuilder.RenameIndex(
            name: "IX_ElectionQuorumExemptions_ElectionId",
            table: "ElectionQuorumExemptions",
            newName: "IX_ElectionQuorumExemptions_ElectionFk");

        migrationBuilder.RenameColumn(
            name: "ElectionId",
            table: "DomainOfInfluenceElections",
            newName: "ElectionFk");

        migrationBuilder.RenameColumn(
            name: "DomainOfInfluenceId",
            table: "DomainOfInfluenceElections",
            newName: "DomainOfInfluenceFk");

        migrationBuilder.RenameIndex(
            name: "IX_DomainOfInfluenceElections_ElectionId",
            table: "DomainOfInfluenceElections",
            newName: "IX_DomainOfInfluenceElections_ElectionFk");

        migrationBuilder.RenameIndex(
            name: "IX_DomainOfInfluenceElections_DomainOfInfluenceId",
            table: "DomainOfInfluenceElections",
            newName: "IX_DomainOfInfluenceElections_DomainOfInfluenceFk");

        migrationBuilder.RenameColumn(
            name: "ListId",
            table: "Candidates",
            newName: "ListFk");

        migrationBuilder.RenameColumn(
            name: "CountryId",
            table: "Candidates",
            newName: "CountryFk");

        migrationBuilder.RenameColumn(
            name: "CandidateId",
            table: "Candidates",
            newName: "CandidateFk");

        migrationBuilder.RenameIndex(
            name: "IX_Candidates_ListId",
            table: "Candidates",
            newName: "IX_Candidates_ListFk");

        migrationBuilder.RenameIndex(
            name: "IX_Candidates_CountryId",
            table: "Candidates",
            newName: "IX_Candidates_CountryFk");

        migrationBuilder.RenameColumn(
            name: "ElectionId",
            table: "BallotDocuments",
            newName: "ElectionFk");

        migrationBuilder.RenameIndex(
            name: "IX_BallotDocuments_ElectionId",
            table: "BallotDocuments",
            newName: "IX_BallotDocuments_ElectionFk");

        migrationBuilder.AddForeignKey(
            name: "FK_BallotDocuments_Elections_ElectionFk",
            table: "BallotDocuments",
            column: "ElectionFk",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Candidates_Countries_CountryFk",
            table: "Candidates",
            column: "CountryFk",
            principalTable: "Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Candidates_Lists_ListFk",
            table: "Candidates",
            column: "ListFk",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_DomainOfInfluenceElections_Elections_ElectionFk",
            table: "DomainOfInfluenceElections",
            column: "ElectionFk",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ElectionQuorumExemptions_Elections_ElectionFk",
            table: "ElectionQuorumExemptions",
            column: "ElectionFk",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_InfoTexts_Elections_ElectionFk",
            table: "InfoTexts",
            column: "ElectionFk",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Lists_Elections_ElectionFk",
            table: "Lists",
            column: "ElectionFk",
            principalTable: "Elections",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnionElements_Lists_ListFk",
            table: "ListUnionElements",
            column: "ListFk",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnionElements_ListUnions_ListUnionFk",
            table: "ListUnionElements",
            column: "ListUnionFk",
            principalTable: "ListUnions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_Lists_ListFk",
            table: "ListUnions",
            column: "ListFk",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_ListUnions_ListUnionFk",
            table: "ListUnions",
            column: "ListUnionFk",
            principalTable: "ListUnions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
