// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ListLocked : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "Locked",
            table: "Lists",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "Version",
            table: "Lists",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Locked",
            table: "Lists");

        migrationBuilder.DropColumn(
            name: "Version",
            table: "Lists");
    }
}
