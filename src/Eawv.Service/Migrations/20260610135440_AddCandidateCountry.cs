// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eawv.Service.Migrations;

/// <inheritdoc />
public partial class AddCandidateCountry : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Country",
            table: "Candidates",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.Sql("UPDATE \"Candidates\" SET \"Country\" = 'CH'");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Country",
            table: "Candidates");
    }
}
