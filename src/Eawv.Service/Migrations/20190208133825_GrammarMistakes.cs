// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class GrammarMistakes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("0ea74799-d3fa-c8ea-b7e6-af290716b6c8"),
            column: "Content",
            value: @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System.Collections.Generic;
@using System;
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
Listennummer;Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatort(e) inkl. Kantonszugehörigkeit;Titel;Beruf(e);Politischer Beruf;Bisher;Vorkumuliert
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
                candidate.Incumbent ? ""Ja"" : ""Nein"",
                candidate.Cloned ? ""Ja"" : ""Nein""
            }
            .Select(f => f?.Replace(CsvSeparator, CsvSeparatorReplacement) ?? """");
            @Raw(string.Join(CsvSeparator, fields))
            @Raw(Environment.NewLine)
        }
    }
}");

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
           }
           </text>
    }
}

@for (var page = 1; page <= pages; page++)
{
    <div class=""page"">
        <header class=""row space"">
            <h1>@Model.Election.Name vom @Model.Election.ContestDate.ToString(""dd. MMMM yyyy"")</h1>
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
}");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("7473694c-6143-646e-6964-61746573242d"),
            column: "Content",
            value: @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayoutHeader"";
    // show signature column only for lists not in draft state or pseudo lists                                                                       
    var showSignatureColumn = !Model.ListIsDraft || Model.List.IsNew;
}
<main>
<div class=""row stretch"">
    <span class=""label space-right"">A</span>
    <div class=""grow"">
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    <span class=""bold"">Bezeichnung</span> des Wahlvorschlags:
                </div>
                <div class=""grow col"">
                    <div class=""input"">
                        <input value=""@Model.List.Name""/>
                        <div class=""border""></div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    Evtl. <span class=""bold"">Präzisierung</span> nach Alter,
                    Geschlecht, Region oder Parteiflügel:
                </div>
                <div class=""input grow"">
                    <input value=""@Model.List.Description""/>
                    <div class=""border""></div>
                </div>
            </div>
        </div>
        <div class=""row col-main space"">
            <div class=""flex"">
                <span class=""bold"">Listennummer</span> (wird vom Kanton zugeteilt):
                <div class=""input inline-input w-4"">
                    <input value=""@Model.List.Indenture""/>
                    <div class=""border""></div>
                </div>
            </div>
            <div class=""flex"">
                Einreichedatum:
                <div class=""input inline-input w-6"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")""/>
                    <div class=""border""></div>
                </div>
                <div class=""input inline-input w-4"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")""/>
                    <div class=""border""></div>
                </div>
                Uhr
            </div>
        </div>
    </div>
</div>
<div>
    <div class=""row space"">
        <div class=""row"">
            <span class=""label space-right"">B</span>
            <div>
                <h2 class=""inline"">Kandidaturen</h2>
                @(Model.List.IsNew ? ""(Bitte in Blockschrift ergänzen)"" : """")
            </div>
        </div>
        <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
    </div>
    <table class=""fullwidth"">
        <thead>
        <tr>
            <th rowspan=""2"">Nr.</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Name</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Vorname</th>
            <th rowspan=""2"">
                <span class=""upright"">Geschlecht (m/w)</span>
            </th>
            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
            <th rowspan=""2"">Heimatort(e) inkl. Kantonszugehörigkeit</th>
            <th rowspan=""2"">
                <span class=""upright"">Bisher</span>
            </th>
            @if (showSignatureColumn)
            {
                <th rowspan=""2"" style=""width: 3.5cm"">
                    Unterschrift<br/>der Kandidierenden
                </th>
            }
            <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
        </tr>
        <tr class=""top-no-border"">
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
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
                inkl. allfälliger Titel und weiterer Bezeichnungen für
                Stimmzettel
            </th>
            <th style=""width: 3cm;"">Strasse/Nr.</th>
            <th>PLZ</th>
            <th>Wohnort</th>
        </tr>
        </thead>
        <tbody>
        @foreach (var candidate in Model.GetClonedAndOrderedCandidates())
        {
            <tr>
                <td class=""filled right"">@candidate.Index</td>
                @if (candidate.IsNew)
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
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
                }
                else
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FamilyName</div>
                        <div>@candidate.BallotFamilyName</div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FirstName</div>
                        <div>@candidate.BallotFirstName</div>
                    </td>
                    <td class=""center"">
                        @switch (candidate.Sex)
                        {
                            case SexType.Female:
                                <span>w</span>
                                break;
                            case SexType.Male:
                                <span>m</span>
                                break;
                        }
                    </td>
                    <td class=""center"">@candidate.DateOfBirth.Day</td>
                    <td class=""center"">@candidate.DateOfBirth.Month</td>
                    <td class=""center"">@candidate.DateOfBirth.Year</td>
                    <td>@(Model.Settings.ShowBallotPaperInfos ?
                              candidate.BallotOccupationalTitle :
                              string.IsNullOrWhiteSpace(candidate.Title) ?
                                  candidate.OccupationalTitle :
                                  $""{candidate.Title}, {candidate.OccupationalTitle}"")</td>
                    <td>@candidate.Street @candidate.HouseNumber</td>
                    <td>@candidate.ZipCode</td>
                    <td>@candidate.Locality</td>
                    <td>@candidate.Origin</td>
                    <!-- Bisher -->
                    <td class=""center""><span class=""@(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></span></td>
                }
                @if (showSignatureColumn)
                {
                    <td></td>
                }
                <td class=""filled""></td>
            </tr>
        }
        </tbody>
    </table>
