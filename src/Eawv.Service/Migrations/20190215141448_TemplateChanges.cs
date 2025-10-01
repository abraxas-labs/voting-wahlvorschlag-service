// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class TemplateChanges : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("0ea74799-d3fa-c8ea-b7e6-af290716b6c8"),
            columns: new[] { "Content", "Filename" },
            values: new object[] { @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System.Collections.Generic;
@using System;
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@(Model.IsProporz ? ""Listennummer;"" : """")Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatort(e) inkl. Kantonszugehörigkeit;Titel;Beruf(e);Politischer Beruf;@(Model.IsMajorz ? ""Partei;"" : """")Bisher@(Model.IsProporz ? "";Vorkumuliert"" : """")
@{
    const char CsvSeparator = ';';
    const char CsvSeparatorReplacement = ',';
    foreach (List list in Model.Election.Lists)
    {
        foreach (Candidate candidate in Model.GetClonedAndOrderedCandidates(list))
        {
            var sex = ((int)candidate.Sex) - 1;
            var fields = new List<string>
            {
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
                candidate.Title,
                candidate.OccupationalTitle,
                candidate.BallotOccupationalTitle,
                candidate.Incumbent ? ""Ja"" : ""Nein""
            };

            if (Model.IsMajorz)
            {
                fields.Insert(fields.Count - 1, candidate.Party); // insert before ""Incumbent""
            }

            if (Model.IsProporz)
            {
                fields.Insert(0, list.Indenture);
                fields.Add(candidate.Cloned ? ""Ja"" : ""Nein"");
            }

            fields = fields.Select(f => f?.Replace(CsvSeparator, CsvSeparatorReplacement) ?? """").ToList();
            @Raw(string.Join(CsvSeparator, fields))
            @Raw(Environment.NewLine)
        }
    }
}", "{ELECTION_NAME} - Kandidaten.csv" });

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"),
            column: "Filename",
            value: "{ELECTION_NAME} - Listenverbindungen (leer).pdf");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("4bec31bf-5e45-7b3c-710e-bd4213a24856"),
            column: "Filename",
            value: "{ELECTION_NAME} - Eingang Kommentar");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("64c50e44-f625-f533-5a88-bab71c3136db"),
            column: "Filename",
            value: "{ELECTION_NAME} - Kandidaten (Bundeskanzlei).csv");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            columns: new[] { "Content", "Filename" },
            values: new object[] { @"@using RazorLight
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayout"";
    var entries = Model.Election.QuorumSignaturesCount ?? 200;
    var entriesPerPage = 10;
    var pages = entries / entriesPerPage;
    
    var tenantTitle = Model.GetInfoText(""tenantTitle"");
    var hasTenantLogo = string.IsNullOrEmpty(tenantTitle) && ViewBag.Model.Election.TenantLogo != null;
}

@section HeaderStyles {
    @if (hasTenantLogo)
    {
           <text>
           .page {
               background-image: url('@ViewBag.Model.ElectionTenantLogoDataURI');
               background-repeat: no-repeat;
               background-position: top right;
               background-size: 75px;
           }
           </text>
    }
}

