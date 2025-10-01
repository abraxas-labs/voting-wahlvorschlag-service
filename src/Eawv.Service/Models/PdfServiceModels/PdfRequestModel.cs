// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Eawv.Service.Models.PdfServiceModels;

public class PdfRequestModel
{
    public bool Landscape { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public PdfFormat Format { get; set; }
}
