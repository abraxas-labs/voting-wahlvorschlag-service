// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Eawv.Service.Configuration;
using Eawv.Service.DataAccess;
using Eawv.Service.Models.PdfServiceModels;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services.Excel;

namespace Eawv.Service.Services;

/// <inheritdoc cref="ITemplateService"/>
public class TemplateService : ITemplateService
{
    private const string ElectionNamePlaceholder = "{ELECTION_NAME}";

    private readonly IPdfService _pdf;
    private readonly TemplateRepository _templateRepository;
    private readonly ITenantService _tenantService;
    private readonly IUserService _userService;
    private readonly AppConfig _config;
    private readonly EchService _eCHService;
    private readonly ExcelService _excelService;
    private readonly RazorRenderer _razorRenderer;

    [SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "TemplateService requires these services injected in the constructor.")]
    public TemplateService(
        IPdfService pdf,
        TemplateRepository templateRepository,
        ITenantService tenantService,
        IUserService userService,
        AppConfig config,
        EchService eCHService,
        ExcelService excelService,
        RazorRenderer razorRenderer)
    {
        _pdf = pdf;
        _templateRepository = templateRepository;
        _tenantService = tenantService;
        _userService = userService;
        _config = config;
        _eCHService = eCHService;
        _excelService = excelService;
        _razorRenderer = razorRenderer;
    }

    public async Task<RenderResult> RenderToHtml(TemplateType type, TemplateBag bag, bool leaveStreamOpen = false)
    {
        await FillTemplateBagInfos(bag, type);
        var filename = BuildFilename(bag);
        return new RenderResult(
            filename,
            MediaTypeNames.Text.Html,
            stream => _razorRenderer.Render(bag, stream, leaveStreamOpen));
    }

    public async Task<RenderResult> RenderToPdf(TemplateType type, TemplateBag bag)
    {
        await FillTemplateBagInfos(bag, type);
        var filename = BuildFilename(bag);
        return new RenderResult(filename, MediaTypeNames.Application.Pdf, async stream =>
        {
            var pipe = new Pipe();
            var htmlToPdfTask = _pdf.HtmlToPdf(pipe.Reader.AsStream(), stream, new PdfRequestModel
            {
                Landscape = bag.Template.Landscape,
                Format = bag.Template.Format,
            });
            await _razorRenderer.Render(bag, pipe.Writer.AsStream());
            await htmlToPdfTask;
        });
    }

    public async Task<RenderResult> RenderToXlsx(TemplateType type, TemplateBag bag)
    {
        var userName = await _userService.GetCurrentUserName();
        await FillTemplateBagInfos(bag);
        return await _excelService.CreateExcel(type, bag, userName);
    }

    public async Task<RenderResult> RenderToCsv(TemplateType type, TemplateBag bag)
    {
        await FillTemplateBagInfos(bag, type);
        var filename = BuildFilename(bag);
        return new RenderResult(filename, MediaTypeNames.Text.Csv, async stream =>
        {
            // explicitly add UTF8-BOM to content, otherwise Excel and other programs may display umlauts incorrectly
            await stream.WriteAsync(Encoding.UTF8.GetPreamble());
            await _razorRenderer.Render(bag, stream);
        });
    }

    public async Task<RenderResult> RenderToXml(TemplateType type, TemplateBag bag)
    {
        await FillTemplateBagInfos(bag, type);
        var filename = BuildFilename(bag);
        return new RenderResult(filename, MediaTypeNames.Text.Xml, stream =>
        {
            _eCHService.WriteXml(type, bag, stream);
            return Task.CompletedTask;
        });
    }

    private async Task FillTemplateBagInfos(TemplateBag bag, TemplateType? type = null)
    {
        if (type.HasValue)
        {
            bag.Template = await _templateRepository.GetBestMatching(type.Value);
        }

        bag.Configuration = _config;
        bag.GetTenantAsync = _tenantService.Get;
        bag.GetUserAsync = _userService.Get;
    }

    private string BuildFilename(TemplateBag bag)
    {
        return bag.Election != null
            ? bag.Template.Filename.Replace(ElectionNamePlaceholder, bag.Election.Name, StringComparison.InvariantCulture)
            : bag.Template.Filename;
    }
}
