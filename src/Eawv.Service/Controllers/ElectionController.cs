// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voting.Lib.Common;

namespace Eawv.Service.Controllers;

[Route("api/elections")]
[ApiController]
[Authorize(Roles = Role.All)]
public class ElectionController : ControllerBase
{
    private static readonly HashSet<string> AllowedLogoMimeTypes =
    [
        "image/jpeg",
        "image/png",
    ];

    private static readonly HashSet<string> AllowedDocumentsMimeTypes =
    [
        "application/pdf",
    ];

    private readonly ElectionRepository _electionRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    private readonly FileValidationService _fileValidationService;
    private readonly DomainOfInfluenceRepository _domainOfInfluenceRepository;
    private readonly IClock _clock;

    public ElectionController(
        ElectionRepository electionRepository,
        AuthService authService,
        IMapper mapper,
        FileValidationService fileValidationService,
        DomainOfInfluenceRepository domainOfInfluenceRepository,
        IClock clock)
    {
        _electionRepository = electionRepository;
        _authService = authService;
        _mapper = mapper;
        _fileValidationService = fileValidationService;
        _domainOfInfluenceRepository = domainOfInfluenceRepository;
        _clock = clock;
    }

    [HttpGet]
    public async Task<IEnumerable<ElectionOverviewModel>> GetElections()
    {
        var elections = await _electionRepository.GetElectionsForCurrentOrParentTenant();

        var mapped = new List<ElectionOverviewModel>();
        foreach (var election in elections)
        {
            var mappedElection = _mapper.Map<ElectionOverviewModel>(election);
            mappedElection.IsArchived = election.IsArchived(_clock);
            mapped.Add(mappedElection);
        }

        return mapped;
    }

    [HttpGet("{id:Guid}")]
    public async Task<ElectionModel> GetElection([Required] Guid id)
    {
        var election = await _electionRepository.Get(id);

        var mapped = _mapper.Map<ElectionModel>(election);
        mapped.IsArchived = election.IsArchived(_clock);
        return mapped;
    }

    [HttpPost]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<ElectionModel> CreateElection([FromBody] CreateElectionModel electionModel)
    {
        var election = _mapper.Map<Election>(electionModel);
        election.TenantId = _authService.GetTenantId();

        ValidateInfoTexts(election.InfoTexts);
        RemoveTimeFromDates(election);
        await ValidateDomainOfInfluences(election.DomainsOfInfluence);

        await _fileValidationService.ValidateFile(election.TenantLogo, AllowedLogoMimeTypes, Request.HttpContext.RequestAborted);
        foreach (var document in electionModel.Documents ?? [])
        {
            await _fileValidationService.ValidateFile(document.Name, document.Document, AllowedDocumentsMimeTypes, Request.HttpContext.RequestAborted);
        }

        var created = await _electionRepository.Create(election);
        var mapped = _mapper.Map<ElectionModel>(created);
        mapped.IsArchived = created.IsArchived(_clock);
        return mapped;
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<ElectionOverviewModel> UpdateElection(
        [Required] Guid id,
        [FromBody] UpdateElectionModel electionModel)
    {
        var existing = await _electionRepository.GetSimpleElection(id);

        if (existing.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        _authService.AssertAdminOnElection(existing);
        await _fileValidationService.ValidateFile(electionModel.TenantLogo, AllowedLogoMimeTypes, Request.HttpContext.RequestAborted);

        var election = _mapper.Map<Election>(electionModel);
        election.Id = id;
        election.TenantId = _authService.GetTenantId();
        RemoveTimeFromDates(election);
        return _mapper.Map<ElectionOverviewModel>(await _electionRepository.Update(election));
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task DeleteElection([Required] Guid id)
    {
        var existing = await _electionRepository.GetSimpleElection(id);

        if (existing.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        _authService.AssertAdminOnElection(existing);
        await _electionRepository.Delete(id);
    }

    private void ValidateInfoTexts(ICollection<InfoText> infoTexts)
    {
        if (infoTexts == null)
        {
            return;
        }

        var keys = infoTexts.Select(x => x.Key).ToList();
        if (keys.Count > keys.Distinct().Count())
        {
            throw new DuplicateInfoTextKeysException();
        }

        foreach (var infoText in infoTexts)
        {
            // Info texts on an election always belong to the "creator tenant"
            infoText.TenantId = _authService.GetTenantId();
        }
    }

    private async Task ValidateDomainOfInfluences(ICollection<DomainOfInfluenceElection> domainOfInfluenceElections)
    {
        var doiIds = domainOfInfluenceElections.Select(e => e.DomainOfInfluenceId);
        var availableDoiIds = (await _domainOfInfluenceRepository.GetAll())
            .Select(doi => doi.Id)
            .ToList();

        if (doiIds.Any(doiId => !availableDoiIds.Contains(doiId)))
        {
            throw new ForbiddenException("Inaccessible domain of influence provided");
        }
    }

    private void RemoveTimeFromDates(Election election)
    {
        election.AvailableFrom = election.AvailableFrom?.Date;
        election.ContestDate = election.ContestDate.Date;
        election.SubmissionDeadlineBegin = election.SubmissionDeadlineBegin.Date;
        election.SubmissionDeadlineEnd = election.SubmissionDeadlineEnd.Date;
    }
}
