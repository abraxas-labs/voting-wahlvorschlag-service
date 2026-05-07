// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Services;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class BallotDocumentRepository : BaseRepository<BallotDocument>
{
    private readonly ITenantService _tenantService;

    public BallotDocumentRepository(EawvContext context, AuthService auth, ITenantService tenantService, IClock clock)
        : base(context, auth, clock)
    {
        _tenantService = tenantService;
    }

    public override async Task<BallotDocument> Get(Guid id)
    {
        var relevantTenantId = await _tenantService.GetParentOrCurrentTenantId();
        var electionPredicate = AuthService.ReadElectionPermissionsPredicate(relevantTenantId);
        var entity = await Context.BallotDocuments
            .SingleOrDefaultAsync(e => e.Id == id && Context.Elections.Where(electionPredicate).Any(el => el.Id == e.ElectionId));
        return entity ?? throw new EntityNotFoundException(id);
    }
}
