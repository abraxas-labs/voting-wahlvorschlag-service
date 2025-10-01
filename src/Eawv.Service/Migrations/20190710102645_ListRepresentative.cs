// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ListRepresentative : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Representative",
            table: "Lists",
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""Lists"" SET ""Representative"" = ""CreatedBy""");

        migrationBuilder.AlterColumn<string>(
            name: "Representative",
            table: "Lists",
            nullable: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Representative",
            table: "Lists");
    }
}
