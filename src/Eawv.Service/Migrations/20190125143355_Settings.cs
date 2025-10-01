// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class AddSettings : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Settings",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: true),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                TenantId = table.Column<string>(nullable: false),
                ShowBallotPaperInfos = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Settings", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Settings_TenantId",
            table: "Settings",
            column: "TenantId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Settings");
    }
}