</div>
<div class=""no-page-break"">
    <div class=""row space-bottom"">
        <div class=""w-12"">Vertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
    <div class=""row space-bottom"">
        <div class=""w-12"">Stellvertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
</div>
<div class=""space-bottom preserve-whitespace"">
@Model.GetInfoText(""formCandidacies"")
</div>
@if (!Model.List.IsNew)
{
    <div class=""row stretch"">
        <div class=""grow version hint"">Version @Model.List.Version</div>
    </div>
}
</main>");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"),
            column: "Content",
            value: @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayoutHeader"";
    // show signature column only for lists not in draft state or pseudo lists                                                                       
    var showSignatureColumn = !Model.ListIsDraft || Model.List.IsNew;
}
<main>
<div class=""row stretch"">
    <span class=""label space-right"">A</span>
    <div class=""grow"">
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    <span class=""bold"">Bezeichnung</span> des Wahlvorschlags:
                </div>
                <div class=""grow col"">
                    <div class=""input"">
                        <input value=""@Model.List.Name""/>
                        <div class=""border""></div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    Evtl. <span class=""bold"">Präzisierung</span> nach Alter,
                    Geschlecht, Region oder Parteiflügel:
                </div>
                <div class=""input grow"">
                    <input value=""@Model.List.Description""/>
                    <div class=""border""></div>
                </div>
            </div>
        </div>
        <div class=""row col-main space"">
            <div class=""flex"">
                <span class=""bold"">Listennummer</span> (wird vom Kanton zugeteilt):
                <div class=""input inline-input w-4"">
                    <input value=""@Model.List.Indenture""/>
                    <div class=""border""></div>
                </div>
            </div>
            <div class=""flex"">
                Einreichedatum:
                <div class=""input inline-input w-6"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")""/>
                    <div class=""border""></div>
                </div>
                <div class=""input inline-input w-4"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")""/>
                    <div class=""border""></div>
                </div>
                Uhr
            </div>
        </div>
    </div>
