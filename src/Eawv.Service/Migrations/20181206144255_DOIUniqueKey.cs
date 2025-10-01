// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class DOIUniqueKey : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_DomainOfInfluenceElections_ElectionId",
            table: "DomainOfInfluenceElections");

        migrationBuilder.AddUniqueConstraint(
            name: "AK_DomainOfInfluenceElections_ElectionId_DomainOfInfluenceId",
            table: "DomainOfInfluenceElections",
            columns: new[] { "ElectionId", "DomainOfInfluenceId" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropUniqueConstraint(
            name: "AK_DomainOfInfluenceElections_ElectionId_DomainOfInfluenceId",
            table: "DomainOfInfluenceElections");

        migrationBuilder.CreateIndex(
            name: "IX_DomainOfInfluenceElections_ElectionId",
            table: "DomainOfInfluenceElections",
            column: "ElectionId");
    }
}
