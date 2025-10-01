// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eawv.Service.Controllers;

[Route("api/domainofinfluences")]
[ApiController]
[Authorize(Roles = Role.All)]
public class DomainOfInfluenceController : ControllerBase
{
    private readonly DomainOfInfluenceRepository _domainOfInfluenceRepository;
    private readonly IMapper _mapper;
    private readonly AuthService _authService;

    public DomainOfInfluenceController(
        DomainOfInfluenceRepository domainOfInfluenceRepository,
        IMapper mapper,
        AuthService authService)
    {
        _domainOfInfluenceRepository = domainOfInfluenceRepository;
        _mapper = mapper;
        _authService = authService;
    }

    [HttpGet]
    public async Task<IEnumerable<DomainOfInfluenceModel>> GetDomainOfInfluences()
    {
        return _mapper.Map<IEnumerable<DomainOfInfluenceModel>>(await _domainOfInfluenceRepository.GetAll());
    }

    [HttpGet("{id:Guid}")]
    public async Task<DomainOfInfluenceModel> GetDomainOfInfluence(Guid id)
    {
        return _mapper.Map<DomainOfInfluenceModel>(await _domainOfInfluenceRepository.Get(id));
    }

    [HttpPost]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<DomainOfInfluenceModel> CreateDomainOfInfluence(
        [FromBody] ModifyDomainOfInfluenceModel domainOfInfluenceModel)
    {
        var domainOfInfluence = _mapper.Map<DomainOfInfluence>(domainOfInfluenceModel);
        domainOfInfluence.TenantId = _authService.GetTenantId();
        return _mapper.Map<DomainOfInfluenceModel>(await _domainOfInfluenceRepository.Create(domainOfInfluence));
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<DomainOfInfluenceModel> UpdateDomainOfInfluence(
        Guid id,
        [FromBody] ModifyDomainOfInfluenceModel domainOfInfluenceModel)
    {
        var existing = await _domainOfInfluenceRepository.Get(id);
        if (existing.TenantId != _authService.GetTenantId())
        {
            throw new ForbiddenException();
        }

        var domainOfInfluence = _mapper.Map<DomainOfInfluence>(domainOfInfluenceModel);
        domainOfInfluence.Id = id;
        domainOfInfluence.TenantId = existing.TenantId;

        return _mapper.Map<DomainOfInfluenceModel>(await _domainOfInfluenceRepository.Update(domainOfInfluence));
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task DeleteDomainOfInfluence(Guid id)
    {
        var existing = await _domainOfInfluenceRepository.Get(id);
        if (existing.TenantId != _authService.GetTenantId())
        {
            throw new ForbiddenException();
        }

        await _domainOfInfluenceRepository.Delete(id);
    }
}
