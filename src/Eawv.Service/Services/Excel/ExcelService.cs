// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Eawv.Service.Models.TemplateServiceModels;
using Voting.Lib.Common;

namespace Eawv.Service.Services.Excel;

public class ExcelService
{
    private const string XlsxMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private const string ExcelAppName = "Abraxas EAWV";
    private const int DefaultSheetId = 1;
    private const int NumberFormatDate = 14;
    private const int NumberCellStyleDate = 1; // needs to be the index of the corresponding cellformat

    private readonly IClock _clock;
    private readonly Dictionary<TemplateType, IExcelExport> _exports;

    public ExcelService(WabstiCandidatesExcelService wabstiCandidatesExcelService, IClock clock)
    {
        _clock = clock;
        _exports = new Dictionary<TemplateType, IExcelExport>
        {
            { TemplateType.WabstiCandidates, wabstiCandidatesExcelService },
        };
    }

    public Task<RenderResult> CreateExcel(
        TemplateType type,
        TemplateBag bag,
        string creator)
    {
        if (!_exports.TryGetValue(type, out var exporter))
        {
            throw new ArgumentException($"Cannot render {type} in xlsx", nameof(type));
        }

        var renderResult = new RenderResult(exporter.BuildFileName(bag), XlsxMimeType, async stream =>
        {
            var exportData = exporter.BuildExport(bag);

            // The excel library expects a read- and writeable stream, which our output stream does not support.
            // We use a memory stream to create the excel first, then copy that to the output
            await using var memoryStream = new MemoryStream();
            BuildExcel(memoryStream, exportData.SheetName, creator, exportData.Header, exportData.Data);
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(stream);
        });
        return Task.FromResult(renderResult);
    }

    private static void SerializeCoreDocProperties(
        ExcelDocumentCoreProperties props,
        CoreFilePropertiesPart part)
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add(
            ExcelDocumentCorePropertiesConstants.NamespaceCorePropsShort,
            ExcelDocumentCorePropertiesConstants.NamespaceCoreProps);
        ns.Add(
            ExcelDocumentCorePropertiesConstants.NamespaceDcElementsShort,
            ExcelDocumentCorePropertiesConstants.NamespaceDcElements);
        ns.Add(
            ExcelDocumentCorePropertiesConstants.NamespaceDcTermsShort,
            ExcelDocumentCorePropertiesConstants.NamespaceDcTerms);
        ns.Add(
            ExcelDocumentCorePropertiesConstants.NamespaceXsiShort,
            ExcelDocumentCorePropertiesConstants.NamespaceXsi);

        using var sw = new StreamWriter(part.GetStream());
        var xs = new XmlSerializer(props.GetType());
        xs.Serialize(sw, props, ns);
    }

    private static Row BuildRow(IEnumerable<object> data)
    {
        var row = new Row();

        foreach (var cellData in data)
        {
            var cell = new Cell();
            switch (cellData)
            {
                case DateTime dt:
                    // no idea why this oadate thing is needed, since cellvalue offers a constructor accepting
                    // datetime. But if this constructor is used, excel shows a repair dialog before opening the file
                    cell.CellValue = new CellValue(dt.ToOADate().ToString(CultureInfo.InvariantCulture));
                    cell.DataType = CellValues.Number;
                    cell.StyleIndex = NumberCellStyleDate;
                    break;
                case int _:
                case double _:
                case decimal _:
                case long _:
                case short _:
                    cell.CellValue = new CellValue(cellData.ToString());
                    cell.DataType = CellValues.Number;
                    break;
                default:
                    cell.CellValue = new CellValue(cellData?.ToString() ?? string.Empty);
                    cell.DataType = CellValues.String;
                    break;
            }

            row.AppendChild(cell);
        }

        return row;
    }

    private static Row BuildHeader(IEnumerable<string> header)
    {
        var headerRow = new Row();
        foreach (var headerName in header)
        {
            headerRow.AppendChild(new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(headerName),
            });
        }

        return headerRow;
    }

    private void BuildExcel(
        Stream target,
        string name,
        string userName,
        IEnumerable<string> header,
        IEnumerable<IEnumerable<object>> data)
    {
        using var doc = SpreadsheetDocument.Create(target, SpreadsheetDocumentType.Workbook, true);

        AddDocProperties(doc, userName);

        var workbookPart = doc.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
        stylesPart.Stylesheet = new Stylesheet
        {
            Fonts = new Fonts(new Font()),
            Fills = new Fills(new Fill()),
            Borders = new Borders(new Border()),
            CellStyleFormats = new CellStyleFormats(new CellFormat()),
            CellFormats = new CellFormats(
                new CellFormat(),
                new CellFormat
                {
                    NumberFormatId = NumberFormatDate,
                    ApplyNumberFormat = true,
                }),
        };

        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        SheetData sheetData = new();
        worksheetPart.Worksheet = new Worksheet(sheetData);

        var sheets = workbookPart.Workbook.AppendChild(new Sheets());
        var sheet = new Sheet
        {
            Id = workbookPart.GetIdOfPart(worksheetPart),
            Name = name,
            SheetId = DefaultSheetId,
        };
        sheets.AppendChild(sheet);

        if (header != null)
        {
            sheetData.AppendChild(BuildHeader(header));
        }

        foreach (var row in data)
        {
            sheetData.AppendChild(BuildRow(row));
        }
    }

    private void AddDocProperties(SpreadsheetDocument doc, string creatorName)
    {
        // no nice api yet for the core props :(
        // https://github.com/OfficeDev/Open-XML-SDK/issues/389 :(
        var coreProps = doc.AddCoreFilePropertiesPart();
        var corePropsValues = new ExcelDocumentCoreProperties
        {
            Created = new ExcelDocumentCreated
            {
                Text = _clock.UtcNow.ToString(ExcelDocumentCorePropertiesConstants.DateTimeFormat, CultureInfo.InvariantCulture),
            },
            Modified = new ExcelDocumentModified
            {
                Text = _clock.UtcNow.ToString(ExcelDocumentCorePropertiesConstants.DateTimeFormat, CultureInfo.InvariantCulture),
            },
            Creator = creatorName,
            LastModifiedBy = creatorName,
        };
        SerializeCoreDocProperties(corePropsValues, coreProps);

        var extendedProps = doc.AddExtendedFilePropertiesPart();
        extendedProps.Properties = new Properties
        {
            Application = new Application
            {
                Text = ExcelAppName,
            },
            ApplicationVersion = new ApplicationVersion
            {
                // Excel can't handle more than 3 positions
                Text = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
            },
        };
    }
}
