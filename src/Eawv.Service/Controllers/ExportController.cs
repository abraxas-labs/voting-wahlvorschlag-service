// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Eawv.Service.Models;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Voting.Lib.Rest.Files;

namespace Eawv.Service.Controllers;

public abstract class ExportController : ControllerBase
{
    private readonly ITemplateService _templateService;

    protected ExportController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    protected async Task<FileResult> Export(TemplateBag bag, TemplateType type, FileTypeModel format)
    {
        var renderResult = format switch
        {
#if DEBUG
            // for debugging purposes (ex. local dev without pdf render service)
            FileTypeModel.Html => await _templateService.RenderToHtml(type, bag),
#endif
            FileTypeModel.Xlsx => await _templateService.RenderToXlsx(type, bag),
            FileTypeModel.Csv => await _templateService.RenderToCsv(type, bag),
            FileTypeModel.Xml => await _templateService.RenderToXml(type, bag),
            _ => await _templateService.RenderToPdf(type, bag),
        };

        return SingleFileResult.Create(renderResult.MimeType, renderResult.Filename, pipeWriter => renderResult.WriterFunction(pipeWriter.AsStream()));
    }
}
