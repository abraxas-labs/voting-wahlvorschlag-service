// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class TemplateEmailNotifications : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Key", "Landscape", "TenantId", "Type" },
            values: new object[] { new Guid("f5021674-afce-9c2c-a533-8a401bb3f2e4"), @"@using RazorLight
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
<h2>Guten Tag</h2>
Der Status des Wahlvorschlags @Model.List.Name wurde geändert.<br />
Für weitere Informationen besuchen Sie bitte die elektronische Abwicklung von Wahlvorschlägen: @Model.GetConfig(""GUIUrl"")elections/@Model.Election.Id.<br />
<br />
Diese Nachricht wurde automatisch generiert.
", "@Model.Election.Name - Statuswechsel Wahlvorschlag", 0, null, false, null, 5 });

        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Key", "Landscape", "TenantId", "Type" },
            values: new object[] { new Guid("4bec31bf-5e45-7b3c-710e-bd4213a24856"), @"@using RazorLight
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
<h2>Guten Tag</h2>
Dem Wahlvorschlag @Model.List.Name wurde ein neuer Kommentar hinterlegt.<br />
Für weitere Informationen besuchen Sie bitte die elektronische Abwicklung von Wahlvorschlägen: @Model.GetConfig(""GUIUrl"")elections/@Model.Election.Id/lists/@Model.List.Id.<br />
<br />
Diese Nachricht wurde automatisch generiert.
", "@Model.Election.Name - Eingang Kommentar", 0, null, false, null, 6 });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("4bec31bf-5e45-7b3c-710e-bd4213a24856"));

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("f5021674-afce-9c2c-a533-8a401bb3f2e4"));
    }
}
