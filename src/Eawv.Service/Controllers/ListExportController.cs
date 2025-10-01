// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Controllers;

[Route("api/elections/{electionId:Guid}/lists/{listId:Guid}/export")]
[ApiController]
[Authorize(Roles = Role.All)]
public class ListExportController : ExportController
{
    private static readonly ListExportTemplateType[] OnlyWahlverwalter =
    [
        ListExportTemplateType.WabstiCandidates,
    ];

    private readonly ElectionRepository _electionRepository;
    private readonly ListRepository _listRepository;
    private readonly SettingRepository _settingRepository;
    private readonly AuthService _authService;

    public ListExportController(
        ITemplateService templateService,
        ElectionRepository electionRepository,
        ListRepository listRepository,
        SettingRepository settingRepository,
        AuthService authService)
        : base(templateService)
    {
        _electionRepository = electionRepository;
        _listRepository = listRepository;
        _settingRepository = settingRepository;
        _authService = authService;
    }

    [HttpGet("{type}")]
    public async Task<FileResult> Export(
        Guid electionId,
        Guid listId,
        [ValidEnum] ListExportTemplateType type,
        [ValidEnum] FileTypeModel? format = null)
    {
        if (!_authService.IsWahlverwalter && OnlyWahlverwalter.Contains(type))
        {
            throw new ForbiddenException($"only Wahlverwalters can export {type}");
        }

        if (!format.HasValue)
        {
            format = type switch
            {
                ListExportTemplateType.WabstiCandidates => FileTypeModel.Xlsx,
                _ => FileTypeModel.Pdf,
            };
        }

        var bag = new TemplateBag
        {
            Election = await _electionRepository.Get(electionId),
            List = await _listRepository.Get(electionId, listId),
            Settings = await _settingRepository.GetSetting(),
        };

        return await Export(bag, (TemplateType)type, format.Value);
    }
}
