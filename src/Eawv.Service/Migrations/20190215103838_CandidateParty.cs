// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class CandidateParty : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Party",
            table: "Candidates",
            nullable: true);

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
}");

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("3464ef11-14e5-9cbb-fcc5-fd31fc1704fd"),
            column: "Content",
            value: @"<html>
  <head>
    <meta charset=""UTF-8"" />
    <style>
      body {
        margin: 0;
        padding: 0;
        position: relative;
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
      }

      @(""@"")media only screen {
        body {
          background-color: gray;
          padding: 1cm;
        }

        .doc {
          background-color: white;
        }
      }

      h1,
      h2,
      h3,
      h4,
      h5 {
        margin: 0;
      }

      h1 {
        margin-bottom: 1mm;
      }

      .input {
        height: 1rem;
        position: relative;
        display: inline-block;
        padding: 0 1mm;
        box-sizing: border-box;
      }
      .input .border {
        border: 1px solid #999;
        border-top: none;
        height: 1mm;
        margin-top: 3mm;
        margin: 3mm 0.5mm 0.5mm 0.5mm;
        bottom: 0;
        position: absolute;
        left: 0;
        right: 0;
      }
      input {
        border: none;
        width: 100%;
        position: relative;
        bottom: 1mm;
      }
      .inline-input {
        bottom: -.5mm;
      }

      .w-2 {
        width: 2rem;
      }
      .w-4 {
        width: 4rem;
      }
      .w-6 {
        width: 6rem;
      }
      .w-12 {
        width: 12rem;
      }
      .w-16 {
        width: 16rem;
      }
      .w-24 {
        width: 24rem;
      }

      .col-main {
        width: 20cm;
      }

      .col-right {
        width: 8.5cm;
      }

      .label {
        font-size: 1.1rem;
        font-weight: 700;
        color: #999;
        margin-right: 0.2rem;
        position: relative;
        top: -1mm;
      }

      .size-A4.landscape {
        width: 297mm;
      }

      .size-A4 {
        width: 210mm;
      }

      .page {
        page-break-after: always;
      }

      .hint {
        font-size: 8pt;
        color: #555;
      }

      .filled {
        background-color: #dadada;
      }

      .small {
        font-size: 7pt;
      }
      .bold {
        font-weight: 700;
      }
      .super {
        vertical-align: super;
      }

      .row {
        display: flex;
        flex-direction: row;
        margin-bottom: 0.15rem;
      }
      .col {
        display: flex;
        flex-direction: column;
      }
      .stretch {
        justify-content: stretch;
      }
      .space {
        justify-content: space-between;
      }
      .grow {
        flex-grow: 1;
      }
      .no-shrink {
        flex-shrink: 0;
      }
      .end {
        justify-content: flex-end;
      }
      .items-end {
        align-items: flex-end;
      }
      .center {
        justify-content: center;
      }
      .right {
        text-align: right;
      }
      .inline {
        display: inline-block;
        margin: 0;
      }
      .fullwidth {
        width: 100%;
      }
      .spacer {
        display: inline-block;
        width: 0.5rem;
      }
      .space-bottom {
        margin-bottom: 1.5mm;
      }
      .space-right {
        margin-right: 2mm;
      }

      .upright {
        writing-mode: vertical-rl;
        transform: rotate(-180deg);
      }

      table {
        margin-bottom: 1.5mm;
      }

      table,
      th,
      td {
        font-size: 11px;
        border-collapse: collapse;
        border: 1px solid #000;
      }

      th {
        background-color: #dadada;
        vertical-align: top;
      }

      th,
      td {
        padding: 1mm;
      }

      thead tr > tr th {
        font-weight: 400;
      }

      thead {
        height: 200px;
      }
      tbody {
        background-color: #fff;
      }
      tbody td {
        border-top: 1px dashed #000;
        border-bottom: 1px dashed #000;
      }

      tbody tr {
        height: 1.2cm;
      }

      tbody tr > td.right {
        text-align: right;
      }

      tbody tr > td.center {
        text-align: center;
      }

      .top-no-border th,
      .top-no-border tr {
        border-top: none;
      }
      .bot-no-border {
        border-bottom: none;
      }

      .center-placeholder::placeholder {
        text-align: center;
        color: #000;
      }

      .version {
        align-items: flex-end;
        justify-content: flex-end;
        display: flex;
      }

      .checkbox-checked {
        background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAy0lEQVQ4y6WTIQ7CMBiFv9duOwQCsVssQSA4A0dAoEZCEAgcAsc1OAkKiZ1A4BHIrpiSMNgYbDVN27zvf31/K8DTY0Rh3gPHP7UZkBMcTDsUnwLe9LEP8BfAWjvsDJC0cs6drbWD1/3oR3Huvd9JWjjnru/nlRAljZMkiV7Wc8BLWtaFWAEEe3fgEMdxImkWxOsaY5+AUHESIKcg3jTcrB4QICPgBmy/RNMMADDGDNM0NW2Axi6UZXkpiqK1Q71f4tNB1kGbAYie3/kBVdgzl8pet1AAAAAASUVORK5CYII=');
        width: 12px;
        height: 12px;
        background-size: contain;
        display: inline-block;
      }

      .checkbox-unchecked {
        background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAR0lEQVQ4y+3TMRXAIBRD0UtPTX0j6KgUXFUWLCCg/KUDWTLlJUsKuoTu6Q3vx2zgMRfUjfKKfmXmwwEcwD8A60yxkQ0oknceO48HO4dlIIgAAAAASUVORK5CYII=');
        width: 12px;
        height: 12px;
        background-size: contain;
        display: inline-block;
      }

      .preserve-whitespace {
        white-space: pre-wrap;
      }
      
      .no-page-break {
        page-break-inside: avoid;
      }

      footer.pdf {
        width: 100%;
        padding-right: 0.75cm;
        @* needed according to https://github.com/GoogleChrome/puppeteer/issues/2104 *@
        font-size: calc(8pt * 210/297);
        font-family: Arial, Helvetica, sans-serif;
      }

      footer.pdf div {
        width: 100%;
        text-align: right;
      }
      
      @RenderSection(""HeaderStyles"", false)
      @RenderSection(""Styles"", false)
    </style>