</div>
<div>
    <div class=""row space"">
        <div class=""row"">
            <span class=""label space-right"">B</span>
            <div>
                <h2 class=""inline"">Kandidaturen</h2>
                @(Model.List.IsNew ? ""(Bitte in Blockschrift ergänzen)"" : """")
            </div>
        </div>
        <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
    </div>
    <table class=""fullwidth"">
        <thead>
        <tr>
            <th rowspan=""2"">Nr.</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Name</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Vorname</th>
            <th rowspan=""2"">
                <span class=""upright"">Geschlecht (m/w)</span>
            </th>
            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
            <th rowspan=""2"">Heimatort(e) inkl. Kantonszugehörigkeit</th>
            <th rowspan=""2"">
                <span class=""upright"">Bisher</span>
            </th>
            @if (showSignatureColumn)
            {
                <th rowspan=""2"" style=""width: 3.5cm"">
                    Unterschrift<br/>der Kandidierenden
                </th>
            }
            <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
        </tr>
        <tr class=""top-no-border"">
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
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
                inkl. allfälliger Titel und weiterer Bezeichnungen für
                Stimmzettel
            </th>
            <th style=""width: 3cm;"">Strasse/Nr.</th>
            <th>PLZ</th>
            <th>Wohnort</th>
        </tr>
        </thead>
        <tbody>
        @foreach (var candidate in Model.GetClonedAndOrderedCandidates())
        {
            <tr>
                <td class=""filled right"">@candidate.Index</td>
                @if (candidate.IsNew)
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
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
                }
                else
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FamilyName</div>
                        <div>@candidate.BallotFamilyName</div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FirstName</div>
                        <div>@candidate.BallotFirstName</div>
                    </td>
                    <td class=""center"">
                        @switch (candidate.Sex)
                        {
                            case SexType.Female:
                                <span>w</span>
                                break;
                            case SexType.Male:
                                <span>m</span>
                                break;
                        }
                    </td>
                    <td class=""center"">@candidate.DateOfBirth.Day</td>
                    <td class=""center"">@candidate.DateOfBirth.Month</td>
                    <td class=""center"">@candidate.DateOfBirth.Year</td>
                    <td>@(Model.Settings.ShowBallotPaperInfos ?
                              candidate.BallotOccupationalTitle :
                              string.IsNullOrWhiteSpace(candidate.Title) ?
                                  candidate.OccupationalTitle :
                                  $""{candidate.Title}, {candidate.OccupationalTitle}"")</td>
                    <td>@candidate.Street @candidate.HouseNumber</td>
                    <td>@candidate.ZipCode</td>
                    <td>@candidate.Locality</td>
                    <td>@candidate.Origin</td>
                    <!-- Bisher -->
                    <td class=""center""><span class=""@(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></span></td>
                }
                @if (showSignatureColumn)
                {
                    <td></td>
                }
                <td class=""filled""></td>
            </tr>
        }
        </tbody>
    </table>
</div>
<div class=""no-page-break"">
    <div class=""row space-bottom"">
        <div class=""w-12"">Vertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
    <div class=""row space-bottom"">
        <div class=""w-12"">Stellvertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
</div>
<div class=""space-bottom preserve-whitespace"">
@Model.GetInfoText(""formCandidacies"")
</div>
@if (!Model.List.IsNew)
{
    <div class=""row stretch"">
        <div class=""grow version hint"">Version @Model.List.Version</div>
    </div>
}
</main>");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("0ea74799-d3fa-c8ea-b7e6-af290716b6c8"),
            column: "Content",
            value: @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System.Collections.Generic;
@using System;
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
Listennummer;Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatort(e) inkl. Kanton(e);Titel;Beruf(e);Politischer Beruf;Bisher;Vorkumuliert
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
                candidate.Incumbent ? ""Ja"" : ""Nein"",
                candidate.Cloned ? ""Ja"" : ""Nein""
            }
            .Select(f => f?.Replace(CsvSeparator, CsvSeparatorReplacement) ?? """");
            @Raw(string.Join(CsvSeparator, fields))
            @Raw(Environment.NewLine)
        }
    }
}");

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
           }
           </text>
    }
}

@for (var page = 1; page <= pages; page++)
{
    <div class=""page"">
        <header class=""row space"">
            <h1>@Model.Election.Name vom @Model.Election.ContestDate.ToString(""dd. MMMM yyyy"")</h1>
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
                <div class=""preserve-whitespace"">@Model.GetInfoText(""formSignatories"")</div>
            </div>
        </main>
    </div>
}");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("7473694c-6143-646e-6964-61746573242d"),
            column: "Content",
            value: @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayoutHeader"";
    // show signature column only for lists not in draft state or pseudo lists                                                                       
    var showSignatureColumn = !Model.ListIsDraft || Model.List.IsNew;
}
<main>
<div class=""row stretch"">
    <span class=""label space-right"">A</span>
    <div class=""grow"">
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    <span class=""bold"">Bezeichnung</span> des Wahlvorschlags:
                </div>
                <div class=""grow col"">
                    <div class=""input"">
                        <input value=""@Model.List.Name""/>
                        <div class=""border""></div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    Evtl. <span class=""bold"">Präzisierung</span> nach Alter,
                    Geschlecht, Region oder Parteiflügel:
                </div>
                <div class=""input grow"">
                    <input value=""@Model.List.Description""/>
                    <div class=""border""></div>
                </div>
            </div>
        </div>
        <div class=""row col-main space"">
            <div class=""flex"">
                <span class=""bold"">Listennnummer</span> (wird vom Kanton zugeteilt):
                <div class=""input inline-input w-4"">
                    <input value=""@Model.List.Indenture""/>
                    <div class=""border""></div>
                </div>
            </div>
            <div class=""flex"">
                Einreichedatum:
                <div class=""input inline-input w-6"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")""/>
                    <div class=""border""></div>
                </div>
                <div class=""input inline-input w-4"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")""/>
                    <div class=""border""></div>
                </div>
                Uhr
            </div>
        </div>
    </div>