@for (var page = 1; page <= pages; page++)
{
    <div class=""page"">
        <header class=""row space"">
            <h1 class=""preserve-whitespace"">@Model.Election.Name</h1>
            @if (!hasTenantLogo)
            {
                <h1>@tenantTitle</h1>
            }
        </header>
        <main>
            <div class=""row stretch"">
                <div class=""grow"">
                    <div class=""row space"">
                        <div class=""row col-main"">
                            <div>
                                @if (Model.IsMajorz)
                                {
                                    <span class=""bold"">Zur Wahl wird vorgeschlagen:</span>
                                }
                                else
                                {
                                    <span><span class=""bold"">Bezeichnung</span> des Wahlvorschlags:</span>
                                }
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
            @if (Model.IsProporz)
            {
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
            }
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
                                (inkl. allfälliger Titel)
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
                                <td class=""center"">@((page - 1) * entriesPerPage + i)</td>
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
                <div class=""preserve-whitespace"">@Model.GetInfoText(""formSignatories"")</div>
            </div>
        </main>
    </div>
}", "{ELECTION_NAME} - Unterzeichnende.pdf" });

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("7473694c-6143-646e-6964-61746573242d"),
            column: "Filename",
            value: "{ELECTION_NAME} - Kandidaten.pdf");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"),
            column: "Filename",
            value: "{ELECTION_NAME} - Kandidaten (leer).pdf");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("f5021674-afce-9c2c-a533-8a401bb3f2e4"),
            column: "Filename",
            value: "{ELECTION_NAME} - Statuswechsel Wahlvorschlag");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("0ea74799-d3fa-c8ea-b7e6-af290716b6c8"),
            columns: new[] { "Content", "Filename" },
            values: new object[] { @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System.Collections.Generic;
@using System;
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
Listennummer;Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatort(e) inkl. Kantonszugehörigkeit;Titel;Beruf(e);Politischer Beruf;@(Model.IsMajorz ? ""Partei;"" : """")Bisher@(Model.IsProporz ? "";Vorkumuliert"" : """")
@{
    const char CsvSeparator = ';';
    const char CsvSeparatorReplacement = ',';
    foreach (List list in Model.Election.Lists)
    {
        foreach (Candidate candidate in Model.GetClonedAndOrderedCandidates(list))
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
                candidate.Title,
                candidate.OccupationalTitle,
                candidate.BallotOccupationalTitle,
                candidate.Incumbent ? ""Ja"" : ""Nein""
            };

            if (Model.IsMajorz)
            {
                fields.Insert(fields.Count - 1, candidate.Party); // insert before ""Incumbent""
            }

            if (Model.IsProporz)
            {
                fields.Add(candidate.Cloned ? ""Ja"" : ""Nein"");
            }

            fields = fields.Select(f => f?.Replace(CsvSeparator, CsvSeparatorReplacement) ?? """").ToList();
            @Raw(string.Join(CsvSeparator, fields))
            @Raw(Environment.NewLine)
        }
    }
}", "@Model.Election.Name - Kandidaten.csv" });

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"),
            column: "Filename",
            value: "@Model.Election.Name - Listenverbindungen (leer).pdf");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("4bec31bf-5e45-7b3c-710e-bd4213a24856"),
            column: "Filename",
            value: "@Model.Election.Name - Eingang Kommentar");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("64c50e44-f625-f533-5a88-bab71c3136db"),
            column: "Filename",
            value: "@Model.Election.Name - Kandidaten (Bundeskanzlei).csv");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            columns: new[] { "Content", "Filename" },
            values: new object[] { @"@using RazorLight
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayout"";
    var entries = Model.Election.QuorumSignaturesCount ?? 200;
    var entriesPerPage = 10;
    var pages = entries / entriesPerPage;
    
    var tenantTitle = Model.GetInfoText(""tenantTitle"");
    var hasTenantLogo = string.IsNullOrEmpty(tenantTitle) && ViewBag.Model.Election.TenantLogo != null;
}

@section HeaderStyles {
    @if (hasTenantLogo)
    {
           <text>
           .page {
               background-image: url('@ViewBag.Model.ElectionTenantLogoDataURI');
               background-repeat: no-repeat;
               background-position: top right;
               background-size: 75px;
           }
           </text>
    }
}

@for (var page = 1; page <= pages; page++)
{
    <div class=""page"">
        <header class=""row space"">
            <h1 class=""preserve-whitespace"">@Model.Election.Name</h1>
            @if (!hasTenantLogo)
            {
                <h1>@tenantTitle</h1>
            }
        </header>
        <main>
            <div class=""row stretch"">
                <div class=""grow"">
                    <div class=""row space"">
                        <div class=""row col-main"">
                            <div>
                                @if (Model.IsMajorz)
                                {
                                    <span class=""bold"">Zur Wahl wird vorgeschlagen:</span>
                                }
                                else
                                {
                                    <span><span class=""bold"">Bezeichnung</span> des Wahlvorschlags:</span>
                                }
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
            @if (Model.IsProporz)
            {
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
            }
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
                                (inkl. allfälliger Titel)
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
                <div class=""preserve-whitespace"">@Model.GetInfoText(""formSignatories"")</div>
            </div>
        </main>
    </div>
}", "@Model.Election.Name - Unterzeichnende.pdf" });

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("7473694c-6143-646e-6964-61746573242d"),
            column: "Filename",
            value: "@Model.Election.Name - Kandidaten.pdf");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"),
            column: "Filename",
            value: "@Model.Election.Name - Kandidaten (leer).pdf");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("f5021674-afce-9c2c-a533-8a401bb3f2e4"),
            column: "Filename",
            value: "@Model.Election.Name - Statuswechsel Wahlvorschlag");
    }
}
