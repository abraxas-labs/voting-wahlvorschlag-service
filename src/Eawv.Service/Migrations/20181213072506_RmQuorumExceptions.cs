// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class RmQuorumExceptions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ElectionQuorumExemptions");

        migrationBuilder.DropColumn(
            name: "NoQuorumRequired",
            table: "Elections");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "NoQuorumRequired",
            table: "Elections",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "ElectionQuorumExemptions",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                ElectionId = table.Column<Guid>(nullable: false),
                ModifiedBy = table.Column<string>(nullable: true),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                PartyTenantId = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ElectionQuorumExemptions", x => x.Id);
                table.ForeignKey(
                    name: "FK_ElectionQuorumExemptions_Elections_ElectionId",
                    column: x => x.ElectionId,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ElectionQuorumExemptions_ElectionId",
            table: "ElectionQuorumExemptions",
            column: "ElectionId");
    }
}
