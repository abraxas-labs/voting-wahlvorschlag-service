// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class DomainOfInfluenceElectionRepository : GetOnWriteRepository<DomainOfInfluenceElection>
{
    public DomainOfInfluenceElectionRepository(EawvContext context, AuthService auth, IClock clock)
        : base(context, auth, clock)
    {
    }

    public override async Task<DomainOfInfluenceElection> Update(DomainOfInfluenceElection doi)
    {
        var entity = await Context.DomainOfInfluenceElections
            .SingleOrDefaultAsync(d =>
                d.ElectionId == doi.ElectionId && d.DomainOfInfluenceId == doi.DomainOfInfluenceId);

        if (entity == null)
        {
            throw new EntityNotFoundException(doi.ElectionId, doi.DomainOfInfluenceId);
        }

        entity.NumberOfMandates = doi.NumberOfMandates;

        return await base.Update(entity);
    }

    public async Task Delete(Guid electionId, Guid domainOfInfluenceId)
    {
        var entity = await Context.DomainOfInfluenceElections
            .SingleOrDefaultAsync(doi =>
                doi.ElectionId == electionId && doi.DomainOfInfluenceId == domainOfInfluenceId);

        if (entity == null)
        {
            throw new EntityNotFoundException(electionId, domainOfInfluenceId);
        }

        Context.DomainOfInfluenceElections.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
