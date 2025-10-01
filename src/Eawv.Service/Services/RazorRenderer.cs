// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Threading.Tasks;
using Eawv.Service.Configuration;
using Eawv.Service.Models.TemplateServiceModels;
using Microsoft.Extensions.Hosting;
using RazorLight;

namespace Eawv.Service.Services;

public class RazorRenderer
{
    private readonly RazorLightEngine _razor;

    public RazorRenderer(AppConfig config, IHostEnvironment env)
    {
        _razor = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(env?.ContentRootPath, config.RazorLight.TemplatesRootPath))
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task Render(TemplateBag bag, Stream outputStream, bool leaveStreamOpen = false)
    {
        var template = await _razor.CompileTemplateAsync(bag.Template.TemplateName);
        await using var streamWriter = new StreamWriter(outputStream, leaveOpen: leaveStreamOpen);
        await _razor.RenderTemplateAsync(template, bag, streamWriter);
    }
}
