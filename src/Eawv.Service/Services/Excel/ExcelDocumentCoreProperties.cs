// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Xml.Serialization;

namespace Eawv.Service.Services.Excel;

[XmlRoot(ElementName = "coreProperties", Namespace = ExcelDocumentCorePropertiesConstants.NamespaceCoreProps)]
public class ExcelDocumentCoreProperties
{
    [XmlElement(ElementName = "creator", Namespace = ExcelDocumentCorePropertiesConstants.NamespaceDcElements)]
    public string Creator { get; set; }

    [XmlElement(ElementName = "lastModifiedBy", Namespace = ExcelDocumentCorePropertiesConstants.NamespaceCoreProps)]
    public string LastModifiedBy { get; set; }

    [XmlElement(ElementName = "created", Namespace = ExcelDocumentCorePropertiesConstants.NamespaceDcTerms)]
    public ExcelDocumentCreated Created { get; set; }

    [XmlElement(ElementName = "modified", Namespace = ExcelDocumentCorePropertiesConstants.NamespaceDcTerms)]
    public ExcelDocumentModified Modified { get; set; }
}
