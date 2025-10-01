// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ListIndentureOptional : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "Indenture",
            table: "Lists",
            nullable: true,
            oldClrType: typeof(int));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "Indenture",
            table: "Lists",
            nullable: false,
            oldClrType: typeof(int),
            oldNullable: true);
    }
}
