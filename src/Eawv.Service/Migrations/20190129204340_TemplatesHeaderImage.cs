// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class TemplatesHeaderImage : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"),
            column: "Content",
            value: @"@inherits RazorLight.TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayoutHeader"";
}

<main>
    <div>
        <div class=""row"">
            <div>
                <h2 class=""inline"">Listenverbindung</h2>
            </div>
        </div>
        <div class=""row"">
            <span class=""preserve-whitespace"">@Model.GetInfoText(""formListunion"")</span>
        </div>
        <div class=""row space"">
            <span>Die unterzeichnenden Vertreterinnen/Vertreter erklären hiermit die folgenden Listen unwiderruflich für miteinander verbunden:</span>
            <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
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
</main>");

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

      .preserve-whitespace {
        white-space: pre-wrap;
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
    var hasTenantLogo = string.IsNullOrEmpty(tenantTitle);
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
            keyValue: new Guid("a148bd7e-0f06-7117-5de1-6956113f1b06"),
            column: "Content",
            value: @"@{
    Layout = ""BaseLayout"";
}
<header class=""row space"">
    <h1>@ViewBag.Model.Election.Name vom @ViewBag.Model.Election.ContestDate.ToString(""dd. MMMM yyyy"")</h1>
    @{
        var tenantTitle = ViewBag.Model.GetInfoText(""tenantTitle"");
        var hasTenantLogo = string.IsNullOrEmpty(tenantTitle);
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

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"),
            column: "Content",
            value: @"@inherits RazorLight.TemplatePage<Eawv.Service.Models.TemplateServiceModels.TemplateBag>
@{
    ViewBag.Model = Model;
    Layout = ""BaseLayoutHeader"";
}

<main>
    <div>
        <div class=""row"">
            <div>
                <h2 class=""inline"">Listenverbindung</h2>
            </div>
        </div>
        <div class=""row space"">
            <span>Die unterzeichnenden Vertreterinnen/Vertreter erklären hiermit die folgenden Listen unwiderruflich für miteinander verbunden:</span>
            <span>Anzahl Sitze: @Model.ElectionNumberOfMandates</span>
        </div>
        <div class=""row space"">
            <span class=""preserve-whitespace"">@Model.GetInfoText(""formListunion"")</span>
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
</main>");

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

      .preserve-whitespace {
        white-space: pre-wrap;
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

      @RenderSection(""BaseStyles"", false)
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
        var hasTenantLogo = string.IsNullOrEmpty(tenantTitle);
    }
    @if (hasTenantLogo)
    {
        <img class=""tenant-logo"" src=""data:image/png;base64,@Convert.ToBase64String(ViewBag.Model.Election.TenantLogo)"" />
    }
    else
    {
        <h1>@tenantTitle</h1>
    }
    
    @section BaseStyles {
        @if (hasTenantLogo)
        {
            @:.tenant-logo {
            @:    margin-bottom: 3mm;
            @:}
        }
    }
    
    @RenderSection(""Header"", false)
</header>
@RenderBody()");
    }
}
