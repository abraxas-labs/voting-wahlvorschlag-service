// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class Init : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Countries",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IsoName = table.Column<string>(nullable: false),
                NameShort = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Countries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DomainsOfInfluence",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                TenantId = table.Column<string>(nullable: false),
                OfficialId = table.Column<int>(nullable: false),
                Name = table.Column<string>(nullable: false),
                ShortName = table.Column<string>(nullable: false),
                DomainOfInfluenceType = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DomainsOfInfluence", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Elections",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                TenantId = table.Column<string>(nullable: false),
                Name = table.Column<string>(nullable: false),
                Description = table.Column<string>(nullable: true),
                SubmissionDeadlineBegin = table.Column<DateTime>(nullable: false),
                SubmissionDeadlineEnd = table.Column<DateTime>(nullable: false),
                ContestDate = table.Column<DateTime>(nullable: false),
                ElectionType = table.Column<int>(nullable: false),
                FileUploadActivated = table.Column<bool>(nullable: false),
                FileUploadDescription = table.Column<string>(nullable: true),
                NoQuorumRequired = table.Column<bool>(nullable: false),
                State = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Elections", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MarkedElements",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                ReferenceId = table.Column<Guid>(nullable: false),
                Key = table.Column<string>(nullable: false),
                Value = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MarkedElements", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DomainOfInfluenceElections",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                NumberOfMandates = table.Column<int>(nullable: false),
                ElectionFk = table.Column<Guid>(nullable: false),
                DomainOfInfluenceFk = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DomainOfInfluenceElections", x => x.Id);
                table.ForeignKey(
                    name: "FK_DomainOfInfluenceElections_DomainsOfInfluence_DomainOfInflu~",
                    column: x => x.DomainOfInfluenceFk,
                    principalTable: "DomainsOfInfluence",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DomainOfInfluenceElections_Elections_ElectionFk",
                    column: x => x.ElectionFk,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ElectionQuorumExemptions",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                ElectionFk = table.Column<Guid>(nullable: false),
                PartyTenantId = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ElectionQuorumExemptions", x => x.Id);
                table.ForeignKey(
                    name: "FK_ElectionQuorumExemptions_Elections_ElectionFk",
                    column: x => x.ElectionFk,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "InfoTexts",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                TenantId = table.Column<string>(nullable: true),
                Key = table.Column<string>(nullable: false),
                Value = table.Column<string>(nullable: false),
                Visible = table.Column<bool>(nullable: false),
                Language = table.Column<string>(nullable: false),
                ElectionFk = table.Column<Guid>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InfoTexts", x => x.Id);
                table.ForeignKey(
                    name: "FK_InfoTexts_Elections_ElectionFk",
                    column: x => x.ElectionFk,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Lists",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                ResponsiblePartyTenantId = table.Column<string>(nullable: false),
                Indenture = table.Column<int>(nullable: false),
                Name = table.Column<string>(nullable: false),
                Description = table.Column<string>(nullable: true),
                SortOrder = table.Column<int>(nullable: false),
                State = table.Column<int>(nullable: false),
                ElectionFk = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Lists", x => x.Id);
                table.ForeignKey(
                    name: "FK_Lists_Elections_ElectionFk",
                    column: x => x.ElectionFk,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BallotDocuments",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                Name = table.Column<string>(nullable: false),
                Document = table.Column<byte[]>(nullable: false),
                ElectionFk = table.Column<Guid>(nullable: false),
                ListId = table.Column<Guid>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BallotDocuments", x => x.Id);
                table.ForeignKey(
                    name: "FK_BallotDocuments_Elections_ElectionFk",
                    column: x => x.ElectionFk,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_BallotDocuments_Lists_ListId",
                    column: x => x.ListId,
                    principalTable: "Lists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Candidates",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                UserId = table.Column<string>(nullable: false),
                FamilyName = table.Column<string>(nullable: false),
                FirstName = table.Column<string>(nullable: false),
                CallName = table.Column<string>(nullable: false),
                DateOfBirth = table.Column<DateTime>(nullable: false),
                Sex = table.Column<int>(nullable: false),
                OccupationalTitle = table.Column<string>(nullable: true),
                Origin = table.Column<string>(nullable: true),
                CandidateTextInfo = table.Column<string>(nullable: false),
                AddressLine1 = table.Column<string>(nullable: false),
                AddressLine2 = table.Column<string>(nullable: true),
                Street = table.Column<string>(nullable: false),
                HouseNumber = table.Column<string>(nullable: true),
                DwellingNumber = table.Column<string>(nullable: true),
                ZipCode = table.Column<string>(nullable: false),
                Locality = table.Column<string>(nullable: true),
                Town = table.Column<string>(nullable: false),
                Incumbent = table.Column<bool>(nullable: false),
                State = table.Column<int>(nullable: false),
                CandidateReference = table.Column<string>(nullable: false),
                PartyTenantId = table.Column<string>(nullable: false),
                CountryFk = table.Column<Guid>(nullable: false),
                CandidateFk = table.Column<Guid>(nullable: true),
                ListFk = table.Column<Guid>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Candidates", x => x.Id);
                table.ForeignKey(
                    name: "FK_Candidates_Countries_CountryFk",
                    column: x => x.CountryFk,
                    principalTable: "Countries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Candidates_Lists_ListFk",
                    column: x => x.ListFk,
                    principalTable: "Lists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "ListUnions",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                ListUnionDescription = table.Column<string>(nullable: true),
                ListUnionFk = table.Column<Guid>(nullable: false),
                ListFk = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ListUnions", x => x.Id);
                table.ForeignKey(
                    name: "FK_ListUnions_Lists_ListFk",
                    column: x => x.ListFk,
                    principalTable: "Lists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ListUnions_ListUnions_ListUnionFk",
                    column: x => x.ListUnionFk,
                    principalTable: "ListUnions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ListUnionElements",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                ListUnionFk = table.Column<Guid>(nullable: false),
                ListFk = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ListUnionElements", x => x.Id);
                table.ForeignKey(
                    name: "FK_ListUnionElements_Lists_ListFk",
                    column: x => x.ListFk,
                    principalTable: "Lists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ListUnionElements_ListUnions_ListUnionFk",
                    column: x => x.ListUnionFk,
                    principalTable: "ListUnions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_BallotDocuments_ElectionFk",
            table: "BallotDocuments",
            column: "ElectionFk");

        migrationBuilder.CreateIndex(
            name: "IX_BallotDocuments_ListId",
            table: "BallotDocuments",
            column: "ListId");

        migrationBuilder.CreateIndex(
            name: "IX_Candidates_CountryFk",
            table: "Candidates",
            column: "CountryFk");

        migrationBuilder.CreateIndex(
            name: "IX_Candidates_ListFk",
            table: "Candidates",
            column: "ListFk");

        migrationBuilder.CreateIndex(
            name: "IX_DomainOfInfluenceElections_DomainOfInfluenceFk",
            table: "DomainOfInfluenceElections",
            column: "DomainOfInfluenceFk");

        migrationBuilder.CreateIndex(
            name: "IX_DomainOfInfluenceElections_ElectionFk",
            table: "DomainOfInfluenceElections",
            column: "ElectionFk");

        migrationBuilder.CreateIndex(
            name: "IX_ElectionQuorumExemptions_ElectionFk",
            table: "ElectionQuorumExemptions",
            column: "ElectionFk");

        migrationBuilder.CreateIndex(
            name: "IX_InfoTexts_ElectionFk",
            table: "InfoTexts",
            column: "ElectionFk");

        migrationBuilder.CreateIndex(
            name: "IX_Lists_ElectionFk",
            table: "Lists",
            column: "ElectionFk");

        migrationBuilder.CreateIndex(
            name: "IX_ListUnionElements_ListFk",
            table: "ListUnionElements",
            column: "ListFk");

        migrationBuilder.CreateIndex(
            name: "IX_ListUnionElements_ListUnionFk",
            table: "ListUnionElements",
            column: "ListUnionFk");

        migrationBuilder.CreateIndex(
            name: "IX_ListUnions_ListFk",
            table: "ListUnions",
            column: "ListFk");

        migrationBuilder.CreateIndex(
            name: "IX_ListUnions_ListUnionFk",
            table: "ListUnions",
            column: "ListUnionFk");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BallotDocuments");

        migrationBuilder.DropTable(
            name: "Candidates");

        migrationBuilder.DropTable(
            name: "DomainOfInfluenceElections");

        migrationBuilder.DropTable(
            name: "ElectionQuorumExemptions");

        migrationBuilder.DropTable(
            name: "InfoTexts");

        migrationBuilder.DropTable(
            name: "ListUnionElements");

        migrationBuilder.DropTable(
            name: "MarkedElements");

        migrationBuilder.DropTable(
            name: "Countries");

        migrationBuilder.DropTable(
            name: "DomainsOfInfluence");

        migrationBuilder.DropTable(
            name: "ListUnions");

        migrationBuilder.DropTable(
            name: "Lists");

        migrationBuilder.DropTable(
            name: "Elections");
    }
}
