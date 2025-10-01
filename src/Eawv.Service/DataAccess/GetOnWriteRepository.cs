// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public abstract class GetOnWriteRepository<TEntity> : BaseRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    protected GetOnWriteRepository(EawvContext context, AuthService authService, IClock clock)
        : base(context, authService, clock)
    {
    }

    public override async Task<TEntity> Create(TEntity entity)
    {
        var saved = await base.Create(entity);
        return await Get(saved.Id);
    }

    public override async Task<TEntity> Update(TEntity entity, bool ignoreTransaction, bool reload)
    {
        var saved = await base.Update(entity, ignoreTransaction, false);

        // to enforce clean get
        Detach(saved.Id);

        return await Get(saved.Id);
    }
}
