// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.Models.TemplateServiceModels;

namespace Eawv.Service.Services.Excel;

public interface IExcelExport
{
    string BuildFileName(TemplateBag bag);

    ExcelExport BuildExport(TemplateBag bag);
}
