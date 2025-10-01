// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class TemplateEmptyListUnions561 : Migration
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
        <div class=""row space"">
            <div class=""row"">
                <div>
                    <h2 class=""inline"">Listenverbindung</h2>
                </div>
            </div>
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
    }
}
