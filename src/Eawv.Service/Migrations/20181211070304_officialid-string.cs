// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class Officialidstring : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "OfficialId",
            table: "DomainsOfInfluence",
            type: "TEXT",
            nullable: false,
            oldClrType: typeof(int));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "OfficialId",
            table: "DomainsOfInfluence",
            type: "INTEGER",
            nullable: false,
            oldClrType: typeof(string));
    }
}
