// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Controllers;

[Route("api/elections/{electionId:Guid}/export")]
[ApiController]
[Authorize(Roles = Role.All)]
public class ElectionExportController : ExportController
{
    private static readonly ElectionExportTemplateType[] OnlyWahlverwalter =
    [
        ElectionExportTemplateType.FederalChancellery,
        ElectionExportTemplateType.EmptyCandidates,
        ElectionExportTemplateType.EmptySignatories,
    ];

    private readonly ElectionRepository _electionRepository;
    private readonly SettingRepository _settingRepository;
    private readonly AuthService _authService;

    public ElectionExportController(
        ITemplateService templateService,
        ElectionRepository electionRepository,
        SettingRepository settingRepository,
        AuthService authService)
        : base(templateService)
    {
        _electionRepository = electionRepository;
        _settingRepository = settingRepository;
        _authService = authService;
    }

    [HttpGet("{type}")]
    public async Task<FileResult> Export(
        Guid electionId,
        [ValidEnum] ElectionExportTemplateType type,
        [ValidEnum] FileTypeModel format = FileTypeModel.Pdf,
        [FromQuery] Guid[] listIds = null)
    {
        var bag = new TemplateBag
        {
            Settings = await _settingRepository.GetSetting(),
        };

        if (!_authService.IsWahlverwalter && OnlyWahlverwalter.Contains(type))
        {
            throw new ForbiddenException($"only Wahlverwalters can export {type}");
        }

        switch (type)
        {
            case ElectionExportTemplateType.EmptyCandidates:
                bag.Election = await _electionRepository.Get(electionId);
                bag.List = new List
                {
                    Candidates = Enumerable.Range(
                        1,
                        bag.Election.DomainsOfInfluence.Sum(doi => doi.NumberOfMandates))
                    .Select(x => new Candidate { Index = x }).ToList(),
                };
                break;
            case ElectionExportTemplateType.FederalChancellery:
            case ElectionExportTemplateType.ECH157:
            case ElectionExportTemplateType.Candidates:
                if (listIds == null || listIds.Length == 0)
                {
                    throw new BadRequestException("Can only export candidates when at least one list id is selected.");
                }

                bag.Election = await _electionRepository.GetEntireElection(electionId);
                bag.Election.Lists = bag.Election.Lists
                    .Where(l => listIds.Contains(l.Id))
                    .ToList();
                break;
            case ElectionExportTemplateType.EmptySignatories:
                bag.Election = await _electionRepository.Get(electionId);
                bag.List = new List();
                break;
            default:
                bag.Election = await _electionRepository.Get(electionId);
                break;
        }

        return await Export(bag, (TemplateType)type, format);
    }
}
