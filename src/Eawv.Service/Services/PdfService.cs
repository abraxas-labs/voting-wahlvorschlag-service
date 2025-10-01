// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Eawv.Service.Models.PdfServiceModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Eawv.Service.Services;

public class PdfService : IPdfService
{
    private const string PdfOptionsHeaderName = "pdf-options";
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
    };

    private readonly HttpClient _http;

    public PdfService(HttpClient http)
    {
        _http = http;
    }

    public async Task HtmlToPdf(Stream htmlStream, Stream pdfStream, PdfRequestModel requestModel)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, new Uri("pdf", UriKind.Relative));
        var pdfOptions = JsonConvert.SerializeObject(requestModel, JsonSerializerSettings);
        request.Headers.Add(PdfOptionsHeaderName, pdfOptions);
        request.Content = new StreamContent(htmlStream);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

        using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        await response.Content.CopyToAsync(pdfStream);
    }
}
