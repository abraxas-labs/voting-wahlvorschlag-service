// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ECH157Template : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Key", "Landscape", "TenantId", "Type" },
            values: new object[] { new Guid("31484345-3735-2d24-0000-000000000000"), null, "{ELECTION_NAME} - eCH-157.xml", 0, null, false, null, 9 });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("31484345-3735-2d24-0000-000000000000"));
    }
}