</head>
<body>
<div class=""doc size-@ViewBag.Model.Template.Format @(ViewBag.Model.Template.Landscape ? ""landscape"" : """")"">
  <header class=""pdf""><span></span></header><!-- enforce empty pdf header -->
  @RenderBody()
  <footer class=""pdf"">
    <div>Seite @ViewBag.Model.CurrentPage / @ViewBag.Model.TotalPages</div>
  </footer>
</div>
</body>
</html>
");

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

        @if (Model.IsMajorz)
        {
            <div class=""row space"">
                <div class=""row col-main space"">
                    <div class=""flex"">
                        <span class=""bold"">Eingang Staatskanzlei</span> (wird von Staatskanzlei ausgefüllt):
                        <div class=""input inline-input w-6"">
                            <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")"" />
                            <div class=""border""></div>
                        </div>
                        <div class=""input inline-input w-4"">
                            <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")"" />
                            <div class=""border""></div>
                        </div>
                        Uhr
                    </div>
                    <div class=""flex"">
                        <div class=""input inline-input w-6"">
                            <input value="""" />
                            <div class=""border""></div>
                        </div>
                        Visum Staatskanzlei
                    </div>
                </div>
            </div>
        }


        @if (Model.IsProporz)
        {
            <div class=""row space"">
                <div class=""row col-main"">
                    <div>
                        Evtl. <span class=""bold"">Präzisierung</span> nach Alter,
                        Geschlecht, Region oder Parteiflügel:
                    </div>
                    <div class=""input grow"">
                        <input value=""@Model.List.Description"" />
                        <div class=""border""></div>
                    </div>
                </div>
            </div>
            <div class=""row col-main space"">
                <div class=""flex"">
                    <span class=""bold"">Listennummer</span> (wird vom Kanton zugeteilt):
                    <div class=""input inline-input w-4"">
                        <input value=""@Model.List.Indenture"" />
                        <div class=""border""></div>
                    </div>
                </div>
                <div class=""flex"">
                    Einreichedatum:
                    <div class=""input inline-input w-6"">
                        <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")"" />
                        <div class=""border""></div>
                    </div>
                    <div class=""input inline-input w-4"">
                        <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")"" />
                        <div class=""border""></div>
                    </div>
                    Uhr
                </div>
            </div>
        }
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
        @if (Model.IsProporz)
        {
            <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
        }
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
            @if (Model.IsMajorz)
            {
                <th rowspan=""2"">
                    <span class=""upright"">Partei</span>
                </th>
            }
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
                    @if (Model.IsMajorz)
                    {
                        <td></td>
                    }
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
                    @if (Model.IsMajorz)
                    {
                        <td>@candidate.Party</td>
                    }
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

        @if (Model.IsMajorz)
        {
            <div class=""row space"">
                <div class=""row col-main space"">
                    <div class=""flex"">
                        <span class=""bold"">Eingang Staatskanzlei</span> (wird von Staatskanzlei ausgefüllt):
                        <div class=""input inline-input w-6"">
                            <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")"" />
                            <div class=""border""></div>
                        </div>
                        <div class=""input inline-input w-4"">
                            <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")"" />
                            <div class=""border""></div>
                        </div>
                        Uhr
                    </div>
                    <div class=""flex"">
                        <div class=""input inline-input w-6"">
                            <input value="""" />
                            <div class=""border""></div>
                        </div>
                        Visum Staatskanzlei
                    </div>
                </div>
            </div>
        }


        @if (Model.IsProporz)
        {
            <div class=""row space"">
                <div class=""row col-main"">
                    <div>
                        Evtl. <span class=""bold"">Präzisierung</span> nach Alter,
                        Geschlecht, Region oder Parteiflügel:
                    </div>
                    <div class=""input grow"">
                        <input value=""@Model.List.Description"" />
                        <div class=""border""></div>
                    </div>
                </div>
            </div>
            <div class=""row col-main space"">
                <div class=""flex"">
                    <span class=""bold"">Listennummer</span> (wird vom Kanton zugeteilt):
                    <div class=""input inline-input w-4"">
                        <input value=""@Model.List.Indenture"" />
                        <div class=""border""></div>
                    </div>
                </div>
                <div class=""flex"">
                    Einreichedatum:
                    <div class=""input inline-input w-6"">
                        <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""d"") : """")"" />
                        <div class=""border""></div>
                    </div>
                    <div class=""input inline-input w-4"">
                        <input value=""@(Model.List.SubmitDate.HasValue ? Model.List.SubmitDate.Value.ToString(""t"") : """")"" />
                        <div class=""border""></div>
                    </div>
                    Uhr
                </div>
            </div>
        }
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
        @if (Model.IsProporz)
        {
            <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
        }
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
            @if (Model.IsMajorz)
            {
                <th rowspan=""2"">
                    <span class=""upright"">Partei</span>
                </th>
            }
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
                    @if (Model.IsMajorz)
                    {
                        <td></td>
                    }
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
                    @if (Model.IsMajorz)
                    {
                        <td>@candidate.Party</td>
                    }
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
        migrationBuilder.DropColumn(
            name: "Party",
            table: "Candidates");

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
            keyValue: new Guid("3464ef11-14e5-9cbb-fcc5-fd31fc1704fd"),
            column: "Content",
            value: @"<html>
  <head>
    <meta charset=""UTF-8"" />
    <style>
      body {
        margin: 0;
        padding: 0;
        position: relative;
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
      }

      @(""@"")media only screen {
        body {
          background-color: gray;
          padding: 1cm;
        }

        .doc {
          background-color: white;
        }
      }

      h1,
      h2,
      h3,
      h4,
      h5 {
        margin: 0;
      }

      h1 {
        margin-bottom: 1mm;
      }

      .input {
        height: 1rem;
        position: relative;
        display: inline-block;
        padding: 0 1mm;
        box-sizing: border-box;
      }
      .input .border {
        border: 1px solid #999;
        border-top: none;
        height: 1mm;
        margin-top: 3mm;
        margin: 3mm 0.5mm 0.5mm 0.5mm;
        bottom: 0;
        position: absolute;
        left: 0;
        right: 0;
      }
      input {
        border: none;
        width: 100%;
        position: relative;
        bottom: 1mm;
      }
      .inline-input {
        bottom: -.5mm;
      }

      .w-2 {
        width: 2rem;
      }
      .w-4 {
        width: 4rem;
      }
      .w-6 {
        width: 6rem;
      }
      .w-12 {
        width: 12rem;
      }
      .w-16 {
        width: 16rem;
      }
      .w-24 {
        width: 24rem;
      }

      .col-main {
        width: 19cm;
      }

      .col-right {
        width: 8.5cm;
      }

      .label {
        font-size: 1.1rem;
        font-weight: 700;
        color: #999;
        margin-right: 0.2rem;
        position: relative;
        top: -1mm;
      }

      .size-A4.landscape {
        width: 297mm;
      }

      .size-A4 {
        width: 210mm;
      }

      .page {
        page-break-after: always;
      }

      .hint {
        font-size: 8pt;
        color: #555;
      }

      .filled {
        background-color: #dadada;
      }

      .small {
        font-size: 7pt;
      }
      .bold {
        font-weight: 700;
      }
      .super {
        vertical-align: super;
      }

      .row {
        display: flex;
        flex-direction: row;
        margin-bottom: 0.15rem;
      }
      .col {
        display: flex;
        flex-direction: column;
      }
      .stretch {
        justify-content: stretch;
      }
      .space {
        justify-content: space-between;
      }
      .grow {
        flex-grow: 1;
      }
      .no-shrink {
        flex-shrink: 0;
      }
      .end {
        justify-content: flex-end;
      }
      .items-end {
        align-items: flex-end;
      }
      .center {
        justify-content: center;
      }
      .right {
        text-align: right;
      }
      .inline {
        display: inline-block;
        margin: 0;
      }
      .fullwidth {
        width: 100%;
      }
      .spacer {
        display: inline-block;
        width: 0.5rem;
      }
      .space-bottom {
        margin-bottom: 1.5mm;
      }
      .space-right {
        margin-right: 2mm;
      }

      .upright {
        writing-mode: vertical-rl;
        transform: rotate(-180deg);
      }

      table {
        margin-bottom: 1.5mm;
      }

      table,
      th,
      td {
        font-size: 11px;
        border-collapse: collapse;
        border: 1px solid #000;
      }

      th {
        background-color: #dadada;
        vertical-align: top;
      }

      th,
      td {
        padding: 1mm;
      }

      thead tr > tr th {
        font-weight: 400;
      }

      thead {
        height: 200px;
      }
      tbody {
        background-color: #fff;
      }
      tbody td {
        border-top: 1px dashed #000;
        border-bottom: 1px dashed #000;
      }

      tbody tr {
        height: 1.2cm;
      }

      tbody tr > td.right {
        text-align: right;
      }

      tbody tr > td.center {
        text-align: center;
      }

      .top-no-border th,
      .top-no-border tr {
        border-top: none;
      }
      .bot-no-border {
        border-bottom: none;
      }

      .center-placeholder::placeholder {
        text-align: center;
        color: #000;
      }

      .version {
        align-items: flex-end;
        justify-content: flex-end;
        display: flex;
      }

      .checkbox-checked {
        background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAy0lEQVQ4y6WTIQ7CMBiFv9duOwQCsVssQSA4A0dAoEZCEAgcAsc1OAkKiZ1A4BHIrpiSMNgYbDVN27zvf31/K8DTY0Rh3gPHP7UZkBMcTDsUnwLe9LEP8BfAWjvsDJC0cs6drbWD1/3oR3Huvd9JWjjnru/nlRAljZMkiV7Wc8BLWtaFWAEEe3fgEMdxImkWxOsaY5+AUHESIKcg3jTcrB4QICPgBmy/RNMMADDGDNM0NW2Axi6UZXkpiqK1Q71f4tNB1kGbAYie3/kBVdgzl8pet1AAAAAASUVORK5CYII=');
        width: 12px;
        height: 12px;
        background-size: contain;
        display: inline-block;
      }

      .checkbox-unchecked {
        background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAR0lEQVQ4y+3TMRXAIBRD0UtPTX0j6KgUXFUWLCCg/KUDWTLlJUsKuoTu6Q3vx2zgMRfUjfKKfmXmwwEcwD8A60yxkQ0oknceO48HO4dlIIgAAAAASUVORK5CYII=');
        width: 12px;
        height: 12px;
        background-size: contain;
        display: inline-block;
      }

      .preserve-whitespace {
        white-space: pre-wrap;
      }
      
      .no-page-break {
        page-break-inside: avoid;
      }

      footer.pdf {
        width: 100%;
        padding-right: 0.75cm;
        @* needed according to https://github.com/GoogleChrome/puppeteer/issues/2104 *@
        font-size: calc(8pt * 210/297);
        font-family: Arial, Helvetica, sans-serif;
      }

      footer.pdf div {
        width: 100%;
        text-align: right;
      }
      
      @RenderSection(""HeaderStyles"", false)
      @RenderSection(""Styles"", false)
    </style>
</head>
<body>
<div class=""doc size-@ViewBag.Model.Template.Format @(ViewBag.Model.Template.Landscape ? ""landscape"" : """")"">
  <header class=""pdf""><span></span></header><!-- enforce empty pdf header -->
  @RenderBody()
  <footer class=""pdf"">
    <div>Seite @ViewBag.Model.CurrentPage / @ViewBag.Model.TotalPages</div>
  </footer>
</div>
</body>
</html>
");

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
}
