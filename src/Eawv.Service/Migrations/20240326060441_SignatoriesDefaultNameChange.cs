// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class SignatoriesDefaultNameChange : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            column: "Filename",
            value: "{ELECTION_NAME} - Unterzeichnende (leer).pdf");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            column: "Filename",
            value: "{ELECTION_NAME} - Unterzeichnende.pdf");
    }
}
