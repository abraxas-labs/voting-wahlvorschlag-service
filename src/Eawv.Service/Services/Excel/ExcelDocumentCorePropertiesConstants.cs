// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Services.Excel;

public static class ExcelDocumentCorePropertiesConstants
{
    public const string NamespaceCoreProps =
        "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";

    public const string NamespaceCorePropsShort = "cp";
    public const string NamespaceDcElements = "http://purl.org/dc/elements/1.1/";
    public const string NamespaceDcElementsShort = "dc";
    public const string NamespaceDcTerms = "http://purl.org/dc/terms/";
    public const string NamespaceDcTermsShort = "dcterms";
    public const string NamespaceXsi = "http://www.w3.org/2001/XMLSchema-instance";
    public const string NamespaceXsiShort = "xsi";

    public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssK";
    public const string DateTimeType = NamespaceDcTermsShort + ":W3CDTF";
}
