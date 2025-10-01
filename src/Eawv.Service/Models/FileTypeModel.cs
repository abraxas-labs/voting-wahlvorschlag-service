// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Models;

public enum FileTypeModel
{
    /// <summary>
    /// PDF.
    /// </summary>
    Pdf,

    /// <summary>
    /// CSV.
    /// </summary>
    Csv,

    /// <summary>
    /// XML.
    /// </summary>
    Xml,

    /// <summary>
    /// XLSX.
    /// </summary>
    Xlsx,

#if DEBUG
    /// <summary>
    /// HTML.
    /// </summary>
    Html,
#endif
}
