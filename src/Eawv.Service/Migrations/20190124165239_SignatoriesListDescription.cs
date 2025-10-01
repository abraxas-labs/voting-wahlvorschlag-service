// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class SignatoriesListDescription : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // My bad, changed a filename of a template after creating the previous migration
        // Now, we have to delete the existing template, which could have one of those two ids
        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("b9bdbd38-929b-42cc-415b-d099a3d599ac"));
        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("64c50e44-f625-f533-5a88-bab71c3136db"));

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            column: "Content",
            value: @"@using RazorLight
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayout"";
    var entries = Model.Election.QuorumSignaturesCount ?? 200;
    var entriesPerPage = 10;
    var pages = entries / entriesPerPage;
}

@for (var page = 1; page <= pages; page++)
{
    <div class=""page"">
        <header class=""row space"">
            <h1>@ViewBag.Model.Election.Name vom @ViewBag.Model.Election.ContestDate.ToString(""dd. MMMM yyyy"")</h1>
            <h1>@ViewBag.Model.ElectionTenant.Name</h1>
        </header>
        <main>
            <div class=""row stretch"">
                <div class=""grow"">
                    <div class=""row space"">
                        <div class=""row col-main"">
                            <div>
                                <span class=""bold"">Bezeichnung</span> des Wahlvorschlags:
                            </div>
                            <div class=""grow col"">
                                <div class=""input"">
                                    <input value=""@Model.List.Name"" />
                                    <div class=""border""></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""row stretch"">
                <div class=""grow"">
                    <div class=""row space"">
                        <div class=""row col-main"">
                            <div>
                                Evtl. <span class=""bold"">Präzisierung</span> nach Alter, Geschlecht, Region oder Parteiflügel:
                            </div>
                            <div class=""grow col"">
                                <div class=""input"">
                                    <input value=""@Model.List.Description"" />
                                    <div class=""border""></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div class=""row space"">
                    <div class=""row items-end"">
                        <span class=""label space-right"">C</span>
                        <div>
                            <h2 class=""inline"">Unterzeichnerinnen und Unterzeichner des Wahlvorschlags</h2>
                        </div>
                    </div>
                </div>
                <table class=""fullwidth"">
                    <thead>
                        <tr>
                            <th rowspan=""2"">Nr.</th>
                            <th rowspan=""2"" style=""width: 3cm"">Name</th>
                            <th rowspan=""2"" style=""width: 3cm"">Vorname</th>
                            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
                            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
                            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
                            <th rowspan=""2"" style=""width: 3.5cm"">Unterschrift</th>
                            <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
                        </tr>
                        <tr class=""top-no-border"">
                            <th>
                                <span class=""upright"">Tag</span>
                            </th>
                            <th>
                                <span class=""upright"">Monat</span>
                            </th>
                            <th>
                                <span class=""upright"">Jahr</span>
                            </th>
                            <th style=""width: 3.5cm;"">
                                (inkl. allfällige Titel)
                            </th>
                            <th style=""width: 3cm;"">Strasse/Nr.</th>
                            <th>PLZ</th>
                            <th>Wohnort</th>
                        </tr>
                    </thead>
                    <tbody>
                        @for (var i = 1; i <= entriesPerPage && (page - 1) * entriesPerPage + i <= entries; i++)
                        {
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class=""filled""></td>
                            </tr>
                        }
                    </tbody>
                </table>
                <div>@Model.GetInfoText(""formSignatories"")</div>
            </div>
        </main>
    </div>
}");

        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Key", "Landscape", "TenantId", "Type" },
            values: new object[] { new Guid("64c50e44-f625-f533-5a88-bab71c3136db"), @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System.Collections.Generic;
@using System;
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
Listennummer;Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatorte;Beruf
@{
    const char CsvSeparator = ';';
    const char CsvSeparatorReplacement = ',';
    foreach (List list in Model.Election.Lists)
    {
        foreach (Candidate candidate in list.Candidates.OrderBy(c => c.Index))
        {
            var sex = ((int)candidate.Sex) - 1;
            var fields = new List<string>
            {
                list.Indenture,
                candidate.Index.ToString(),
                candidate.FamilyName,
                candidate.FirstName,
                candidate.BallotFamilyName,
                candidate.BallotFirstName,
                sex.ToString(),
                candidate.DateOfBirth.ToString(""dd.MM.yyyy""),
                candidate.Street + ' ' + candidate.HouseNumber,
                candidate.ZipCode,
                candidate.Locality,
                candidate.Origin,
                candidate.OccupationalTitle
            }
            .Select(f => f?.Replace(CsvSeparator, CsvSeparatorReplacement) ?? """");
            @Raw(string.Join(CsvSeparator, fields))
            @Raw(Environment.NewLine)
        }
    }
}", "@Model.Election.Name - Kandidaten (Bundeskanzlei).csv", 0, null, false, null, 3 });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("64c50e44-f625-f533-5a88-bab71c3136db"));

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            column: "Content",
            value: @"@using RazorLight
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayout"";
    var entries = Model.Election.QuorumSignaturesCount ?? 200;
    var entriesPerPage = 10;
    var pages = entries / entriesPerPage;
}

@for (var page = 1; page <= pages; page++)
{
    <div class=""page"">
        <header class=""row space"">
            <h1>@ViewBag.Model.Election.Name vom @ViewBag.Model.Election.ContestDate.ToString(""dd. MMMM yyyy"")</h1>
            <h1>@ViewBag.Model.ElectionTenant.Name</h1>
        </header>
        <main>
            <div class=""row stretch"">
                <div class=""grow"">
                    <div class=""row space"">
                        <div class=""row col-main"">
                            <div>
                                <span class=""bold"">Bezeichnung</span> des Wahlvorschlags:
                            </div>
                            <div class=""grow col"">
                                <div class=""input"">
                                    <input value=""@Model.List.Name"" />
                                    <div class=""border""></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div class=""row space"">
                    <div class=""row items-end"">
                        <span class=""label space-right"">C</span>
                        <div>
                            <h2 class=""inline"">Unterzeichnerinnen und Unterzeichner des Wahlvorschlags</h2>
                        </div>
                    </div>
                </div>
                <table class=""fullwidth"">
                    <thead>
                        <tr>
                            <th rowspan=""2"">Nr.</th>
                            <th rowspan=""2"" style=""width: 3cm"">Name</th>
                            <th rowspan=""2"" style=""width: 3cm"">Vorname</th>
                            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
                            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
                            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
                            <th rowspan=""2"" style=""width: 3.5cm"">Unterschrift</th>
                            <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
                        </tr>
                        <tr class=""top-no-border"">
                            <th>
                                <span class=""upright"">Tag</span>
                            </th>
                            <th>
                                <span class=""upright"">Monat</span>
                            </th>
                            <th>
                                <span class=""upright"">Jahr</span>
                            </th>
                            <th style=""width: 3.5cm;"">
                                (inkl. allfällige Titel)
                            </th>
                            <th style=""width: 3cm;"">Strasse/Nr.</th>
                            <th>PLZ</th>
                            <th>Wohnort</th>
                        </tr>
                    </thead>
                    <tbody>
                        @for (var i = 1; i <= entriesPerPage && (page - 1) * entriesPerPage + i <= entries; i++)
                        {
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class=""filled""></td>
                            </tr>
                        }
                    </tbody>
                </table>
                <div>@Model.GetInfoText(""formSignatories"")</div>
            </div>
        </main>
    </div>
}");

        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Key", "Landscape", "TenantId", "Type" },
            values: new object[] { new Guid("b9bdbd38-929b-42cc-415b-d099a3d599ac"), @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System.Collections.Generic;
@using System;
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
Listennummer;Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatorte;Beruf
@{
    const char CsvSeparator = ';';
    const char CsvSeparatorReplacement = ',';
    foreach (List list in Model.Election.Lists)
    {
        foreach (Candidate candidate in list.Candidates.OrderBy(c => c.Index))
        {
            var sex = ((int)candidate.Sex) - 1;
            var fields = new List<string>
            {
                list.Indenture,
                candidate.Index.ToString(),
                candidate.FamilyName,
                candidate.FirstName,
                candidate.BallotFamilyName,
                candidate.BallotFirstName,
                sex.ToString(),
                candidate.DateOfBirth.ToString(""dd.MM.yyyy""),
                candidate.Street + ' ' + candidate.HouseNumber,
                candidate.ZipCode,
                candidate.Locality,
                candidate.Origin,
                candidate.OccupationalTitle
            }
            .Select(f => f?.Replace(CsvSeparator, CsvSeparatorReplacement) ?? """");
            @Raw(string.Join(CsvSeparator, fields))
            @Raw(Environment.NewLine)
        }
    }
}", "@Model.Election.Name - Kandidaten (Bundeskanzlei).csv", 0, null, false, null, 3 });
    }
}
