// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class CandidatesCsv : Migration
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
Listennummer;Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatorte;Heimatkanton;Beruf;Politischer Beruf;Bisher;Vorkumuliert
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
                candidate.OriginCanton,
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
            keyValue: new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"),
            column: "Type",
            value: 5);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("4bec31bf-5e45-7b3c-710e-bd4213a24856"),
            column: "Type",
            value: 8);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            column: "Type",
            value: 6);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"),
            column: "Type",
            value: 4);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("f5021674-afce-9c2c-a533-8a401bb3f2e4"),
            column: "Type",
            value: 7);

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

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("b9bdbd38-929b-42cc-415b-d099a3d599ac"));

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
Listennummer;Kandidatennummer;Amtliche Namen;Amtliche Vornamen;Politische Namen;Politische Vornamen;Geschlecht;Geburtsdatum;Adresse;Wplz;Wohnort;Heimatorte;Beruf;Politischer Wohnsitz;Land
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
                $""{candidate.Origin} ({candidate.OriginCanton})"",
                candidate.OccupationalTitle,
                """",
                """"
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
            keyValue: new Guid("17213c83-88e6-69a2-7f34-a3b716acfda5"),
            column: "Type",
            value: 4);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("4bec31bf-5e45-7b3c-710e-bd4213a24856"),
            column: "Type",
            value: 7);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("6e676953-7461-726f-6965-73242d000000"),
            column: "Type",
            value: 5);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("906ac8af-b905-0d33-1f03-a462c66f0387"),
            column: "Type",
            value: 3);

        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("f5021674-afce-9c2c-a533-8a401bb3f2e4"),
            column: "Type",
            value: 6);
    }
}
