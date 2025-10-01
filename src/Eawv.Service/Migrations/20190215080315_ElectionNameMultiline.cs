// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ElectionNameMultiline : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
            keyValue: new Guid("a148bd7e-0f06-7117-5de1-6956113f1b06"),
            column: "Content",
            value: @"@{
    Layout = ""BaseLayout"";
}
<header class=""row space"">
    <h1 class=""preserve-whitespace"">@ViewBag.Model.Election.Name</h1>
    @{
        var tenantTitle = ViewBag.Model.GetInfoText(""tenantTitle"");
        var hasTenantLogo = string.IsNullOrEmpty(tenantTitle) && ViewBag.Model.Election.TenantLogo != null;
    }
    
    @if (!hasTenantLogo)
    {
        <h1>@tenantTitle</h1>
    }
    
    @section HeaderStyles {
        @if (hasTenantLogo)
        {
           <text>
           .doc {
               background-image: url('@ViewBag.Model.ElectionTenantLogoDataURI');
               background-repeat: no-repeat;
               background-position: top right;
               background-size: 75px;
            }
           </text>
        }
    }
    
    @RenderSection(""Header"", false)
</header>
@RenderBody()");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
            keyValue: new Guid("a148bd7e-0f06-7117-5de1-6956113f1b06"),
            column: "Content",
            value: @"@{
    Layout = ""BaseLayout"";
}
<header class=""row space"">
    <h1>@ViewBag.Model.Election.Name vom @ViewBag.Model.Election.ContestDate.ToString(""dd. MMMM yyyy"")</h1>
    @{
        var tenantTitle = ViewBag.Model.GetInfoText(""tenantTitle"");
        var hasTenantLogo = string.IsNullOrEmpty(tenantTitle) && ViewBag.Model.Election.TenantLogo != null;
    }
    
    @if (!hasTenantLogo)
    {
        <h1>@tenantTitle</h1>
    }
    
    @section HeaderStyles {
        @if (hasTenantLogo)
        {
           <text>
           .doc {
               background-image: url('@ViewBag.Model.ElectionTenantLogoDataURI');
               background-repeat: no-repeat;
               background-position: top right;
            }
           </text>
        }
    }
    
    @RenderSection(""Header"", false)
</header>
@RenderBody()");
    }
}
