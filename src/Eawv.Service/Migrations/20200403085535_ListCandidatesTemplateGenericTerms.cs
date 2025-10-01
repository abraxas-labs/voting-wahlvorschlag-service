// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ListCandidatesTemplateGenericTerms : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
    <div class=""grow"">
        @if (Model.IsProporz)
        {
            <div class=""row space"">
                <div class=""row col-main"">
                    <div>
                        <span><span class=""bold"">Bezeichnung</span> des Wahlvorschlags:</span>
                    </div>
                    <div class=""grow col"">
                        <div class=""input"">
                            <input value=""@Model.List.Name"" />
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
            </div>
        }

        @if (Model.IsMajorz)
        {
            <div class=""row space"">
                <div class=""row col-main space"">
                    <div class=""flex"">
                        <span class=""bold"">Eingang bei zuständiger Behörde</span> (bitte nicht ausfüllen):
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
                        Visum zuständige Behörde
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
                    <span class=""bold"">Listennummer</span> (Bitte nicht ausfüllen, wird zugeteilt):
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
            <div>
                <h2 class=""inline"">Kandidatur(en)</h2>
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
                <th rowspan=""2"" style=""width: 3cm"">
                    <span>Partei</span>
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
                <input value=""@Model.ListRepresentative?.Firstname @Model.ListRepresentative?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListRepresentative?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
                <input value=""@Model.ListDeputy?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
    <div class=""grow"">
        @if (Model.IsProporz)
        {
            <div class=""row space"">
                <div class=""row col-main"">
                    <div>
                        <span><span class=""bold"">Bezeichnung</span> des Wahlvorschlags:</span>
                    </div>
                    <div class=""grow col"">
                        <div class=""input"">
                            <input value=""@Model.List.Name"" />
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
            </div>
        }

        @if (Model.IsMajorz)
        {
            <div class=""row space"">
                <div class=""row col-main space"">
                    <div class=""flex"">
                        <span class=""bold"">Eingang bei zuständiger Behörde</span> (bitte nicht ausfüllen):
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
                        Visum zuständige Behörde
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
                    <span class=""bold"">Listennummer</span> (Bitte nicht ausfüllen, wird zugeteilt):
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
            <div>
                <h2 class=""inline"">Kandidatur(en)</h2>
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
                <th rowspan=""2"" style=""width: 3cm"">
                    <span>Partei</span>
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
                <input value=""@Model.ListRepresentative?.Firstname @Model.ListRepresentative?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListRepresentative?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
                <input value=""@Model.ListDeputy?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
    <div class=""grow"">
        @if (Model.IsProporz)
        {
            <div class=""row space"">
                <div class=""row col-main"">
                    <div>
                        <span><span class=""bold"">Bezeichnung</span> des Wahlvorschlags:</span>
                    </div>
                    <div class=""grow col"">
                        <div class=""input"">
                            <input value=""@Model.List.Name"" />
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
            </div>
        }

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
            <div>
                <h2 class=""inline"">Kandidatur(en)</h2>
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
                <th rowspan=""2"" style=""width: 3cm"">
                    <span>Partei</span>
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
                <input value=""@Model.ListRepresentative?.Firstname @Model.ListRepresentative?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListRepresentative?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
                <input value=""@Model.ListDeputy?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
    <div class=""grow"">
        @if (Model.IsProporz)
        {
            <div class=""row space"">
                <div class=""row col-main"">
                    <div>
                        <span><span class=""bold"">Bezeichnung</span> des Wahlvorschlags:</span>
                    </div>
                    <div class=""grow col"">
                        <div class=""input"">
                            <input value=""@Model.List.Name"" />
                            <div class=""border""></div>
                        </div>
                    </div>
                </div>
            </div>
        }

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
            <div>
                <h2 class=""inline"">Kandidatur(en)</h2>
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
                <th rowspan=""2"" style=""width: 3cm"">
                    <span>Partei</span>
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
                <input value=""@Model.ListRepresentative?.Firstname @Model.ListRepresentative?.Lastname""/>
                <div class=""border""></div>
            </div>
            E-Mail:
            <div class=""input inline-input w-16"">
                <input value=""@Model.ListRepresentative?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
                <input value=""@Model.ListDeputy?.Emails?.FirstOrDefault(e => e.Primary)?.Email""/>
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
