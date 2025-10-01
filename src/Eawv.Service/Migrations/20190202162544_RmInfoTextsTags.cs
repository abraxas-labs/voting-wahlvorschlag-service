// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class RmInfoTextsTags : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "InfoTexts",
            keyColumn: "Key",
            keyValue: "originCanton"
        );

        migrationBuilder.DeleteData(
            table: "MarkedElements",
            keyColumn: "Field",
            keyValue: "originCanton"
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
