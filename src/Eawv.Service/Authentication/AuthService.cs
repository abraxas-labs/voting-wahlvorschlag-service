// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Voting.Lib.Common;
using Voting.Lib.Iam.Store;
using Voting.Lib.Iam.TokenHandling;

namespace Eawv.Service.Authentication;

public class AuthService
{
    private readonly ITokenHandler _servicetokenHandler;
    private readonly IAuth _auth;
    private readonly IClock _clock;

    public AuthService(
        ITokenHandler servicetokenHandler,
        IAuth auth,
        IClock clock)
    {
        _servicetokenHandler = servicetokenHandler;
        _auth = auth;
        _clock = clock;
    }

    public bool IsWahlverwalter => _auth.HasRole(Role.Wahlverwalter);

    public bool IsUser => _auth.HasRole(Role.User);

    public string GetTenantId()
    {
        return _auth.Tenant.Id;
    }

    public string GetUserId()
    {
        return _auth.User.Loginid;
    }

    public void AssertAdminOnElection(Election e)
    {
        if (!IsWahlverwalter || e.TenantId != GetTenantId())
        {
            throw new ForbiddenException(e.Id);
        }
    }

    public Expression<Func<Election, bool>> ReadElectionPermissionsPredicate(string tenantId)
    {
        return e =>
            e.TenantId == tenantId
            && (IsWahlverwalter
                || e.AvailableFrom == null
                || e.AvailableFrom < _clock.UtcNow);
    }

    public void AssertListWriteAccess(List list)
    {
        // if wahlverwalter, the election must be of his own tenant
        if (IsWahlverwalter && list.Election.TenantId == GetTenantId())
        {
            return;
        }

        if (list.Locked)
        {
            throw new ListLockedException(list.Id);
        }

        if (list.Election.AvailableFrom.HasValue && list.Election.AvailableFrom > _clock.UtcNow)
        {
            throw new ElectionNotAvailableException(list.Election.Id, list.Election.AvailableFrom.Value);
        }

        // if user => list must be of the same tenant,
        // user must be the creator or representative or a deputy
        if (IsUser &&
            list.ResponsiblePartyTenantId == GetTenantId() &&
            (list.CreatedBy == GetUserId()
             || list.Representative == GetUserId()
             || list.DeputyUsers.Contains(GetUserId())))
        {
            return;
        }

        throw new ForbiddenException(list.Id);
    }

    public void AssertListReadPermissions(List list)
    {
        var hasRights = ReadListPermissionsPredicate().Compile().Invoke(list);
        if (!hasRights)
        {
            throw new ForbiddenException(list.Id);
        }
    }

    public Expression<Func<List, bool>> ReadListPermissionsPredicate()
    {
        var tenant = GetTenantId();
        if (IsWahlverwalter)
        {
            return l => l.Election.TenantId == tenant;
        }

        var userId = GetUserId();
        return l =>
            l.ResponsiblePartyTenantId == tenant
            && (l.Election.AvailableFrom == null || l.Election.AvailableFrom <= _clock.UtcNow)
            && (l.CreatedBy == userId
                || l.Representative == userId
                || l.MemberUsers.Contains(userId)
                || l.DeputyUsers.Contains(userId));
    }

    internal async Task<string> GetServiceToken()
    {
        return await _servicetokenHandler.GetToken(CancellationToken.None);
    }
}
