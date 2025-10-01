// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Eawv.Service.Models.TemplateServiceModels;

namespace Eawv.Service.Services;

/// <summary>
/// Service for rendering various <see cref="TemplateType"/>s with their corresponding Razor views, using the passed data within a <see cref="TemplateBag"/>.
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// Renders the Razor-View defined by the <see cref="TemplateType"/>  using the <see cref="TemplateBag"/> as model.
    /// </summary>
    /// <param name="type">The template type.</param>
    /// <param name="bag">The view model data.</param>
    /// <param name="leaveStreamOpen">True to leave the render result stream open. Defaults to false.</param>
    /// <returns>A <see cref="RenderResult"/> with html content.</returns>
    Task<RenderResult> RenderToHtml(TemplateType type, TemplateBag bag, bool leaveStreamOpen = false);

    /// <summary>
    /// Renders the Razor-View defined by the <see cref="TemplateType"/>  using the <see cref="TemplateBag"/> as model,
    /// respecting pdf format specialties.
    /// </summary>
    /// <param name="type">The template type.</param>
    /// <param name="bag">The view model data.</param>
    /// <returns>A <see cref="RenderResult"/> with pdf as content.</returns>
    Task<RenderResult> RenderToPdf(TemplateType type, TemplateBag bag);

    /// <summary>
    /// Renders the Razor-View defined by the <see cref="TemplateType"/>  using the <see cref="TemplateBag"/> as model,
    /// respecting xlsx format specialties.
    /// </summary>
    /// <param name="type">The template type.</param>
    /// <param name="bag">The view model data.</param>
    /// <returns>A <see cref="RenderResult"/> with xlsx as content.</returns>
    Task<RenderResult> RenderToXlsx(TemplateType type, TemplateBag bag);

    /// <summary>
    /// Renders the Razor-View defined by the <see cref="TemplateType"/>  using the <see cref="TemplateBag"/> as model,
    /// respecting csv format specialties.
    /// </summary>
    /// <param name="type">The template type.</param>
    /// <param name="bag">The view model data.</param>
    /// <returns>A <see cref="RenderResult"/> with csv as content.</returns>
    Task<RenderResult> RenderToCsv(TemplateType type, TemplateBag bag);

    /// <summary>
    /// Renders the Razor-View defined by the <see cref="TemplateType"/>  using the <see cref="TemplateBag"/> as model,
    /// respecting xml format specialties.
    /// </summary>
    /// <param name="type">The template type.</param>
    /// <param name="bag">The view model data.</param>
    /// <returns>A <see cref="RenderResult"/> with xml content.</returns>
    Task<RenderResult> RenderToXml(TemplateType type, TemplateBag bag);
}
