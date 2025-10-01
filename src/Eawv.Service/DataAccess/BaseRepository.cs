// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public abstract class BaseRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    protected BaseRepository(EawvContext context, AuthService authService, IClock clock)
    {
        Context = context;
        AuthService = authService;
        Clock = clock;
        DbSet = context.Set<TEntity>();
        IgnoredModifyProperties =
        [
            x => x.CreatedBy,
            x => x.CreationDate,
        ];
    }

    protected EawvContext Context { get; }

    protected AuthService AuthService { get; }

    protected IClock Clock { get; }

    protected DbSet<TEntity> DbSet { get; }

    protected List<Expression<Func<TEntity, dynamic>>> IgnoredModifyProperties { get; }

    protected bool HasActiveTransaction => Transaction.Current != null;

    public virtual async Task<TEntity> Create(TEntity entity)
    {
        entity.CreationDate = Clock.UtcNow;
        entity.CreatedBy = AuthService.GetUserId();

        Context.Add(entity);

        await Save();

        return entity;
    }

    public virtual async Task<TEntity> Get(Guid id)
    {
        TEntity entity = await DbSet.FindAsync(id);
        return entity ?? throw new EntityNotFoundException(id);
    }

    public virtual async Task<TEntity> Update(TEntity entity)
    {
        return await Update(entity, false, true);
    }

    public virtual async Task<TEntity> Update(TEntity entity, bool ignoreTransaction, bool reload)
    {
        entity.ModifiedDate = Clock.UtcNow;
        entity.ModifiedBy = AuthService.GetUserId();

        Detach(entity.Id);

        var entry = DbSet.Attach(entity);
        entry.CurrentValues.SetValues(entity);
        entry.State = EntityState.Modified;
        foreach (var ignoredProperty in IgnoredModifyProperties)
        {
            entry.Property(ignoredProperty).IsModified = false;
        }

        var saved = await Save(ignoreTransaction);
        if (saved && reload)
        {
            await entry.ReloadAsync();
        }

        return entity;
    }

    public virtual async Task Delete(Guid id)
    {
        Detach(id);

        var entity = new TEntity { Id = id };
        DbSet.Attach(entity);
        DbSet.Remove(entity);

        await Save();
    }

    protected async Task<bool> Save(bool ignoreTransaction = false)
    {
        if (!ignoreTransaction && HasActiveTransaction)
        {
            return false;
        }

        await Context.SaveChangesAsync();
        return true;
    }

    protected void Detach(Guid id)
    {
        var local = DbSet.Local
            .FirstOrDefault(e => e.Id == id);
        if (local != null)
        {
            Context.Entry(local).State = EntityState.Detached;
        }
    }

    protected void SetCreationFields<T>(ICollection<T> collection)
        where T : BaseEntity
    {
        if (collection == null)
        {
            return;
        }

        foreach (var entity in collection)
        {
            entity.CreatedBy = AuthService.GetUserId();
            entity.CreationDate = Clock.UtcNow;
        }
    }
}
