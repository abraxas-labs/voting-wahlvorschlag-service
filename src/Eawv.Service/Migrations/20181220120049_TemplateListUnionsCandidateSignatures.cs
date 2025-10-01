// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class TemplateListUnionsCandidateSignatures : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("709a8ef6-9d5e-c029-ddaa-346ea4021b43"));

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("7473694c-6143-646e-6964-617465732d00"));

        migrationBuilder.AddColumn<string>(
            name: "Key",
            table: "Templates",
            nullable: true);

        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Key", "Landscape", "TenantId", "Type" },
            values: new object[,]
            {
                { new Guid("3464ef11-14e5-9cbb-fcc5-fd31fc1704fd"), @"<html>
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
                        height: 1cm;
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
                      
                      .checkbox-checked::after {
                        content: '☑';
                        font-size: 22px;
                      }
                      
                      .checkbox-unchecked::after {
                        content: '☐';
                        font-size: 22px;
                      }
                      
                      @RenderSection(""Styles"", false)
                    </style>
                  </head>
                <body>
                <div class=""doc size-@ViewBag.Model.Template.Format @(ViewBag.Model.Template.Landscape ? ""landscape"" : """")"">
                  @RenderBody()
                </div>
                </body>
                </html>
                ", null, 0, "BaseLayout", false, null, 0 },
                { new Guid("a148bd7e-0f06-7117-5de1-6956113f1b06"), @"@{
                    Layout = ""BaseLayout"";
                }
                <header class=""row space"">
                    <h1>@ViewBag.Model.Election.Name vom @ViewBag.Model.Election.ContestDate.ToString(""dd. MMMM y"")</h1>
                    <h1>@ViewBag.Model.ElectionTenant.Name</h1>
                    @RenderSection(""Header"", false)
                </header>
                @RenderBody()", null, 0, "BaseLayoutHeader", false, null, 0 },
                { new Guid("7473694c-6143-646e-6964-61746573242d"), @"@using Eawv.Service.DataAccess.Entities
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
                            @if (Model.List.SubmitDate.HasValue)
                            {
                                <div class=""flex"">
                                    Einreichedatum:
                                    <div class=""input inline-input w-6"">
                                        <input value=""@Model.List.SubmitDate.Value.ToString(""d"")""/>
                                        <div class=""border""></div>
                                    </div>
                                    <div class=""input inline-input w-4"">
                                        <input value=""@Model.List.SubmitDate.Value.ToString(""t"")""/>
                                        <div class=""border""></div>
                                    </div>
                                    Uhr
                                </div>
                            }
                        </div>
                    </div>
                </div>
                <div>
                    <div class=""row space"">
                        <div class=""row"">
                            <span class=""label space-right"">B</span>
                            <div>
                                <h2 class=""inline"">Kandidaturen</h2>
                            </div>
                        </div>
                        <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
                    </div>
                    <table class=""fullwidth"">
                        <thead>
                        <tr>
                            <th rowspan=""2"">Nr.</th>
                            <th rowspan=""2"" style=""width: 3cm"">Name</th>
                            <th rowspan=""2"" style=""width: 3cm"">Vorname</th>
                            <th rowspan=""2"">
                                <span class=""upright"">Geschlecht (m/w)</span>
                            </th>
                            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
                            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
                            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
                            <th colspan=""2"" class=""bot-no-border"">Heimatort/e</th>
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
                            <th style=""width: 3cm;""></th>
                            <th>Kt.</th>
                        </tr>
                        </thead>
                        <tbody>
                        @foreach (var candidate in Model.List.Candidates)
                        {
                            <tr>
                                <td class=""filled right"">@candidate.Index</td>
                                @if (candidate.IsNew)
                                {
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
                                    <td></td>
                                    <td></td>
                                }
                                else
                                {
                                    <td>@(string.IsNullOrEmpty(candidate.BallotFamilyName) ? candidate.FamilyName : candidate.BallotFamilyName)</td>
                                    <td>@(string.IsNullOrEmpty(candidate.BallotFirstName) ? candidate.FirstName : candidate.BallotFirstName)</td>
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
                                    <td>@candidate.OccupationalTitle</td>
                                    <td>@candidate.Street @candidate.HouseNumber</td>
                                    <td>@candidate.ZipCode</td>
                                    <td>@candidate.Locality</td>
                                    <td class=""right"">@candidate.Origin</td>
                                    <td>@candidate.OriginCanton</td>
                                    <!-- Bisher -->
                                    <td class=""center @(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></td>
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
                <div class=""row space-bottom"">
                    <div class=""w-12"">Vertretung des Wahlvorschlags</div>
                    <div>
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname""/>
                            <div class=""border""></div>
                        </div>
                        E-Mail:
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
                <div class=""row space-bottom"">
                    <div class=""w-12"">Stellvertretung des Wahlvorschlags</div>
                    <div>
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
                            <div class=""border""></div>
                        </div>
                        E-Mail:
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
                <div class=""space-bottom"">
                    @Model.GetInfoText(""formCandidacies"")
                </div>
                @if (!Model.List.IsNew)
                {
                    <div class=""row stretch"">
                        <div class=""grow version hint"">Version @Model.List.Version</div>
                    </div>
                }
                </main>", "@Model.Election.Name - Kandidaten.pdf", 0, null, true, null, 1 },
                { new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"), @"@using Eawv.Service.DataAccess.Entities
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
                            @if (Model.List.SubmitDate.HasValue)
                            {
                                <div class=""flex"">
                                    Einreichedatum:
                                    <div class=""input inline-input w-6"">
                                        <input value=""@Model.List.SubmitDate.Value.ToString(""d"")""/>
                                        <div class=""border""></div>
                                    </div>
                                    <div class=""input inline-input w-4"">
                                        <input value=""@Model.List.SubmitDate.Value.ToString(""t"")""/>
                                        <div class=""border""></div>
                                    </div>
                                    Uhr
                                </div>
                            }
                        </div>
                    </div>
                </div>
                <div>
                    <div class=""row space"">
                        <div class=""row"">
                            <span class=""label space-right"">B</span>
                            <div>
                                <h2 class=""inline"">Kandidaturen</h2>
                            </div>
                        </div>
                        <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
                    </div>
                    <table class=""fullwidth"">
                        <thead>
                        <tr>
                            <th rowspan=""2"">Nr.</th>
                            <th rowspan=""2"" style=""width: 3cm"">Name</th>
                            <th rowspan=""2"" style=""width: 3cm"">Vorname</th>
                            <th rowspan=""2"">
                                <span class=""upright"">Geschlecht (m/w)</span>
                            </th>
                            <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
                            <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
                            <th colspan=""3"" class=""bot-no-border"">Adresse</th>
                            <th colspan=""2"" class=""bot-no-border"">Heimatort/e</th>
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
                            <th style=""width: 3cm;""></th>
                            <th>Kt.</th>
                        </tr>
                        </thead>
                        <tbody>
                        @foreach (var candidate in Model.List.Candidates)
                        {
                            <tr>
                                <td class=""filled right"">@candidate.Index</td>
                                @if (candidate.IsNew)
                                {
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
                                    <td></td>
                                    <td></td>
                                }
                                else
                                {
                                    <td>@(string.IsNullOrEmpty(candidate.BallotFamilyName) ? candidate.FamilyName : candidate.BallotFamilyName)</td>
                                    <td>@(string.IsNullOrEmpty(candidate.BallotFirstName) ? candidate.FirstName : candidate.BallotFirstName)</td>
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
                                    <td>@candidate.OccupationalTitle</td>
                                    <td>@candidate.Street @candidate.HouseNumber</td>
                                    <td>@candidate.ZipCode</td>
                                    <td>@candidate.Locality</td>
                                    <td class=""right"">@candidate.Origin</td>
                                    <td>@candidate.OriginCanton</td>
                                    <!-- Bisher -->
                                    <td class=""center @(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></td>
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
                <div class=""row space-bottom"">
                    <div class=""w-12"">Vertretung des Wahlvorschlags</div>
                    <div>
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname""/>
                            <div class=""border""></div>
                        </div>
                        E-Mail:
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
                <div class=""row space-bottom"">
                    <div class=""w-12"">Stellvertretung des Wahlvorschlags</div>
                    <div>
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
                            <div class=""border""></div>
                        </div>
                        E-Mail:
                        <div class=""input inline-input w-24"">
                            <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
                <div class=""space-bottom"">
                    @Model.GetInfoText(""formCandidacies"")
                </div>
                @if (!Model.List.IsNew)
                {
                    <div class=""row stretch"">
                        <div class=""grow version hint"">Version @Model.List.Version</div>
                    </div>
                }
                </main>", "@Model.Election.Name - Kandidaten (leer).pdf", 0, null, true, null, 2 },
                { new Guid("6e676953-7461-726f-6965-73242d000000"), @"@using RazorLight
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
                            <h1>@ViewBag.Model.Election.Name vom @ViewBag.Model.Election.ContestDate.ToString(""dd. MMMM y"")</h1>
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
                                                    <input/>
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
                                    <span style=""width: 34rem;"">@Model.GetInfoText(""formSignatories"")</span>
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
                            </div>
                        </main>
                    </div>
                }", "@Model.Election.Name - Unterzeichnende.pdf", 0, null, true, null, 4 },
                { new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"), @"@inherits RazorLight.TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
                @{
                    ViewBag.Model = Model;                                                                     
                    Layout = ""BaseLayoutHeader"";
                }
                <main>
                    <div>
                        <div class=""row space"">
                            <div class=""row"">
                                <div>
                                    <h2 class=""inline"">Listenverbindung</h2>
                                </div>
                            </div>
                            <span>Anzahl Sitze: 30</span>
                        </div>
                        <div class=""row space"">
                            <span>@Model.GetInfoText(""formListunion"")</span>
                        </div>
                        <table class=""fullwidth"">
                            <thead>
                            <tr>
                                <th rowspan=""2"" style=""width: 1cm;"">Liste<br/>Nr.</th>
                                <th rowspan=""2"" style=""width: 4cm;"">Listenbezeichnung</th>
                                <th rowspan=""2"" style=""width: 1cm;"">Stamm-<br/>liste</th>
                                <th style=""width: 4.5cm;"" class=""bot-no-border"">Bemerkungen</th>
                                <th colspan=""2"" class=""bot-no-border"">Vertreter/Vertreterin</th>
                                <th style=""width: 3cm;"" rowspan=""2"">Ort</th>
                                <th rowspan=""2"">Datum</th>
                            </tr>
                            <tr class=""top-no-border"">
                                <th style=""width: 4.5cm;"">ggf. Unterlistenverbindung(en)</th>
                                <th>Name Vorname</th>
                                <th>Unterschrift</th>
                            </tr>
                            </thead>
                            <tbody>
                                @for (var i = 0; i < 10; i++)
                                {
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td class=""center checkbox-unchecked""></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                }
                            </tbody>
                        </table>
                    </div>
                </main>", "@Model.Election.Name - Listenverbindungen (leer).pdf", 0, null, true, null, 3 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Templates_Type_TenantId_Key",
            table: "Templates",
            columns: new[] { "Type", "TenantId", "Key" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Templates_Type_TenantId_Key",
            table: "Templates");

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"));

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("3464ef11-14e5-9cbb-fcc5-fd31fc1704fd"));

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"));

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("7473694c-6143-646e-6964-61746573242d"));

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"));

        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("a148bd7e-0f06-7117-5de1-6956113f1b06"));

        migrationBuilder.DropColumn(
            name: "Key",
            table: "Templates");

        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Landscape", "TenantId", "Type" },
            values: new object[] { new Guid("7473694c-6143-646e-6964-617465732d00"), @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
  // show signature column only for lists not in draft state or pseudo lists                                                                       
  var showSignatureColumn = !Model.ListIsDraft || Model.List.IsNew;
}
<html>
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
          padding: 1cm; // default of pdf renderer
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
        height: 1cm;
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
      
      .checkbox-checked::after {
        content: '☑';
        font-size: 22px;
      }
      
      .checkbox-unchecked::after {
        content: '☐';
        font-size: 22px;
      }
    </style>
  </head>
  <body>
    <div class=""doc size-A4 landscape"">
      <header class=""row space"">
        <h1>@Model.Election.Name vom @Model.Election.ContestDate.ToString(""dd. MMMM y"")</h1>
        <h1>@Model.ElectionTenant.Name</h1>
      </header>
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
              @if(Model.List.SubmitDate.HasValue) 
              {
                <div class=""flex"">
                  Einreichedatum:
                  <div class=""input inline-input w-6"">
                    <input value=""@Model.List.SubmitDate.Value.ToString(""d"")""/>
                    <div class=""border""></div>
                  </div>
                  <div class=""input inline-input w-4"">
                    <input value=""@Model.List.SubmitDate.Value.ToString(""t"")""/>
                    <div class=""border""></div>
                  </div>
                  Uhr
                </div>
              }
            </div>
          </div>
        </div>
        <div>
          <div class=""row space"">
            <div class=""row"">
              <span class=""label space-right"">B</span>
              <div>
                <h2 class=""inline"">Kandidaturen</h2>
              </div>
            </div>
            <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
          </div>
          <table class=""fullwidth"">
            <thead>
              <tr>
                <th rowspan=""2"">Nr.</th>
                <th rowspan=""2"" style=""width: 3cm"">Name</th>
                <th rowspan=""2"" style=""width: 3cm"">Vorname</th>
                <th rowspan=""2"">
                  <span class=""upright"">Geschlecht (m/w)</span>
                </th>
                <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
                <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
                <th colspan=""3"" class=""bot-no-border"">Adresse</th>
                <th colspan=""2"" class=""bot-no-border"">Heimatort/e</th>
                <th rowspan=""2""><span class=""upright"">Bisher</span></th>
                @if (showSignatureColumn)
                {
                  <th rowspan=""2"" style=""width: 3.5cm"">
                    Unterschrift<br />der Kandidierenden
                  </th>
                }
                <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
              </tr>
              <tr class=""top-no-border"">
                <th><span class=""upright"">Tag</span></th>
                <th><span class=""upright"">Monat</span></th>
                <th><span class=""upright"">Jahr</span></th>
                <th style=""width: 3.5cm;"">
                  inkl. allfällige Titel und weitere Bezeichnungen für
                  Stimmzettel
                </th>
                <th style=""width: 3cm;"">Strasse/Nr.</th>
                <th>PLZ</th>
                <th>Wohnort</th>
                <th style=""width: 3cm;""></th>
                <th>Kt.</th>
              </tr>
            </thead>
            <tbody>
            @foreach (var candidate in Model.List.Candidates)
            {
              <tr>
                <td class=""filled right"">@candidate.Index</td>
                @if (candidate.IsNew)
                {
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
                  <td></td>
                  <td></td>
                }
                else
                {
                  <td>@(string.IsNullOrEmpty(candidate.BallotFamilyName) ? candidate.FamilyName : candidate.BallotFamilyName)</td>
                  <td>@(string.IsNullOrEmpty(candidate.BallotFirstName) ? candidate.FirstName : candidate.BallotFirstName)</td>
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
                  <td>@candidate.OccupationalTitle</td>
                  <td>@candidate.Street @candidate.HouseNumber</td>
                  <td>@candidate.ZipCode</td>
                  <td>@candidate.Locality</td>
                  <td class=""right"">@candidate.Origin</td>
                  <td>@candidate.OriginCanton</td>
                  <!-- Bisher -->
                  <td class=""center @(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></td>
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
        <div class=""row space-bottom"">
          <div class=""w-12"">Vertretung des Wahlvorschlags</div>
          <div>
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname"" />
              <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
              <div class=""border""></div>
            </div>
          </div>
        </div>
        <div class=""row space-bottom"">
          <div class=""w-12"">Stellvertretung des Wahlvorschlags</div>
          <div>
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
              <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
              <div class=""border""></div>
            </div>
          </div>
        </div>
        <div class=""space-bottom"">
          @Model.GetInfoText(""formCandidacies"")
        </div>
        @if (!Model.List.IsNew)
        {
          <div class=""row stretch"">
            <div class=""grow version hint"">Version @Model.List.Version</div>
          </div>
        }
       </main>
    </div>
  </body>
</html>
", "@Model.Election.Name - Kandidaten.pdf", 0, true, null, 0 });

        migrationBuilder.InsertData(
            table: "Templates",
            columns: new[] { "Id", "Content", "Filename", "Format", "Landscape", "TenantId", "Type" },
            values: new object[] { new Guid("709a8ef6-9d5e-c029-ddaa-346ea4021b43"), @"@using Eawv.Service.DataAccess.Entities
@using RazorLight
@using System.Linq
@using System
@inherits TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
  // show signature column only for lists not in draft state or pseudo lists                                                                       
  var showSignatureColumn = !Model.ListIsDraft || Model.List.IsNew;
}
<html>
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
          padding: 1cm; // default of pdf renderer
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
        height: 1cm;
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
      
      .checkbox-checked::after {
        content: '☑';
        font-size: 22px;
      }
      
      .checkbox-unchecked::after {
        content: '☐';
        font-size: 22px;
      }
    </style>
  </head>
  <body>
    <div class=""doc size-A4 landscape"">
      <header class=""row space"">
        <h1>@Model.Election.Name vom @Model.Election.ContestDate.ToString(""dd. MMMM y"")</h1>
        <h1>@Model.ElectionTenant.Name</h1>
      </header>
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
              @if(Model.List.SubmitDate.HasValue) 
              {
                <div class=""flex"">
                  Einreichedatum:
                  <div class=""input inline-input w-6"">
                    <input value=""@Model.List.SubmitDate.Value.ToString(""d"")""/>
                    <div class=""border""></div>
                  </div>
                  <div class=""input inline-input w-4"">
                    <input value=""@Model.List.SubmitDate.Value.ToString(""t"")""/>
                    <div class=""border""></div>
                  </div>
                  Uhr
                </div>
              }
            </div>
          </div>
        </div>
        <div>
          <div class=""row space"">
            <div class=""row"">
              <span class=""label space-right"">B</span>
              <div>
                <h2 class=""inline"">Kandidaturen</h2>
              </div>
            </div>
            <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
          </div>
          <table class=""fullwidth"">
            <thead>
              <tr>
                <th rowspan=""2"">Nr.</th>
                <th rowspan=""2"" style=""width: 3cm"">Name</th>
                <th rowspan=""2"" style=""width: 3cm"">Vorname</th>
                <th rowspan=""2"">
                  <span class=""upright"">Geschlecht (m/w)</span>
                </th>
                <th colspan=""3"" class=""bot-no-border"">Geburtsdatum</th>
                <th class=""bot-no-border"" style=""width: 4cm;"">Beruf</th>
                <th colspan=""3"" class=""bot-no-border"">Adresse</th>
                <th colspan=""2"" class=""bot-no-border"">Heimatort/e</th>
                <th rowspan=""2""><span class=""upright"">Bisher</span></th>
                @if (showSignatureColumn)
                {
                  <th rowspan=""2"" style=""width: 3.5cm"">
                    Unterschrift<br />der Kandidierenden
                  </th>
                }
                <th rowspan=""2"" style=""width: .2cm"">Kontrolle (leer lassen)</th>
              </tr>
              <tr class=""top-no-border"">
                <th><span class=""upright"">Tag</span></th>
                <th><span class=""upright"">Monat</span></th>
                <th><span class=""upright"">Jahr</span></th>
                <th style=""width: 3.5cm;"">
                  inkl. allfällige Titel und weitere Bezeichnungen für
                  Stimmzettel
                </th>
                <th style=""width: 3cm;"">Strasse/Nr.</th>
                <th>PLZ</th>
                <th>Wohnort</th>
                <th style=""width: 3cm;""></th>
                <th>Kt.</th>
              </tr>
            </thead>
            <tbody>
            @foreach (var candidate in Model.List.Candidates)
            {
              <tr>
                <td class=""filled right"">@candidate.Index</td>
                @if (candidate.IsNew)
                {
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
                  <td></td>
                  <td></td>
                }
                else
                {
                  <td>@(string.IsNullOrEmpty(candidate.BallotFamilyName) ? candidate.FamilyName : candidate.BallotFamilyName)</td>
                  <td>@(string.IsNullOrEmpty(candidate.BallotFirstName) ? candidate.FirstName : candidate.BallotFirstName)</td>
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
                  <td>@candidate.OccupationalTitle</td>
                  <td>@candidate.Street @candidate.HouseNumber</td>
                  <td>@candidate.ZipCode</td>
                  <td>@candidate.Locality</td>
                  <td class=""right"">@candidate.Origin</td>
                  <td>@candidate.OriginCanton</td>
                  <!-- Bisher -->
                  <td class=""center @(candidate.Incumbent ? ""checkbox-checked"" : ""checkbox-unchecked"")""></td>
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
        <div class=""row space-bottom"">
          <div class=""w-12"">Vertretung des Wahlvorschlags</div>
          <div>
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListCreatedBy?.Firstname @Model.ListCreatedBy?.Lastname"" />
              <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListCreatedBy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
              <div class=""border""></div>
            </div>
          </div>
        </div>
        <div class=""row space-bottom"">
          <div class=""w-12"">Stellvertretung des Wahlvorschlags</div>
          <div>
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListDeputy?.Firstname @Model.ListDeputy?.Lastname""/>
              <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-24"">
              <input value=""@Model.ListDeputy?.Emails.FirstOrDefault(e => e.Primary)?.Email""/>
              <div class=""border""></div>
            </div>
          </div>
        </div>
        <div class=""space-bottom"">
          @Model.GetInfoText(""formCandidacies"")
        </div>
        @if (!Model.List.IsNew)
        {
          <div class=""row stretch"">
            <div class=""grow version hint"">Version @Model.List.Version</div>
          </div>
        }
       </main>
    </div>
  </body>
</html>
", "@Model.Election.Name - Kandidaten (leer).pdf", 0, true, null, 1 });
    }
}
