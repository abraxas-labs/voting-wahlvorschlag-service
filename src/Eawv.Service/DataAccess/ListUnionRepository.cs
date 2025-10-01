// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class ListUnionRepository : BaseRepository<ListUnion>
{
    public ListUnionRepository(EawvContext context, AuthService authService, IClock clock)
        : base(context, authService, clock)
    {
    }

    public async Task<ListUnion> KeepFirst(List<ListUnion> unions)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        Context.ListUnions.RemoveRange(unions.Skip(1));
        var union = await Update(unions[0]);

        await Save(true);
        transaction.Complete();

        return union;
    }
}
