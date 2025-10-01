// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class OrderIndex : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CloneOrderIndex",
            table: "Candidates",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "OrderIndex",
            table: "Candidates",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql("UPDATE \"Candidates\" SET \"OrderIndex\" = \"Index\"");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CloneOrderIndex",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "OrderIndex",
            table: "Candidates");
    }
}
