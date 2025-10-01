// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Services;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class DomainOfInfluenceRepository : BaseRepository<DomainOfInfluence>
{
    private readonly ITenantService _tenantService;

    public DomainOfInfluenceRepository(EawvContext context, AuthService authService, ITenantService tenantService, IClock clock)
        : base(context, authService, clock)
    {
        _tenantService = tenantService;
    }

    public async Task<IEnumerable<DomainOfInfluence>> GetAll()
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        return await Context.DomainsOfInfluence.Where(doi => doi.TenantId == tenantId)
            .ToListAsync();
    }

    public override async Task<DomainOfInfluence> Get(Guid id)
    {
        var doi = await base.Get(id);
        if (doi.TenantId != await _tenantService.GetParentOrCurrentTenantId())
        {
            throw new ForbiddenException(id);
        }

        return doi;
    }
}