</div>
<div>
    <div class=""row space"">
        <div class=""row"">
            <span class=""label space-right"">B</span>
            <div>
                <h2 class=""inline"">Kandidaturen</h2>
                @(Model.List.IsNew ? ""(Bitte in Blockschrift ergänzen)"" : """")
            </div>
        </div>
        <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
    </div>
    <table class=""fullwidth"">
        <thead>
        <tr>
            <th rowspan=""2"">Nr.</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Name</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Vorname</th>
            <th rowspan=""2"">
                <span class=""upright"">Geschlecht (m/w)</span>
            </th>
            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
            <th rowspan=""2"">Heimatort(e) inkl. Kanton(e)</th>
            <th rowspan=""2"">
                <span class=""upright"">Bisher</span>
            </th>
            @if (showSignatureColumn)
            {
                <th rowspan=""2"" style=""width: 3.5cm"">
                    Unterschrift<br/>der Kandidierenden
                </th>
            }
            <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
        </tr>
        <tr class=""top-no-border"">
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
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
                inkl. allfällige Titel und weitere Bezeichnungen für
                Stimmzettel
            </th>
            <th style=""width: 3cm;"">Strasse/Nr.</th>
            <th>PLZ</th>
            <th>Wohnort</th>
        </tr>
        </thead>
        <tbody>
        @foreach (var candidate in Model.GetClonedAndOrderedCandidates())
        {
            <tr>
                <td class=""filled right"">@candidate.Index</td>
                @if (candidate.IsNew)
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
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
                }
                else
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FamilyName</div>
                        <div>@candidate.BallotFamilyName</div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FirstName</div>
                        <div>@candidate.BallotFirstName</div>
                    </td>
                    <td class=""center"">
                        @switch (candidate.Sex)
                        {
                            case SexType.Female:
                                <span>w</span>
                                break;
                            case SexType.Male:
                                <span>m</span>
                                break;
                        }
                    </td>
                    <td class=""center"">@candidate.DateOfBirth.Day</td>
                    <td class=""center"">@candidate.DateOfBirth.Month</td>
                    <td class=""center"">@candidate.DateOfBirth.Year</td>
                    <td>@(Model.Settings.ShowBallotPaperInfos ?
                              candidate.BallotOccupationalTitle :
                              string.IsNullOrWhiteSpace(candidate.Title) ?
                                  candidate.OccupationalTitle :
                                  $""{candidate.Title}, {candidate.OccupationalTitle}"")</td>
                    <td>@candidate.Street @candidate.HouseNumber</td>
                    <td>@candidate.ZipCode</td>
                    <td>@candidate.Locality</td>
                    <td>@candidate.Origin</td>
                    <!-- Bisher -->
                    <td class=""center""><span class=""@(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></span></td>
                }
                @if (showSignatureColumn)
                {
                    <td></td>
                }
                <td class=""filled""></td>
            </tr>
        }
        </tbody>
    </table>
</div>
<div class=""no-page-break"">
    <div class=""row space-bottom"">
        <div class=""w-12"">Vertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
    <div class=""row space-bottom"">
        <div class=""w-12"">Stellvertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
</div>
<div class=""space-bottom preserve-whitespace"">
@Model.GetInfoText(""formCandidacies"")
</div>
@if (!Model.List.IsNew)
{
    <div class=""row stretch"">
        <div class=""grow version hint"">Version @Model.List.Version</div>
    </div>
}
</main>");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"),
            column: "Content",
            value: @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayoutHeader"";
    // show signature column only for lists not in draft state or pseudo lists                                                                       
    var showSignatureColumn = !Model.ListIsDraft || Model.List.IsNew;
}
<main>
<div class=""row stretch"">
    <span class=""label space-right"">A</span>
    <div class=""grow"">
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    <span class=""bold"">Bezeichnung</span> des Wahlvorschlags:
                </div>
                <div class=""grow col"">
                    <div class=""input"">
                        <input value=""@Model.List.Name""/>
                        <div class=""border""></div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row space"">
            <div class=""row col-main"">
                <div>
                    Evtl. <span class=""bold"">Präzisierung</span> nach Alter,
                    Geschlecht, Region oder Parteiflügel:
                </div>
                <div class=""input grow"">
                    <input value=""@Model.List.Description""/>
                    <div class=""border""></div>
                </div>
            </div>
        </div>
        <div class=""row col-main space"">
            <div class=""flex"">
                <span class=""bold"">Listennnummer</span> (wird vom Kanton zugeteilt):
                <div class=""input inline-input w-4"">
                    <input value=""@Model.List.Indenture""/>
                    <div class=""border""></div>
                </div>
            </div>
            <div class=""flex"">
                Einreichedatum:
                <div class=""input inline-input w-6"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")""/>
                    <div class=""border""></div>
                </div>
                <div class=""input inline-input w-4"">
                    <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")""/>
                    <div class=""border""></div>
                </div>
                Uhr
            </div>
        </div>
    </div>
</div>
<div>
    <div class=""row space"">
        <div class=""row"">
            <span class=""label space-right"">B</span>
            <div>
                <h2 class=""inline"">Kandidaturen</h2>
                @(Model.List.IsNew ? ""(Bitte in Blockschrift ergänzen)"" : """")
            </div>
        </div>
        <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
    </div>
    <table class=""fullwidth"">
        <thead>
        <tr>
            <th rowspan=""2"">Nr.</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Name</th>
            <th class=""bot-no-border"" style=""width: 3cm"">Vorname</th>
            <th rowspan=""2"">
                <span class=""upright"">Geschlecht (m/w)</span>
            </th>
            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
            <th rowspan=""2"">Heimatort(e) inkl. Kanton(e)</th>
            <th rowspan=""2"">
                <span class=""upright"">Bisher</span>
            </th>
            @if (showSignatureColumn)
            {
                <th rowspan=""2"" style=""width: 3.5cm"">
                    Unterschrift<br/>der Kandidierenden
                </th>
            }
            <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
        </tr>
        <tr class=""top-no-border"">
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
            <th style=""width: 3cm;"">
                <em>1. Zeile Amtlich</em><br />
                <em>2. Zeile Politisch</em>
            </th>
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
                inkl. allfällige Titel und weitere Bezeichnungen für
                Stimmzettel
            </th>
            <th style=""width: 3cm;"">Strasse/Nr.</th>
            <th>PLZ</th>
            <th>Wohnort</th>
        </tr>
        </thead>
        <tbody>
        @foreach (var candidate in Model.GetClonedAndOrderedCandidates())
        {
            <tr>
                <td class=""filled right"">@candidate.Index</td>
                @if (candidate.IsNew)
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;""></div>
                        <div></div>
                    </td>
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
                }
                else
                {
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FamilyName</div>
                        <div>@candidate.BallotFamilyName</div>
                    </td>
                    <td>
                        <div style=""border-bottom: 1px solid #999;"">@candidate.FirstName</div>
                        <div>@candidate.BallotFirstName</div>
                    </td>
                    <td class=""center"">
                        @switch (candidate.Sex)
                        {
                            case SexType.Female:
                                <span>w</span>
                                break;
                            case SexType.Male:
                                <span>m</span>
                                break;
                        }
                    </td>
                    <td class=""center"">@candidate.DateOfBirth.Day</td>
                    <td class=""center"">@candidate.DateOfBirth.Month</td>
                    <td class=""center"">@candidate.DateOfBirth.Year</td>
                    <td>@(Model.Settings.ShowBallotPaperInfos ?
                              candidate.BallotOccupationalTitle :
                              string.IsNullOrWhiteSpace(candidate.Title) ?
                                  candidate.OccupationalTitle :
                                  $""{candidate.Title}, {candidate.OccupationalTitle}"")</td>
                    <td>@candidate.Street @candidate.HouseNumber</td>
                    <td>@candidate.ZipCode</td>
                    <td>@candidate.Locality</td>
                    <td>@candidate.Origin</td>
                    <!-- Bisher -->
                    <td class=""center""><span class=""@(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></span></td>
                }
                @if (showSignatureColumn)
                {
                    <td></td>
                }
                <td class=""filled""></td>
            </tr>
        }
        </tbody>
    </table>
</div>
<div class=""no-page-break"">
    <div class=""row space-bottom"">
        <div class=""w-12"">Vertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
    <div class=""row space-bottom"">
        <div class=""w-12"">Stellvertretung des Wahlvorschlags: </div>
        <div>
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                <div class=""border""></div>
            </div>
            Unterschrift:
            <div class=""input inline-input w-16"">
                <input />
                <div class=""border""></div>
            </div>
        </div>
    </div>
</div>
<div class=""space-bottom preserve-whitespace"">
@Model.GetInfoText(""formCandidacies"")
</div>
@if (!Model.List.IsNew)
{
    <div class=""row stretch"">
        <div class=""grow version hint"">Version @Model.List.Version</div>
    </div>
}
</main>");
    }
}
