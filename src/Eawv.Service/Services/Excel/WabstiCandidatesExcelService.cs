// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Models.TemplateServiceModels;

namespace Eawv.Service.Services.Excel;

public class WabstiCandidatesExcelService : IExcelExport
{
    private static readonly string[] Header =
    [
        "ListePlatz",
        "KandidatNr",
        "Anrede",
        "Titel",
        "Nachname",
        "Vorname",
        "AmtlicherNachname",
        "AmtlicherVorname",
        "Strasse",
        "Plz",
        "Wohnort",
        "Heimatort",
        "Beruf",
        "GebDatum",
        "Bisher",
        "Landcode",
        "TelefonP",
        "TelefonG",
        "Telefax",
        "eMail",
    ];

    public string BuildFileName(TemplateBag bag)
        => bag.Settings.WabstiExportTenantTitle + "_" + bag.List.Name;

    public ExcelExport BuildExport(TemplateBag bag)
        => new()
        {
            Data = BuildData(bag),
            Header = Header,
            SheetName = "Wahlvorschlag",
        };

    private static IEnumerable<IEnumerable<object>> BuildData(TemplateBag bag)
    {
        return bag.GetClonedAndOrderedCandidates().Select((candidate, i) => new object[]
        {
            i + 1,
            candidate.Index.ToString("D2", CultureInfo.InvariantCulture),
            candidate.Sex == SexType.Male ? "Herr" : "Frau",
            candidate.Title,
            candidate.BallotFamilyName,
            candidate.BallotFirstName,
            candidate.FamilyName,
            candidate.FirstName,
            candidate.Street + ' ' + candidate.HouseNumber,
            candidate.ZipCode,
            candidate.Locality,
            candidate.Origin,
            candidate.OccupationalTitle,
            candidate.DateOfBirth,
            candidate.Incumbent ? "Ja" : "Nein",
            "CH",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
        });
    }
}
