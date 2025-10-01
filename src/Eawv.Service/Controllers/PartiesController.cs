// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.Models;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eawv.Service.Controllers;

[Route("api/parties")]
[ApiController]
[Authorize(Roles = Role.Wahlverwalter)]
public class PartiesController
{
    private readonly ITenantService _tenantService;
    private readonly IMapper _mapper;

    public PartiesController(ITenantService tenantService, IMapper mapper)
    {
        _tenantService = tenantService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<PartyModel>> GetParties()
    {
        return _mapper.Map<IEnumerable<PartyModel>>(await _tenantService.GetParties());
    }

    [HttpPost]
    public Task CreateParty([FromBody] ModifyPartyModel model)
    {
        return _tenantService.CreateParty(model.Name);
    }

    [HttpDelete("{id}")]
    public async Task DeleteParty(string id)
    {
        await _tenantService.RemoveParty(id);
    }
}
