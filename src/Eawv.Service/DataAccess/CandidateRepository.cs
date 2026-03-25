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

        var candidatesToKeep = candidates.Select(c => c.Id).ToHashSet();
        var candidatesToDelete = existingCandidates.Where(c => !candidatesToKeep.Contains(c.Id)).ToList();
        DbSet.RemoveRange(candidatesToDelete);

        foreach (var candidate in candidates)
        {
            var existing = existingCandidates.SingleOrDefault(c => c.Id == candidate.Id);
            if (existing == null)
            {
                SetCreationFields(candidate.MarkedElements);
                await Create(candidate);
            }
            else
            {
                if (AreCandidatesEqual(existing, candidate))
                {
                    continue;
                }

                SetCreatedModifiedFields(candidate.MarkedElements, existing.MarkedElements);

                // Restore correct creation parameters for already existing candidates
                candidate.CreatedBy = existing.CreatedBy;
                candidate.CreationDate = existing.CreationDate;
                await Update(candidate);
            }
        }

        await IncreaseListVersion(listId);

        await Save(true);
        transaction.Complete();
    }

    private void SetCreatedModifiedFields(ICollection<MarkedElement> markedElements, ICollection<MarkedElement> existingElements)
    {
        var existingFields = existingElements.ToDictionary(e => e.Field);
        foreach (var markedElement in markedElements)
        {
            if (existingFields.TryGetValue(markedElement.Field, out var existing))
            {
                markedElement.CreatedBy = existing.CreatedBy;
                markedElement.CreationDate = existing.CreationDate;
                SetModifiedFields(markedElement);
            }
            else
            {
                SetCreationFields(markedElement);
            }
        }
    }

    private bool AreCandidatesEqual(Candidate existing, Candidate candidate)
    {
        var equal = existing.FamilyName == candidate.FamilyName &&
            existing.FirstName == candidate.FirstName &&
            existing.Title == candidate.Title &&
            existing.OccupationalTitle == candidate.OccupationalTitle &&
            existing.DateOfBirth == candidate.DateOfBirth &&
            existing.Sex == candidate.Sex &&
            existing.Origin == candidate.Origin &&
            existing.Street == candidate.Street &&
            existing.HouseNumber == candidate.HouseNumber &&
            existing.ZipCode == candidate.ZipCode &&
            existing.Locality == candidate.Locality &&
            existing.BallotFamilyName == candidate.BallotFamilyName &&
            existing.BallotFirstName == candidate.BallotFirstName &&
            existing.BallotOccupationalTitle == candidate.BallotOccupationalTitle &&
            existing.BallotLocality == candidate.BallotLocality &&
            existing.Incumbent == candidate.Incumbent &&
            existing.Cloned == candidate.Cloned &&
            existing.Index == candidate.Index &&
            existing.OrderIndex == candidate.OrderIndex &&
            existing.CloneOrderIndex == candidate.CloneOrderIndex &&
            existing.Party == candidate.Party;

        if (!equal)
        {
            return false;
        }

        if (existing.MarkedElements.Count != candidate.MarkedElements.Count)
        {
            return false;
        }

        var existingFields = existing.MarkedElements.Select(m => m.Field).ToHashSet();
        return candidate.MarkedElements.All(m => existingFields.Contains(m.Field));
    }

    private async Task IncreaseListVersion(Guid listId)
    {
        var list = await Context.Lists.FindAsync(listId);
        list!.Version++;
    }
}
