// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Threading.Tasks;
using Eawv.Service.Models.PdfServiceModels;

namespace Eawv.Service.Services;

public interface IPdfService
{
    Task HtmlToPdf(Stream htmlStream, Stream pdfStream, PdfRequestModel requestModel);
}
