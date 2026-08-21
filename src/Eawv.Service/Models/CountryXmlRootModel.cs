// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Xml.Serialization;

namespace Eawv.Service.Models;

[Serializable]
[XmlRoot("CountryCollection")]
public class CountryXmlRootModel
{
    [XmlArray("countries")]
    [XmlArrayItem("country")]
    public CountryXmlModel[] Country { get; set; }
}
