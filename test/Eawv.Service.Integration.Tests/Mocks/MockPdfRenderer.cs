// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Threading.Tasks;
using Eawv.Service.Models.PdfServiceModels;
using Eawv.Service.Services;

namespace Eawv.Service.Integration.Tests.Mocks;

public class MockPdfRenderer : IPdfService
{
    public async Task HtmlToPdf(Stream htmlStream, Stream pdfStream, PdfRequestModel requestModel)
    {
        await htmlStream.CopyToAsync(pdfStream);
    }
}
