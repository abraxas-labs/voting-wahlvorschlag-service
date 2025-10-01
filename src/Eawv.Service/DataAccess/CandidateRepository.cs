// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class CandidateRepository : BaseRepository<Candidate>
{
    public CandidateRepository(EawvContext context, AuthService auth, IClock clock)
        : base(context, auth, clock)
    {
        IgnoredModifyProperties.Add(c => c.Index);
    }

    public async Task<IEnumerable<Candidate>> GetCandidatesForList(Guid listId)
    {
        Func<List, bool> authListFunc = AuthService.ReadListPermissionsPredicate().Compile();

        var candidates = await Context.Candidates
            .Include(c => c.MarkedElements)
            .Include(c => c.List)
                .ThenInclude(c => c.Election)
            .Where(c => c.ListId == listId)
            .ToListAsync();

        // do this locally, otherwise EF Core gets confused
        return candidates
            .Where(c => authListFunc(c.List))
            .OrderBy(c => c.OrderIndex)
            .ToList();
    }

    public override async Task<Candidate> Get(Guid id)
    {
        var candidate = await Context.Candidates
            .Include(c => c.MarkedElements)
            .Include(c => c.List)
                .ThenInclude(l => l.Election)
            .SingleOrDefaultAsync(l => l.Id == id) ?? throw new EntityNotFoundException(id);
        AuthService.AssertListReadPermissions(candidate.List);
        return candidate;
    }

    public async Task UpdateAll(Guid listId, IList<Candidate> candidates)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var existingCandidates = await DbSet
            .Include(c => c.MarkedElements)
            .Where(c => c.ListId == listId)
            .ToListAsync();
        DbSet.RemoveRange(existingCandidates);

        foreach (var candidate in candidates)
        {
            SetCreationFields(candidate.MarkedElements);

            // Restore correct creation parameters for already existing candidates
            var existing = existingCandidates.SingleOrDefault(c => c.Id == candidate.Id);
            if (existing != null)
            {
                candidate.CreatedBy = existing.CreatedBy;
                candidate.CreationDate = existing.CreationDate;
                candidate.ModifiedBy = AuthService.GetUserId();
                candidate.ModifiedDate = Clock.UtcNow;
            }

            await Create(candidate);
        }

        await IncreaseListVersion(listId);

        await Save(true);
        transaction.Complete();
    }

    private async Task IncreaseListVersion(Guid listId)
    {
        var list = await Context.Lists.FindAsync(listId);
        list!.Version++;
    }
}
