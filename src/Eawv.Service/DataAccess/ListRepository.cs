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

public class ListRepository : GetOnWriteRepository<List>
{
    public ListRepository(EawvContext context, AuthService auth, IClock clock)
        : base(context, auth, clock)
    {
    }

    public async Task<IEnumerable<List>> GetListsForElection(Guid electionId)
    {
        var authPredicate = AuthService.ReadListPermissionsPredicate();

        return await Context.Elections
            .Where(e => e.Id == electionId)
            .SelectMany(e => e.Lists)
            .Include(l => l.ListUnion)
                .ThenInclude(lu => lu.UnionLists)
            .Include(l => l.ListSubUnion)
                .ThenInclude(lu => lu.SubUnionLists)
            .Where(authPredicate)
            .ToListAsync();
    }

    public async Task<IList<List>> GetListsForUnions(Guid electionId, List<Guid> ids)
    {
        // no access control, since all callers do this already
        return await Context.Lists
            .Include(l => l.ListUnion)
            .ThenInclude(lu => lu.UnionLists)
            .Include(l => l.ListSubUnion)
            .ThenInclude(lu => lu.SubUnionLists)
            .Where(l => l.ElectionId == electionId && ids.Contains(l.Id))
            .ToListAsync();
    }

    public async Task<List> Get(Guid electionId, Guid id)
    {
        var list = await Context.Lists
                       .Include(l => l.Election)
                       .Include(l => l.Candidates)
                       .Include(l => l.ListUnion)
                        .ThenInclude(lu => lu.UnionLists)
                       .Include(l => l.ListSubUnion)
                        .ThenInclude(lu => lu.SubUnionLists)
                       .SingleOrDefaultAsync(l => l.Id == id) ?? throw new EntityNotFoundException(id);

        AuthService.AssertListReadPermissions(list);

        if (electionId != Guid.Empty && electionId != list.ElectionId)
        {
            throw new EntityNotFoundException(id);
        }

        return list;
    }

    public override async Task<List> Get(Guid id) => await Get(Guid.Empty, id);

    public override async Task Delete(Guid id)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var list = await Context.Lists
            .Where(l => l.Id == id)
            .Include(l => l.ListSubUnion)
            .Include(l => l.ListUnion)
            .SingleOrDefaultAsync(l => l.Id == id);

        if (list == null)
        {
            throw new EntityNotFoundException(id);
        }

        if (ShouldRemoveUnion(list.ListUnion))
        {
            Context.ListUnions.Remove(list.ListUnion);
        }

        if (ShouldRemoveUnion(list.ListSubUnion))
        {
            Context.ListUnions.Remove(list.ListSubUnion);
        }

        await base.Delete(id);

        await Save(true);
        transaction.Complete();

        static bool ShouldRemoveUnion(ListUnion union) => union?.Lists.Count <= 2;
    }

    public override async Task<List> Create(List entity)
    {
        if (entity.SortOrder == 0)
        {
            entity.SortOrder = await Context.Lists.CountAsync(l => l.ElectionId == entity.ElectionId);
        }

        return await base.Create(entity);
    }
}
