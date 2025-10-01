// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Xml.Serialization;

namespace Eawv.Service.Services.Excel;

[XmlRoot(ElementName = "modified", Namespace = ExcelDocumentCorePropertiesConstants.NamespaceDcTerms)]
public class ExcelDocumentModified
{
    [XmlAttribute(AttributeName = "type", Namespace = ExcelDocumentCorePropertiesConstants.NamespaceXsi)]
    public string Type { get; set; } = ExcelDocumentCorePropertiesConstants.DateTimeType;

    [XmlText]
    public string Text { get; set; }
}
