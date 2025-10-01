// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
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

namespace Eawv.Service.Controllers;

[Route("api/infotexts")]
[ApiController]
[Authorize(Roles = Role.All)]
public class InfoTextController : ControllerBase
{
    private readonly InfoTextRepository _infoTextRepo;
    private readonly ElectionRepository _electionRepository;
    private readonly ITenantService _tenantService;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;

    public InfoTextController(
        InfoTextRepository infoTextRepo,
        ElectionRepository electionRepository,
        ITenantService tentantService,
        AuthService authService,
        IMapper mapper)
    {
        _infoTextRepo = infoTextRepo;
        _electionRepository = electionRepository;
        _tenantService = tentantService;
        _authService = authService;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns the best matching info text for the current mandant/election (if present) for each key.
    /// </summary>
    /// <param name="electionId">The Guid of the election.</param>
    /// <returns>A collection of info texts.</returns>
    [HttpGet]
    public async Task<IEnumerable<InfoTextModel>> GetInfoTexts(Guid? electionId)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        return _mapper.Map<IEnumerable<InfoTextModel>>(await _infoTextRepo.GetBestMatching(electionId, tenantId));
    }

    [HttpGet("{id:Guid}")]
    public async Task<InfoTextModel> GetInfoText(Guid id)
    {
        return _mapper.Map<InfoTextModel>(await _infoTextRepo.Get(id));
    }

    [HttpGet("{key}")]
    public async Task<InfoTextModel> GetInfoTextByKey(string key, Guid? electionId)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        return _mapper.Map<InfoTextModel>(await _infoTextRepo.GetBestMatching(key, electionId, tenantId));
    }

    [HttpPost]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<InfoTextModel> CreateOrUpdateInfoText([FromBody] ModifyInfoTextModel infoTextModel)
    {
        var infoText = _mapper.Map<InfoText>(infoTextModel);

        if (infoText.ElectionId != null)
        {
            var el = await _electionRepository.GetSimpleElection(infoText.ElectionId.Value);
            _authService.AssertAdminOnElection(el);
        }

        infoText.TenantId = _authService.GetTenantId();
        return _mapper.Map<InfoTextModel>(await _infoTextRepo.CreateOrUpdate(infoText));
    }

    [HttpPost("batch")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<IEnumerable<InfoTextModel>> CreateOrUpdateInfoTextBatch([FromBody] IEnumerable<ModifyInfoTextModel> infoTextModels)
    {
        var infoTexts = _mapper.Map<List<InfoText>>(infoTextModels);

        var tenantId = _authService.GetTenantId();
        foreach (var infoText in infoTexts)
        {
            infoText.TenantId = tenantId;
        }

        var keys = infoTexts.Select(x => x.Key).Distinct();
        if (infoTexts.Count > keys.Count())
        {
            throw new DuplicateInfoTextKeysException();
        }

        var electionIds = infoTexts.Where(t => t.ElectionId != null).Select(t => t.ElectionId.Value).Distinct();
        foreach (var e in await _electionRepository.GetSimpleElections(electionIds))
        {
            _authService.AssertAdminOnElection(e);
        }

        return _mapper.Map<IEnumerable<InfoTextModel>>(await _infoTextRepo.CreateOrUpdate(infoTexts));
    }
}
