// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Eawv.Service.Services.Excel;

public class ExcelExport
{
    public string SheetName { get; set; }

    public IEnumerable<string> Header { get; set; }

    public IEnumerable<IEnumerable<object>> Data { get; set; }
}
