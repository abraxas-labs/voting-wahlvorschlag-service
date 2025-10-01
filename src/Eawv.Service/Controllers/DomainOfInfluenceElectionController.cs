// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eawv.Service.Controllers;

[Route("api/elections/{electionId:Guid}/domainofinfluences")]
[ApiController]
[Authorize(Roles = Role.All)]
public class DomainOfInfluenceElectionController
{
    private readonly DomainOfInfluenceElectionRepository _domainOfInfluenceElectionRepository;
    private readonly DomainOfInfluenceRepository _domainOfInfluenceRepository;
    private readonly ElectionRepository _electionRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;

    public DomainOfInfluenceElectionController(
        DomainOfInfluenceElectionRepository domainOfInfluenceElectionRepository,
        DomainOfInfluenceRepository domainOfInfluenceRepository,
        ElectionRepository electionRepository,
        AuthService authService,
        IMapper mapper)
    {
        _domainOfInfluenceElectionRepository = domainOfInfluenceElectionRepository;
        _domainOfInfluenceRepository = domainOfInfluenceRepository;
        _electionRepository = electionRepository;
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<DomainOfInfluenceElectionModel> CreateDomainOfInfluenceElection(
        Guid electionId,
        [FromBody] CreateDomainOfInfluenceElectionModel doiModel)
    {
        var election = await _electionRepository.GetSimpleElection(electionId);
        _authService.AssertAdminOnElection(election);

        await EnsureHasDomainOfInfluencePermission(doiModel.Id);

        var doi = _mapper.Map<DomainOfInfluenceElection>(doiModel);
        doi.ElectionId = election.Id;

        return _mapper.Map<DomainOfInfluenceElectionModel>(await _domainOfInfluenceElectionRepository.Create(doi));
    }

    [HttpPut("{domainOfInfluenceId:Guid}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<DomainOfInfluenceElectionModel> UpdateDomainOfInfluenceElection(
        Guid electionId,
        Guid domainOfInfluenceId,
        [FromBody] UpdateDomainOfInfluenceElectionModel doiModel)
    {
        var election = await _electionRepository.GetSimpleElection(electionId);
        _authService.AssertAdminOnElection(election);

        var doi = _mapper.Map<DomainOfInfluenceElection>(doiModel);
        doi.ElectionId = election.Id;
        doi.DomainOfInfluenceId = domainOfInfluenceId;

        return _mapper.Map<DomainOfInfluenceElectionModel>(await _domainOfInfluenceElectionRepository.Update(doi));
    }

    [HttpDelete("{domainOfInfluenceId:Guid}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task DeleteDomainOfInfluenceElection(Guid electionId, Guid domainOfInfluenceId)
    {
        var election = await _electionRepository.GetSimpleElection(electionId);
        _authService.AssertAdminOnElection(election);

        await _domainOfInfluenceElectionRepository.Delete(electionId, domainOfInfluenceId);
    }

    private async Task EnsureHasDomainOfInfluencePermission(Guid domainOfInfluenceId)
    {
        _ = await _domainOfInfluenceRepository.Get(domainOfInfluenceId);
    }
}
