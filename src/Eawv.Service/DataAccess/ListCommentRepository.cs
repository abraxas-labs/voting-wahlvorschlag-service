// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class ListCommentRepository : BaseRepository<ListComment>
{
    public ListCommentRepository(EawvContext context, AuthService auth, IClock clock)
        : base(context, auth, clock)
    {
    }

    public async Task<IEnumerable<ListComment>> GetCommentsForList(Guid listId)
    {
        var authPredicate = AuthService.ReadListPermissionsPredicate();

        return await Context.Lists
            .Where(e => e.Id == listId)
            .Where(authPredicate)
            .SelectMany(e => e.Comments)
            .OrderByDescending(c => c.CreationDate)
            .ToListAsync();
    }

    public override async Task<ListComment> Get(Guid id)
    {
        var comment = await Context.ListComments
            .Include(c => c.List)
                .ThenInclude(l => l.Election)
            .SingleOrDefaultAsync(l => l.Id == id) ?? throw new EntityNotFoundException(id);
        AuthService.AssertListReadPermissions(comment.List);
        return comment;
    }

    public async Task Delete(Guid listId, Guid id)
    {
        var comment = await DbSet.SingleOrDefaultAsync(c => c.Id == id && c.ListId == listId);

        if (comment == null)
        {
            throw new EntityNotFoundException(listId, id);
        }

        DbSet.Remove(comment);
        await Context.SaveChangesAsync();
    }
}
