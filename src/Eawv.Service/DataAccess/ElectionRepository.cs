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

public class ElectionRepository : GetOnWriteRepository<Election>
{
    private readonly ITenantService _tenantService;

    public ElectionRepository(EawvContext context, AuthService authService, ITenantService tenantService, IClock clock)
        : base(context, authService, clock)
    {
        _tenantService = tenantService;
    }

    public async Task<List<Election>> GetElectionsForCurrentOrParentTenant()
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        return await Context.Elections
            .Where(AuthService.ReadElectionPermissionsPredicate(tenantId))
            .ToListAsync();
    }

    public override async Task<Election> Get(Guid id)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        var entity = await Context.Elections
            .Include(e => e.DomainsOfInfluence)
                .ThenInclude(doi => doi.DomainOfInfluence)
            .Include(e => e.Documents)
            .Include(e => e.InfoTexts)
            .Where(AuthService.ReadElectionPermissionsPredicate(tenantId))
            .SingleOrDefaultAsync(x => x.Id == id);
        return entity ?? throw new EntityNotFoundException(id);
    }

    // returns the election including lists and candidates
    public async Task<Election> GetEntireElection(Guid id)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        var entity = await Context.Elections.AsSplitQuery()
            .Include(e => e.DomainsOfInfluence)
                .ThenInclude(doi => doi.DomainOfInfluence)
            .Include(e => e.Documents)
            .Include(e => e.InfoTexts)
            .Include(e => e.Lists.AsQueryable().Where(AuthService.ReadListPermissionsPredicate()))
                .ThenInclude(l => l.Candidates.OrderBy(c => c.OrderIndex))
            .Include(e => e.Lists)
                .ThenInclude(l => l.ListUnion)
            .Include(e => e.Lists)
                .ThenInclude(l => l.ListSubUnion)
            .Where(AuthService.ReadElectionPermissionsPredicate(tenantId))
            .SingleOrDefaultAsync(x => x.Id == id);
        return entity ?? throw new EntityNotFoundException(id);
    }

    public async Task<Election> GetSimpleElection(Guid id)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        return await Context.Elections
            .Where(AuthService.ReadElectionPermissionsPredicate(tenantId))
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new EntityNotFoundException(id);
    }

    public async Task<int> GetTotalNumberOfMandates(Guid id)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        return await Context.Elections
            .Where(e => e.Id == id)
            .Where(AuthService.ReadElectionPermissionsPredicate(tenantId))
            .SelectMany(e => e.DomainsOfInfluence)
            .SumAsync(doi => doi.NumberOfMandates);
    }

    public async Task<IEnumerable<Election>> GetSimpleElections(IEnumerable<Guid> ids)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        var list = await Context.Elections
            .Where(x => ids.Contains(x.Id))
            .Where(AuthService.ReadElectionPermissionsPredicate(tenantId))
            .ToListAsync();

        if (list.Count != ids.Count())
        {
            throw new EntityNotFoundException();
        }

        return list;
    }

    public override async Task<Election> Create(Election election)
    {
        SetCreationFields(election.DomainsOfInfluence);
        SetCreationFields(election.Documents);
        SetCreationFields(election.InfoTexts);
        return await base.Create(election);
    }
}
